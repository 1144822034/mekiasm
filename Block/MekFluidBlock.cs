using Engine;
using Engine.Graphics;
using Game;
using System.Collections.Generic;
using System.Linq;

namespace Mekiasm
{
   public class MekFluidBlock:WaterBlock
    { 
        public new const int Index = 1009;
        public new static int MaxLevel = 7;
        public MekFluidBlock(){        
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public virtual Item GetItem(ref int value)
        {
            int id = Terrain.ExtractData(value);
            if (Terrain.ExtractContents(value) != 1009) return null;
            return MekiasmInit.items_fluid.Where(p => p.itemid == id).FirstOrDefault();
        }
        public override int GetFaceTextureSlot(int face, int value)
        {
            return GetItem(ref value).GetFaceTextureSlot(face, value);
        }
        public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)
        {
          return GetItem(ref value).GetDisplayName(subsystemTerrain,value);
        }
        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            GetItem(ref value).DrawBlock(primitivesRenderer,value,color,size,ref matrix,environmentData);
        }
        public override bool IsFaceTransparent(SubsystemTerrain subsystemTerrain, int face, int value)
        {
            return false;
        }
        public override IEnumerable<int> GetCreativeValues()
        {
            int[] arr = new int[MekiasmInit.items_fluid.Count];
            for (int i = 0; i < MekiasmInit.items_fluid.Count; i++)
            {
                arr[i] = Terrain.MakeBlockValue(1009, 0, MekiasmInit.items_fluid.Array[i].itemid);
            }
            return arr;
        }
        public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometrySubsets geometry, int value, int x, int y, int z)
        {
            GetItem(ref value).GenerateTerrainVertices(this,generator,geometry,value,x,y,z);
        }               
    }
}
