using Engine;
using Engine.Graphics;
using Game;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using XmlUtilities;

namespace Mekiasm {
	public class MekClothingBlock : ClothingBlock
	{
		public new const int Index = 1011;
		public static MekClothData[] clothingData;
		public override void Initialize()
		{
			int num = 0;
			Dictionary<int, MekClothData> dictionary = new Dictionary<int, MekClothData>();
			IEnumerator<XElement> enumerator =XmlUtils.LoadXmlFromStream(ModsManager.GetEntries(".xclo")[0].Stream,Encoding.UTF8,true).Elements().GetEnumerator();
			while (enumerator.MoveNext())
			{
				XElement current = enumerator.Current;
				MekClothData clothingData = new MekClothData
				{
					Index = XmlUtils.GetAttributeValue<int>(current, "Index"),
					DisplayIndex = num++,
					DisplayName = XmlUtils.GetAttributeValue<string>(current, "DisplayName"),
					Slot = XmlUtils.GetAttributeValue<ClothingSlot>(current, "Slot"),
					ArmorProtection = XmlUtils.GetAttributeValue<float>(current, "ArmorProtection"),
					Sturdiness = XmlUtils.GetAttributeValue<float>(current, "Sturdiness"),
					Insulation = XmlUtils.GetAttributeValue<float>(current, "Insulation"),
					MovementSpeedFactor = XmlUtils.GetAttributeValue<float>(current, "MovementSpeedFactor"),
					SteedMovementSpeedFactor = XmlUtils.GetAttributeValue<float>(current, "SteedMovementSpeedFactor"),
					DensityModifier = XmlUtils.GetAttributeValue<float>(current, "DensityModifier"),
					IsOuter = XmlUtils.GetAttributeValue<bool>(current, "IsOuter"),
					CanBeDyed = XmlUtils.GetAttributeValue<bool>(current, "CanBeDyed"),
					Layer = XmlUtils.GetAttributeValue<int>(current, "Layer"),
					PlayerLevelRequired = XmlUtils.GetAttributeValue<int>(current, "PlayerLevelRequired"),
					Texture = ContentManager.Get<Texture2D>(XmlUtils.GetAttributeValue<string>(current, "TextureName")),
					ImpactSoundsFolder = XmlUtils.GetAttributeValue<string>(current, "ImpactSoundsFolder"),
					Description = XmlUtils.GetAttributeValue<string>(current, "Description")
				};
				dictionary[clothingData.Index]=clothingData;
			}
			clothingData = new MekClothData[dictionary.Count];
			int i=0;
			foreach (KeyValuePair<int,MekClothData> keyValuePair in dictionary)
			{
				clothingData[i] = keyValuePair.Value;
				++i;
			}
			Model playerModel = CharacterSkinsManager.GetPlayerModel(PlayerClass.Male);
			Matrix[] array = new Matrix[playerModel.Bones.Count];
			playerModel.CopyAbsoluteBoneTransformsTo(array);
			i = playerModel.FindBone("Hand1").Index;
			int index = playerModel.FindBone("Hand2").Index;
			array[i] = Matrix.CreateRotationY(0.1f) * array[i];
			array[index] = Matrix.CreateRotationY(-0.1f) * array[index];
			m_innerMesh = new BlockMesh();
			foreach (ModelMesh mesh in playerModel.Meshes)
			{
				Matrix matrix = array[mesh.ParentBone.Index];
				foreach (ModelMeshPart meshPart in mesh.MeshParts)
				{
					Color color = Color.White * 0.8f;
					color.A = byte.MaxValue;
					m_innerMesh.AppendModelMeshPart(meshPart, matrix, makeEmissive: false, flipWindingOrder: false, doubleSided: false, flipNormals: false, Color.White);
					m_innerMesh.AppendModelMeshPart(meshPart, matrix, makeEmissive: false, flipWindingOrder: true, doubleSided: false, flipNormals: true, color);
				}
			}
			Model outerClothingModel = CharacterSkinsManager.GetOuterClothingModel(PlayerClass.Male);
			Matrix[] array2 = new Matrix[outerClothingModel.Bones.Count];
			outerClothingModel.CopyAbsoluteBoneTransformsTo(array2);
			int index2 = outerClothingModel.FindBone("Leg1").Index;
			int index3 = outerClothingModel.FindBone("Leg2").Index;
			array2[index2] = Matrix.CreateTranslation(-0.02f, 0f, 0f) * array2[index2];
			array2[index3] = Matrix.CreateTranslation(0.02f, 0f, 0f) * array2[index3];
			m_outerMesh = new BlockMesh();
			foreach (ModelMesh mesh2 in outerClothingModel.Meshes)
			{
				Matrix matrix2 = array2[mesh2.ParentBone.Index];
				foreach (ModelMeshPart meshPart2 in mesh2.MeshParts)
				{
					Color color2 = Color.White * 0.8f;
					color2.A = byte.MaxValue;
					m_outerMesh.AppendModelMeshPart(meshPart2, matrix2, makeEmissive: false, flipWindingOrder: false, doubleSided: false, flipNormals: false, Color.White);
					m_outerMesh.AppendModelMeshPart(meshPart2, matrix2, makeEmissive: false, flipWindingOrder: true, doubleSided: false, flipNormals: true, color2);
				}
			}
		}
		public override string GetDescription(int value)
		{
			return GetClothingData(Terrain.ExtractData(value)).Description;
		}
		public override string GetCategory(int value)
		{
			if (GetClothingColor(Terrain.ExtractData(value)) == 0)
				return base.GetCategory(value);
			return "染色的";
		}
		public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)
		{
			int data = Terrain.ExtractData(value);
			MekClothData clothingData = GetClothingData(data);
			int clothingColor = GetClothingColor(data);
			if (clothingColor != 0)
				return SubsystemPalette.GetName(subsystemTerrain, clothingColor, "染色的 " + clothingData.DisplayName);
			return clothingData.DisplayName;
		}
		public new static MekClothData GetClothingData(int data)
		{
			int d = GetClothingIndex(data);
			Log.Information(d);
			if (d >= clothingData.Count()) d = 0;
			return clothingData[d];
		}
		public new static int GetClothingIndex(int data)
		{
			return data & 0xFF;
		}
		public override IEnumerable<int> GetCreativeValues()
		{
			List<int> blocks = new List<int>();
			foreach (MekClothData mekClothData in clothingData)
			{
				int data = SetClothingColor(SetClothingIndex(0, mekClothData.Index), 0);
				blocks.Add(Terrain.MakeBlockValue(Index, 0, data));
			}
			return blocks.ToArray();
		}
		public new static int GetClothingColor(int data)
		{
			return (data >> 12) & 0xF;
		}

		public new static int SetClothingColor(int data, int color)
		{
			return (data & -61441) | ((color & 0xF) << 12);
		}

		public new static int SetClothingIndex(int data, int clothingIndex)
		{
			return (data & -256) | (clothingIndex & 0xFF);
		}
		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
		{
			int data = Terrain.ExtractData(value);
			int clothingColor = GetClothingColor(data);
			MekClothData clothingData = GetClothingData(data);
			Matrix matrix2 = m_slotTransforms[(int)clothingData.Slot] * Matrix.CreateScale(size) * matrix;
			if (clothingData.IsOuter)
				BlocksManager.DrawMeshBlock(primitivesRenderer, m_outerMesh, clothingData.Texture, color * SubsystemPalette.GetFabricColor(environmentData, clothingColor), 1f, ref matrix2, environmentData);
			else
				BlocksManager.DrawMeshBlock(primitivesRenderer, m_innerMesh, clothingData.Texture, color * SubsystemPalette.GetFabricColor(environmentData, clothingColor), 1f, ref matrix2, environmentData);
		}
	}
}
