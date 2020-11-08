using Engine;
using Engine.Graphics;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mekiasm
{
    public class ItemPlant:Item
    {
        public ItemPlant(int dd,string nn) : base(dd,nn) { }
        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            ILibrary.DrawCubeBlock(primitivesRenderer, value, new Vector3(size), ref matrix, Color.White, Color.White, environmentData);
        }
        public override void GenerateTerrainVertices(Block block, BlockGeometryGenerator generator, TerrainGeometrySubsets geometry, int value, int x, int y, int z)
        {
            generator.GenerateCubeVertices(block, value, x, y, z, Color.White, SubsystemItemBlockBase.GTV(x, z, geometry).OpaqueSubsetsByFace);
        }
        public override bool IsFaceTransparent(SubsystemTerrain subsystemTerrain, int face, int value)
        {
            return false;
        }
        public override bool IsInteractive(SubsystemTerrain subsystemTerrain, int value)
        {
            return true;
        }

        public override int GetFaceTextureSlot(int face, int value)
        {
            int plantid = SubsystemMekPlantsBehavior.getPlantID(value);
            int level = SubsystemMekPlantsBehavior.getLevel(value);
            int data = Terrain.ExtractData(value);
            switch (face) {
                case 4:
                    return 223;//木头顶部和底部
                case 5:
                    return 223;
                case 0:
                    return 224;
                case 1:
                    return 244;
                case 2:
                    return 244 + data;
                case 3:
                    return 244;
            }
            return Terrain.ExtractData(value);
        }
        public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)
        {
            return DisplayName;
        }

    }
}
