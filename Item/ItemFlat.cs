using Engine;
using Engine.Graphics;
using Game;
using System.Collections.Generic;

namespace Mekiasm
{
    public class ItemFlat:Item
    {
        public Texture2D texture2D;
        public ItemFlat(int id,string name,Texture2D texture):base(id,name) {                        
            texture2D = texture;   
        }
        public override void GetDropValues(SubsystemTerrain subsystemTerrain, int oldValue, int newValue, int toolLevel, List<BlockDropValue> dropValues, out bool showDebris)
        {
            showDebris = true;
            dropValues.Add(new BlockDropValue() { Value = oldValue, Count = 1 });
        }
        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            ILibrary.DrawFlatBlock(primitivesRenderer,value,size,ref matrix,texture2D,color,true,environmentData);
        }
        public override void GenerateTerrainVertices(Block block, BlockGeometryGenerator generator, TerrainGeometrySubsets geometry, int value, int x, int y, int z)
        {
        }
    }
}
