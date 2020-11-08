using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.Graphics;
using Engine.Media;
using Game;
using GameEntitySystem;
using TemplatesDatabase;

namespace Mekiasm
{
    //静态引用库
    public class ILibrary
    {
        public static Texture2D texture;
        public static readonly Vector4[] m_slotTexCoords = new Vector4[256];
        public static SortedMultiCollection<Point2, TerrainGeometrySubsets> Data;
        public static BitmapFont m_font;
        public static List<int> normalface = new List<int>();//渲染面修正
        public static List<int> normalpos = new List<int>();//位置面修正

        public static void DrawCubeBlock(PrimitivesRenderer3D primitivesRenderer, int value, Vector3 size, ref Matrix matrix, Color color, Color topColor, DrawBlockEnvironmentData environmentData)
        {

            TexturedBatch3D texturedBatch3D = primitivesRenderer.TexturedBatch(texture, useAlphaTest: true, 0, null, RasterizerState.CullCounterClockwiseScissor, null, SamplerState.PointClamp);
            float s = LightingManager.LightIntensityByLightValue[environmentData.Light];//光强            
            color = Color.MultiplyColorOnly(color, s);
            topColor = Color.MultiplyColorOnly(topColor, s);
            Vector3 translation = matrix.Translation;
            Vector3 vector = matrix.Right * size.X;
            Vector3 v = matrix.Up * size.Y;
            Vector3 v2 = matrix.Forward * size.Z;
            Vector3 v3 = translation + 0.5f * (-vector - v - v2);
            Vector3 v4 = translation + 0.5f * (vector - v - v2);
            Vector3 v5 = translation + 0.5f * (-vector + v - v2);
            Vector3 v6 = translation + 0.5f * (vector + v - v2);
            Vector3 v7 = translation + 0.5f * (-vector - v + v2);
            Vector3 v8 = translation + 0.5f * (vector - v + v2);
            Vector3 v9 = translation + 0.5f * (-vector + v + v2);
            Vector3 v10 = translation + 0.5f * (vector + v + v2);
            if (environmentData.ViewProjectionMatrix.HasValue)
            {
                Matrix m = environmentData.ViewProjectionMatrix.Value;
                Vector3.Transform(ref v3, ref m, out v3);
                Vector3.Transform(ref v4, ref m, out v4);
                Vector3.Transform(ref v5, ref m, out v5);
                Vector3.Transform(ref v6, ref m, out v6);
                Vector3.Transform(ref v7, ref m, out v7);
                Vector3.Transform(ref v8, ref m, out v8);
                Vector3.Transform(ref v9, ref m, out v9);
                Vector3.Transform(ref v10, ref m, out v10);
            }
            Block block = BlocksManager.Blocks[Terrain.ExtractContents(value)];
            Vector4 vector2 = m_slotTexCoords[block.GetFaceTextureSlot(0, value)];
            texturedBatch3D.QueueQuad(color: Color.MultiplyColorOnly(color, LightingManager.CalculateLighting(-matrix.Forward)), p1: v3, p2: v5, p3: v6, p4: v4, texCoord1: new Vector2(vector2.X, vector2.W), texCoord2: new Vector2(vector2.X, vector2.Y), texCoord3: new Vector2(vector2.Z, vector2.Y), texCoord4: new Vector2(vector2.Z, vector2.W));
            Vector4 vector3 = m_slotTexCoords[block.GetFaceTextureSlot(2, value)];
            texturedBatch3D.QueueQuad(color: Color.MultiplyColorOnly(color, LightingManager.CalculateLighting(matrix.Forward)), p1: v7, p2: v8, p3: v10, p4: v9, texCoord1: new Vector2(vector3.Z, vector3.W), texCoord2: new Vector2(vector3.X, vector3.W), texCoord3: new Vector2(vector3.X, vector3.Y), texCoord4: new Vector2(vector3.Z, vector3.Y));
            Vector4 vector4 = m_slotTexCoords[block.GetFaceTextureSlot(5, value)];
            texturedBatch3D.QueueQuad(color: Color.MultiplyColorOnly(color, LightingManager.CalculateLighting(-matrix.Up)), p1: v3, p2: v4, p3: v8, p4: v7, texCoord1: new Vector2(vector4.X, vector4.Y), texCoord2: new Vector2(vector4.Z, vector4.Y), texCoord3: new Vector2(vector4.Z, vector4.W), texCoord4: new Vector2(vector4.X, vector4.W));
            Vector4 vector5 = m_slotTexCoords[block.GetFaceTextureSlot(4, value)];
            texturedBatch3D.QueueQuad(color: Color.MultiplyColorOnly(topColor, LightingManager.CalculateLighting(matrix.Up)), p1: v5, p2: v9, p3: v10, p4: v6, texCoord1: new Vector2(vector5.X, vector5.W), texCoord2: new Vector2(vector5.X, vector5.Y), texCoord3: new Vector2(vector5.Z, vector5.Y), texCoord4: new Vector2(vector5.Z, vector5.W));
            Vector4 vector6 = m_slotTexCoords[block.GetFaceTextureSlot(1, value)];
            texturedBatch3D.QueueQuad(color: Color.MultiplyColorOnly(color, LightingManager.CalculateLighting(-matrix.Right)), p1: v3, p2: v7, p3: v9, p4: v5, texCoord1: new Vector2(vector6.Z, vector6.W), texCoord2: new Vector2(vector6.X, vector6.W), texCoord3: new Vector2(vector6.X, vector6.Y), texCoord4: new Vector2(vector6.Z, vector6.Y));
            Vector4 vector7 = m_slotTexCoords[block.GetFaceTextureSlot(3, value)];
            texturedBatch3D.QueueQuad(color: Color.MultiplyColorOnly(color, LightingManager.CalculateLighting(matrix.Right)), p1: v4, p2: v6, p3: v10, p4: v8, texCoord1: new Vector2(vector7.X, vector7.W), texCoord2: new Vector2(vector7.X, vector7.Y), texCoord3: new Vector2(vector7.Z, vector7.Y), texCoord4: new Vector2(vector7.Z, vector7.W));
        }
        public static Vector4 TextureSlotToTextureCoords(int slot)
        {
            int num = slot % 16;
            int num2 = slot / 16;
            return new Vector4((float)(((double)num + 0.001) / 16.0), (float)(((double)num2 + 0.001) / 16.0), (float)(((double)(num + 1) - 0.001) / 16.0), (float)(((double)(num2 + 1) - 0.001) / 16.0));
        }
        public static Vector4 TextureSlotToTextureCoords(int slot, int per)
        {
            int num = slot % per;
            int num2 = slot / per;
            return new Vector4((float)(((double)num + 0.001) / (double)per), (float)(((double)num2 + 0.001) / (double)per), (float)(((double)(num + 1) - 0.001) / (double)per), (float)(((double)(num2 + 1) - 0.001) / (double)per));
        }
        public static Vector2 getTexcoord(int slot, int per, int type)
        {
            Vector4 vector4 = TextureSlotToTextureCoords(slot, per);
            switch (type)
            {
                case 1: return new Vector2(vector4.X, vector4.Y);
                case 2: return new Vector2(vector4.Z, vector4.Y);
                case 3: return new Vector2(vector4.Z, vector4.W);
                case 4: return new Vector2(vector4.X, vector4.W);
                default: return new Vector2(vector4.X, vector4.Y);
            }
        }
        public static Vector2 getTexcoord(int type)
        {
            switch (type)
            {
                case 1: return new Vector2(0, 0);
                case 2: return new Vector2(1, 0);
                case 3: return new Vector2(1, 1);
                case 4: return new Vector2(0, 1);
                default: return new Vector2(0, 0);
            }
        }
        public static void GenerateChunkVertices(TerrainUpdater updater, TerrainChunk chunk, int x1, int z1, int x2, int z2)
        {
            if (chunk.ThreadState == TerrainChunkState.InvalidVertices1)
            {
                Data.Remove(chunk.Coords);
            }
            Terrain terrain = updater.m_terrain;
            TerrainChunk chunkAtCoords = terrain.GetChunkAtCoords(chunk.Coords.X - 1, chunk.Coords.Y - 1);
            TerrainChunk chunkAtCoords2 = terrain.GetChunkAtCoords(chunk.Coords.X, chunk.Coords.Y - 1);
            TerrainChunk chunkAtCoords3 = terrain.GetChunkAtCoords(chunk.Coords.X + 1, chunk.Coords.Y - 1);
            TerrainChunk chunkAtCoords4 = terrain.GetChunkAtCoords(chunk.Coords.X - 1, chunk.Coords.Y);
            TerrainChunk chunkAtCoords5 = terrain.GetChunkAtCoords(chunk.Coords.X + 1, chunk.Coords.Y);
            TerrainChunk chunkAtCoords6 = terrain.GetChunkAtCoords(chunk.Coords.X - 1, chunk.Coords.Y + 1);
            TerrainChunk chunkAtCoords7 = terrain.GetChunkAtCoords(chunk.Coords.X, chunk.Coords.Y + 1);
            TerrainChunk chunkAtCoords8 = terrain.GetChunkAtCoords(chunk.Coords.X + 1, chunk.Coords.Y + 1);
            if (chunkAtCoords4 == null)
            {
                x1 = MathUtils.Max(x1, 1);
            }
            if (chunkAtCoords2 == null)
            {
                z1 = MathUtils.Max(z1, 1);
            }
            if (chunkAtCoords5 == null)
            {
                x2 = MathUtils.Min(x2, 15);
            }
            if (chunkAtCoords7 == null)
            {
                z2 = MathUtils.Min(z2, 15);
            }
            for (int i = x1; i < x2; i++)
            {
                for (int j = z1; j < z2; j++)
                {
                    switch (i)
                    {
                        case 0:
                            if ((j == 0 && chunkAtCoords == null) || (j == 15 && chunkAtCoords6 == null))
                            {
                                continue;
                            }
                            break;
                        case 15:
                            if ((j == 0 && chunkAtCoords3 == null) || (j == 15 && chunkAtCoords8 == null))
                            {
                                continue;
                            }
                            break;
                    }
                    int num = i + chunk.Origin.X;
                    int num2 = j + chunk.Origin.Y;
                    int bottomHeightFast = chunk.GetBottomHeightFast(i, j);
                    int bottomHeight = terrain.GetBottomHeight(num - 1, num2);
                    int bottomHeight2 = terrain.GetBottomHeight(num + 1, num2);
                    int bottomHeight3 = terrain.GetBottomHeight(num, num2 - 1);
                    int bottomHeight4 = terrain.GetBottomHeight(num, num2 + 1);
                    int x3 = MathUtils.Min(bottomHeightFast - 1, MathUtils.Min(bottomHeight, bottomHeight2, bottomHeight3, bottomHeight4));
                    int topHeightFast = chunk.GetTopHeightFast(i, j);
                    int num3 = MathUtils.Max(x3, 1);
                    topHeightFast = MathUtils.Min(topHeightFast, 126);
                    int num4 = TerrainChunk.CalculateCellIndex(i, 0, j);
                    for (int k = num3; k <= topHeightFast; k++)
                    {
                        int cellValueFast = chunk.GetCellValueFast(num4 + k);
                        int num5 = Terrain.ExtractContents(cellValueFast);
                        if (num5 != 0)
                        {
                            BlocksManager.Blocks[num5].GenerateTerrainVertices(updater.m_subsystemTerrain.BlockGeometryGenerator, chunk.Geometry, cellValueFast, num, k, num2);
                            chunk.GeometryMinY = MathUtils.Min(chunk.GeometryMinY, k);
                            chunk.GeometryMaxY = MathUtils.Max(chunk.GeometryMaxY, k);
                        }
                    }
                }
            }
        }
        public static void DrawFlatBlock(PrimitivesRenderer3D primitivesRenderer, int value, float size, ref Matrix matrix, Texture2D texture, Color color, bool isEmissive, DrawBlockEnvironmentData environmentData)
{
	environmentData = (environmentData ?? BlocksManager.m_defaultEnvironmentData);
	if (!isEmissive)
	{
		color = Color.MultiplyColorOnly(color, LightingManager.LightIntensityByLightValue[environmentData.Light]);
	}
	Vector3 translation = matrix.Translation;
	Vector3 vector;
	Vector3 v;
	if (environmentData.BillboardDirection.HasValue)
	{
		vector = Vector3.Normalize(Vector3.Cross(environmentData.BillboardDirection.Value, Vector3.UnitY));
		v = -Vector3.Normalize(Vector3.Cross(environmentData.BillboardDirection.Value, vector));
	}
	else
	{
		vector = matrix.Right;
		v = matrix.Up;
	}
	Vector3 v2 = translation + 0.85f * size * (-vector - v);
	Vector3 v3 = translation + 0.85f * size * (vector - v);
	Vector3 v4 = translation + 0.85f * size * (-vector + v);
	Vector3 v5 = translation + 0.85f * size * (vector + v);
	if (environmentData.ViewProjectionMatrix.HasValue)
	{
		Matrix m = environmentData.ViewProjectionMatrix.Value;
		Vector3.Transform(ref v2, ref m, out v2);
		Vector3.Transform(ref v3, ref m, out v3);
		Vector3.Transform(ref v4, ref m, out v4);
		Vector3.Transform(ref v5, ref m, out v5);
	}
	Vector4  vector2 = new Vector4(0f, 0f, 1f, 1f);
            if (texture == null)
	{
		texture = ((environmentData.SubsystemTerrain != null) ? environmentData.SubsystemTerrain.SubsystemAnimatedTextures.AnimatedBlocksTexture : BlocksTexturesManager.DefaultBlocksTexture);
	}
	TexturedBatch3D texturedBatch3D = primitivesRenderer.TexturedBatch(texture, useAlphaTest: true, 0, null, RasterizerState.CullCounterClockwiseScissor, null, SamplerState.PointClamp);
	texturedBatch3D.QueueQuad(v2, v4, v5, v3, new Vector2(vector2.X, vector2.W), new Vector2(vector2.X, vector2.Y), new Vector2(vector2.Z, vector2.Y), new Vector2(vector2.Z, vector2.W), color);
	if (!environmentData.BillboardDirection.HasValue)
	{
		texturedBatch3D.QueueQuad(v2, v3, v5, v4, new Vector2(vector2.X, vector2.W), new Vector2(vector2.Z, vector2.W), new Vector2(vector2.Z, vector2.Y), new Vector2(vector2.X, vector2.Y), color);
	}
}

        public static void Init() {
           texture = ContentManager.Get<Texture2D>("Mekiasm/Blocks_32");
            m_font = ContentManager.Get<BitmapFont>("Fonts/SignFont");

            for (int i = 0; i < 256; i++)
            {
                m_slotTexCoords[i] = TextureSlotToTextureCoords(i);
            }
            normalface.Add(1);
            normalface.Add(0);
            normalface.Add(5);
            normalface.Add(5);
            normalface.Add(11);
            normalface.Add(7);

            normalpos.Add(1);
            normalpos.Add(0);
            normalpos.Add(3);
            normalpos.Add(2);
            normalpos.Add(0);
            normalpos.Add(0);
        }
        public static int GetDirection(int value) {
            int dd =Terrain.ExtractData(value);
            dd=((dd >> 12) & 0xf);
            return dd;
        }
        public static int setDirection(int value,int direction) {//返回新的value
            int dd = (direction<<12)&0xf000;
            int data = getItemId(value);
            dd = Terrain.ReplaceData(value,(data|dd)&0xffff);
            return dd;
        }
        public static int getItemId(int value) {
            int dd = Terrain.ExtractData(value);
            return (dd&0xfff);
        }
        public static int getPrimaryValue(int value) {
            return Terrain.MakeBlockValue(Terrain.ExtractContents(value),0,Terrain.ExtractData(value));
        }
        public static BlockPlacementData GetPlacementValue(SubsystemTerrain subsystemTerrain, ComponentMiner componentMiner, int value, TerrainRaycastResult raycastResult) {
            BlockPlacementData result = default(BlockPlacementData);
            int realdata = 0;//位置修正
            Vector3 forward = Matrix.CreateFromQuaternion(componentMiner.ComponentCreature.ComponentCreatureModel.EyeRotation).Forward;
            float num = Vector3.Dot(forward, Vector3.UnitZ);
            float num2 = Vector3.Dot(forward, Vector3.UnitX);
            float num3 = Vector3.Dot(forward, -Vector3.UnitZ);
            float num4 = Vector3.Dot(forward, -Vector3.UnitX);
            if (num == MathUtils.Max(num, num2, num3, num4))
            {
                realdata = 2;
            }
            else if (num2 == MathUtils.Max(num, num2, num3, num4))
            {
                realdata = 3;
            }
            else if (num3 == MathUtils.Max(num, num2, num3, num4))
            {
                realdata = 0;
            }
            else if (num4 == MathUtils.Max(num, num2, num3, num4))
            {
                realdata = 1;
            }
            result.Value = ILibrary.setDirection(value,realdata);//4*4表示方块位置，朝向
            result.CellFace = raycastResult.CellFace;
            return result;
        }

        public static int GetFaceTextureSlot(int face, int value,int map,int othermap)
        {
            int pos = GetDirection(value);//朝向
            if (face == pos) return map; else return othermap;            
        }
        public static int GetFaceTextureSlot(int face, int value, int[] maps)
        {
            int pos = GetDirection(value);//朝向
            switch (face)
            {//返回基础的各个面
                case 4: return maps[4];//顶面固定
                case 5: return maps[5];//底面固定
                default:
                    if (face == pos) return maps[0]; else if (face == getOppsiteFace(pos)) return maps[2];
                    else if (face == getNearRightFace(pos)) return maps[1]; else return maps[3];
            }
        }
        public static int getOppsiteFace(int face) {
            switch (face) {
                case 0:return 2;
                case 1:return 3;
                case 2:return 0;
                case 3:return 1;
                case 4:return 5;
                case 5:return 4;            
            }
            return 0;
        }
        public static int getNearRightFace(int face) {
            switch (face)
            {
                case 0: return 1;
                case 1: return 2;
                case 2: return 3;
                case 3: return 0;
            }
            return 0;
        }

        public static bool addBaseEnergyEntity(Project project,string name,Point3 point, int max) {
            try
            {
                DatabaseObject databaseObject = project.GameDatabase.Database.FindDatabaseObject(name, project.GameDatabase.EntityTemplateType, throwIfNotFound: true);
                ValuesDictionary valuesDictionary = new ValuesDictionary();
                valuesDictionary.PopulateFromDatabaseObject(databaseObject);
                valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue("Coordinates", point);
                valuesDictionary.GetValue<ValuesDictionary>("EnergyBlock").SetValue<int>("MaxQuantity", max);
                valuesDictionary.GetValue<ValuesDictionary>("EnergyBlock").SetValue<int>("Quantity", 0);
                valuesDictionary.GetValue<ValuesDictionary>("EnergyBlock").SetValue<int>("Mode", 0);
                Entity entity = project.CreateEntity(valuesDictionary);
                project.AddEntity(entity);
                return true;
            }
            catch { return false; }
        }
        public static bool addCoalGeneratorEntity(Project project, Point3 point, int max)
        {
            try
            {
                DatabaseObject databaseObject = project.GameDatabase.Database.FindDatabaseObject("CoalGeneratorBlock", project.GameDatabase.EntityTemplateType, throwIfNotFound: true);
                ValuesDictionary valuesDictionary = new ValuesDictionary();
                valuesDictionary.PopulateFromDatabaseObject(databaseObject);
                valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue("Coordinates", point);
                valuesDictionary.GetValue<ValuesDictionary>("EnergyBlock").SetValue<int>("MaxQuantity", max);
                valuesDictionary.GetValue<ValuesDictionary>("EnergyBlock").SetValue<int>("Quantity", 0);
                valuesDictionary.GetValue<ValuesDictionary>("EnergyBlock").SetValue<int>("Mode", 0);
                Entity entity = project.CreateEntity(valuesDictionary);
                project.AddEntity(entity);
                return true;
            }
            catch { return false; }
        }

        public static bool addCrusherEntity(Project project, Point3 point, int max)
        {
            try
            {
                DatabaseObject databaseObject = project.GameDatabase.Database.FindDatabaseObject("CrusherBlock", project.GameDatabase.EntityTemplateType, throwIfNotFound: true);
                ValuesDictionary valuesDictionary = new ValuesDictionary();
                valuesDictionary.PopulateFromDatabaseObject(databaseObject);
                valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue("Coordinates", point);
                valuesDictionary.GetValue<ValuesDictionary>("EnergyBlock").SetValue<int>("MaxQuantity", max);
                valuesDictionary.GetValue<ValuesDictionary>("EnergyBlock").SetValue<int>("Quantity", 0);
                valuesDictionary.GetValue<ValuesDictionary>("EnergyBlock").SetValue<int>("Mode", 0);
                Entity entity = project.CreateEntity(valuesDictionary);
                project.AddEntity(entity);
                return true;
            }
            catch { return false; }
        }
        public static bool addEnrichEntity(Project project, Point3 point, int max)
        {
            try
            {
                DatabaseObject databaseObject = project.GameDatabase.Database.FindDatabaseObject("EnrichBlock", project.GameDatabase.EntityTemplateType, throwIfNotFound: true);
                ValuesDictionary valuesDictionary = new ValuesDictionary();
                valuesDictionary.PopulateFromDatabaseObject(databaseObject);
                valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue("Coordinates", point);
                valuesDictionary.GetValue<ValuesDictionary>("EnergyBlock").SetValue<int>("MaxQuantity", max);
                valuesDictionary.GetValue<ValuesDictionary>("EnergyBlock").SetValue<int>("Quantity", 0);
                valuesDictionary.GetValue<ValuesDictionary>("EnergyBlock").SetValue<int>("Mode", 0);
                Entity entity = project.CreateEntity(valuesDictionary);
                project.AddEntity(entity);
                return true;
            }
            catch { return false; }
        }
        public static bool addMekSmeltEntity(Project project, Point3 point, int max)
        {
            try
            {
                DatabaseObject databaseObject = project.GameDatabase.Database.FindDatabaseObject("MekSmeltBlock", project.GameDatabase.EntityTemplateType, throwIfNotFound: true);
                ValuesDictionary valuesDictionary = new ValuesDictionary();
                valuesDictionary.PopulateFromDatabaseObject(databaseObject);
                valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue("Coordinates", point);
                valuesDictionary.GetValue<ValuesDictionary>("EnergyBlock").SetValue<int>("MaxQuantity", max);
                valuesDictionary.GetValue<ValuesDictionary>("EnergyBlock").SetValue<int>("Quantity", 0);
                valuesDictionary.GetValue<ValuesDictionary>("EnergyBlock").SetValue<int>("Mode", 0);
                Entity entity = project.CreateEntity(valuesDictionary);
                project.AddEntity(entity);
                return true;
            }
            catch { return false; }
        }

        public static bool addManufactorymeltEntity(Project project, Point3 point, int max)
        {
            try
            {
                DatabaseObject databaseObject = project.GameDatabase.Database.FindDatabaseObject("MamufactoryBlock", project.GameDatabase.EntityTemplateType, throwIfNotFound: true);
                ValuesDictionary valuesDictionary = new ValuesDictionary();
                valuesDictionary.PopulateFromDatabaseObject(databaseObject);
                valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue("Coordinates", point);
                valuesDictionary.GetValue<ValuesDictionary>("EnergyBlock").SetValue<int>("MaxQuantity", max);
                valuesDictionary.GetValue<ValuesDictionary>("EnergyBlock").SetValue<int>("Quantity", 0);
                valuesDictionary.GetValue<ValuesDictionary>("EnergyBlock").SetValue<int>("Mode", 0);
                Entity entity = project.CreateEntity(valuesDictionary);
                project.AddEntity(entity);
                return true;
            }
            catch { return false; }
        }
        public static bool addAlloyEntity(Project project, Point3 point, int max)
        {
            try
            {
                DatabaseObject databaseObject = project.GameDatabase.Database.FindDatabaseObject("AlloyBlock", project.GameDatabase.EntityTemplateType, throwIfNotFound: true);
                ValuesDictionary valuesDictionary = new ValuesDictionary();
                valuesDictionary.PopulateFromDatabaseObject(databaseObject);
                valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue("Coordinates", point);
                valuesDictionary.GetValue<ValuesDictionary>("EnergyBlock").SetValue<int>("MaxQuantity", max);
                valuesDictionary.GetValue<ValuesDictionary>("EnergyBlock").SetValue<int>("Quantity", 0);
                valuesDictionary.GetValue<ValuesDictionary>("EnergyBlock").SetValue<int>("Mode", 0);
                Entity entity = project.CreateEntity(valuesDictionary);
                project.AddEntity(entity);
                return true;
            }
            catch { return false; }
        }
        public static bool addMekDynamicEntity(Project project, Point3 point, int max)
        {
            try
            {
                DatabaseObject databaseObject = project.GameDatabase.Database.FindDatabaseObject("MekDynamicBlock", project.GameDatabase.EntityTemplateType, throwIfNotFound: true);
                ValuesDictionary valuesDictionary = new ValuesDictionary();
                valuesDictionary.PopulateFromDatabaseObject(databaseObject);
                valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue("Coordinates", point);
                valuesDictionary.GetValue<ValuesDictionary>("FluidBlock").SetValue<int>("MaxQuantity", max);
                valuesDictionary.GetValue<ValuesDictionary>("FluidBlock").SetValue<int>("Quantity", 0);
                valuesDictionary.GetValue<ValuesDictionary>("FluidBlock").SetValue<int>("Mode", 0);
                Entity entity = project.CreateEntity(valuesDictionary);
                project.AddEntity(entity);
                return true;
            }
            catch { return false; }
        }      
        public static bool removeBaseEnergyEntity(Project project,SubsystemBlockEntities subsystemBlockEntities,int x,int y,int z) {
            ComponentBlockEntity blockEntity = subsystemBlockEntities.GetBlockEntity(x, y, z);
            Vector3 position = new Vector3(x, y, z) + new Vector3(0.5f);
            if (blockEntity != null)
                {
                IInventory inventory= blockEntity.Entity.FindComponent<IInventory>();
                if (inventory != null) inventory.DropAllItems(position);
                       project.RemoveEntity(blockEntity.Entity, disposeEntity: true);
                }
                return true;
        }
        public static bool removeMekFluidEntity(Project project, SubsystemBlockEntities subsystemBlockEntities, int x, int y, int z)
        {
            ComponentBlockEntity blockEntity = subsystemBlockEntities.GetBlockEntity(x, y, z);
            Vector3 position = new Vector3(x, y, z) + new Vector3(0.5f);
            if (blockEntity != null)
            {
                IInventory inventory = blockEntity.Entity.FindComponent<IInventory>();
                if (inventory != null) inventory.DropAllItems(position);
                project.RemoveEntity(blockEntity.Entity, disposeEntity: true);
            }
            return true;
        }
    }
}
