using Engine;
using Engine.Graphics;
using Game;
using System.Collections.Generic;

namespace Mekiasm
{
    public class ItemHFluid:Item
    {
		public BoundingBox[][] m_boundingBoxesByLevel = new BoundingBox[16][];
		public Color topcolor;
        public Color sidecolor;
		public float[] m_heightByLevel = new float[16];
		public ItemHFluid(int d,string n,int maxlevel,Color top,Color side) : base(d,n) {
			for (int i = 0; i < 16; i++)
			{
				float num = 0.875f * MathUtils.Saturate(1f - (float)i / (float)maxlevel);
				m_heightByLevel[i] = num;
				m_boundingBoxesByLevel[i] = new BoundingBox[1]
				{
				new BoundingBox(new Vector3(0f, 0f, 0f), new Vector3(1f, num, 1f))
				};
			}
			topcolor = top;
            sidecolor = side;            
        }
		public override int GetFaceTextureSlot(int face, int value)
		{
			return 255;
		}
		public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            BlocksManager.DrawCubeBlock(primitivesRenderer, value, new Vector3(size), ref matrix, color, color, environmentData);
        }
        
        public void GenerateTerrainVertices(FluidBlock block, BlockGeometryGenerator generator, TerrainGeometrySubsets geometry, int value, int x, int y, int z)
        {
			int data = Terrain.ExtractData(value);
			if (true)
			{
				Terrain terrain = generator.Terrain;
				int cellValueFast = terrain.GetCellValueFast(x - 1, y, z - 1);
				int cellValueFast2 = terrain.GetCellValueFast(x, y, z - 1);
				int cellValueFast3 = terrain.GetCellValueFast(x + 1, y, z - 1);
				int cellValueFast4 = terrain.GetCellValueFast(x - 1, y, z);
				int cellValueFast5 = terrain.GetCellValueFast(x + 1, y, z);
				int cellValueFast6 = terrain.GetCellValueFast(x - 1, y, z + 1);
				int cellValueFast7 = terrain.GetCellValueFast(x, y, z + 1);
				int cellValueFast8 = terrain.GetCellValueFast(x + 1, y, z + 1);
				float h = CalculateNeighborHeight(cellValueFast);
				float num = CalculateNeighborHeight(cellValueFast2);
				float h2 = CalculateNeighborHeight(cellValueFast3);
				float num2 = CalculateNeighborHeight(cellValueFast4);
				float num3 = CalculateNeighborHeight(cellValueFast5);
				float h3 = CalculateNeighborHeight(cellValueFast6);
				float num4 = CalculateNeighborHeight(cellValueFast7);
				float h4 = CalculateNeighborHeight(cellValueFast8);
				float levelHeight = GetLevelHeight(GetLevel(data));
				float height = CalculateFluidVertexHeight(h, num, num2, levelHeight);
				float height2 = CalculateFluidVertexHeight(num, h2, levelHeight, num3);
				float height3 = CalculateFluidVertexHeight(levelHeight, num3, num4, h4);
				float height4 = CalculateFluidVertexHeight(num2, levelHeight, h3, num4);
				generator.GenerateCubeVertices(block, value, x, y, z, height, height2, height3, height4, sidecolor, topcolor, topcolor, topcolor, topcolor, 255, SubsystemItemBlockBase.GTV(x,z,geometry).OpaqueSubsetsByFace);
			}
			else
				generator.GenerateCubeVertices(block, value, x, y, z, sidecolor, SubsystemItemBlockBase.GTV(x,z,geometry).OpaqueSubsetsByFace);
		}
		public static float CalculateFluidVertexHeight(float h1, float h2, float h3, float h4)
		{
			float num = MathUtils.Max(h1, h2, h3, h4);
			if (num < 1f)
			{
				if (h1 == 0.01f || h2 == 0.01f || h3 == 0.01f || h4 == 0.01f)
					return 0f;
				return num;
			}
			return 1f;
		}
		// Game.FluidBlock
		public static float ZeroSubst(float v, float subst)
		{
			if (v != 0f)
				return v;
			return subst;
		}
		public static int SetLevel(int data, int level)
		{
			return (data & -16) | (level & 0xF);
		}
		public bool IsTheSameFluid(int contents)
		{
			return false;
		}
		public static bool GetIsTop(int data)
		{
			return (data & 0x10) != 0;
		}
		// Game.FluidBlock
		public static int GetLevel(int data)
		{
			return data & 0xF;
		}
		public float GetLevelHeight(int level)
		{
			return m_heightByLevel[level];
		}


		public float CalculateNeighborHeight(int value)
		{
			int num = Terrain.ExtractContents(value);
			if (IsTheSameFluid(num))
			{
				int data = Terrain.ExtractData(value);
				if (GetIsTop(data))
					return GetLevelHeight(GetLevel(data));
				return 1f;
			}
			if (num == 0)
				return 0.01f;
			return 0f;
		}

	}
}
