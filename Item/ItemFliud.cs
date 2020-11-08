using Engine;
using Engine.Graphics;
using Game;
using System.Collections.Generic;
namespace Mekiasm
{
    class ItemFliud:Item
    {
        public Color global = Color.White;
        public List<BoundingBox> boundingBoxes = new List<BoundingBox>();
        public ItemFliud(int id,Color color,string n) {
            global = color;
            this.itemid = id;
            this.DisplayName = n;
            BoundingBox bounding = new BoundingBox();
            bounding.Min = Vector3.Zero;
            bounding.Max = Vector3.Zero;
            boundingBoxes.Add(bounding);
        }
        public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)
        {
            return DisplayName;
        }
        public override int GetFaceTextureSlot(int face, int value)
        {
            return 255;
        }
        public override BoundingBox[] GetCustomCollisionBoxes(SubsystemTerrain terrain, int value)
        {
            return boundingBoxes.ToArray();
        }
        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            ILibrary.DrawCubeBlock(primitivesRenderer, value,new Vector3(size), ref matrix, global,global, environmentData);
        }
        public override void GenerateTerrainVertices(Block block, BlockGeometryGenerator generator, TerrainGeometrySubsets geometry, int value, int x, int y, int z)
        {
            generator.GenerateCubeVertices(block,value,x,y,z,global,geometry.TransparentSubsetsByFace);
        }
    }
}
