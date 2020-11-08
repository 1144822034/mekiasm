using Engine;
using Engine.Graphics;
using Engine.Media;
using Game;
using System.Collections.Generic;
using TemplatesDatabase;

namespace Mekiasm
{
    public class SubsystemItemAlphaBlockBehavior : SubsystemBlockBehavior,IDrawable
    {
        public SubsystemItemElectricBehavior subsystemItemElectricBehavior;
        public bool TransmitSetflag = false;
        public Point3 TransmitStartPoint;
        public FontBatch2D fontBatch2D=null;
        public PrimitivesRenderer2D primitivesRenderer=new PrimitivesRenderer2D();
        public Dictionary<Vector3, int> Animate_Points = new Dictionary<Vector3, int>();
        public override int[] HandledBlocks => new int[] { 1007,1006};
        public FengliAnimate fengliAnimate = new FengliAnimate();
        public int[] DrawOrders => new int[] {99 };

        public override void Load(ValuesDictionary valuesDictionary)
        {
            fontBatch2D = primitivesRenderer.FontBatch(ContentManager.Get<BitmapFont>("Fonts/SignFont"));
            subsystemItemElectricBehavior = Project.FindSubsystem<SubsystemItemElectricBehavior>();
            base.Load(valuesDictionary);
        }
        public override bool OnUse(Vector3 start, Vector3 direction, ComponentMiner componentMiner)
        {
            int blockid = Terrain.ExtractContents(componentMiner.ActiveBlockValue);
            int id = ILibrary.getItemId(componentMiner.ActiveBlockValue);
            switch (blockid)
            {
                case 1006:
                    switch (id)
                    {
                        case 500:
                            TerrainRaycastResult? raycastResult = componentMiner.PickTerrainForDigging(start, direction);
                            if (TransmitSetflag && !raycastResult.HasValue) {//设定了传送地点且不对准传送机
                                subsystemItemElectricBehavior.tranFLag = true;
                                subsystemItemElectricBehavior.leftFLag = false;
                                componentMiner.ComponentPlayer.ComponentBody.Position = new Vector3(TransmitStartPoint.X, TransmitStartPoint.Y+1, TransmitStartPoint.Z+1);
                                SubsystemTerrain.TerrainUpdater.UpdateChunkSingleStep(SubsystemTerrain.Terrain.GetChunkAtCell(TransmitStartPoint.X, TransmitStartPoint.Z), 15);


                            }
                            if (Terrain.ExtractContents(raycastResult.Value.Value) == 1003 && Terrain.ExtractData(raycastResult.Value.Value) == 64)
                            {
                                if (!TransmitSetflag) { //没有设定地点
                                    TransmitStartPoint = raycastResult.Value.CellFace.Point;
                                    TransmitSetflag = true;
                                    componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage("传送门1设定成功", false, false);
                                    return false;
                                }
                                if (raycastResult.Value.CellFace.Point != TransmitStartPoint && TransmitSetflag)
                                {
                                    if (subsystemItemElectricBehavior.tranmitPoints.TryGetValue(TransmitStartPoint, out NewMutiBlockCheck.TranmitResult tranmitResult))
                                    {
                                        NewMutiBlockCheck.TranmitResult tranmitResult1 = subsystemItemElectricBehavior.tranmitPoints[TransmitStartPoint];
                                        if (subsystemItemElectricBehavior.tranmitPoints.TryGetValue(raycastResult.Value.CellFace.Point, out NewMutiBlockCheck.TranmitResult tranmitResultaa))
                                        {
                                            NewMutiBlockCheck.TranmitResult tranmitResult2 = subsystemItemElectricBehavior.tranmitPoints[raycastResult.Value.CellFace.Point];
                                            TransmitSetflag = false;
                                            tranmitResult2.connectPoint = TransmitStartPoint;
                                            tranmitResult1.connectPoint = raycastResult.Value.CellFace.Point;
                                            subsystemItemElectricBehavior.tranmitPoints[TransmitStartPoint] = tranmitResult1;
                                            subsystemItemElectricBehavior.tranmitPoints[raycastResult.Value.CellFace.Point] = tranmitResult2;
                                            componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage("传送门连接成功", false, false);
                                            return false;
                                        }
                                        else {
                                            componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage("传送门结构被破坏，无法连接", false, false);
                                            return false;
                                        }
                                    }
                                }
                            }
                            break;
                    }

                    break;

                case 1007:

                    switch (id)
                    {
                        case 39:
                            break;
                    }
                    break;
            }
            return false;
        }
        public override bool OnInteract(TerrainRaycastResult raycastResult, ComponentMiner componentMiner)
        {
            int blockid = Terrain.ExtractContents(raycastResult.Value);
            int id = ILibrary.getItemId(raycastResult.Value);
            switch (blockid) {
                case 1006:
                    switch (id) {
                        case 500:
                            componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage("",false,false);
                            break;                    
                    }
                    
                    break;

                case 1007:
            switch (id) {
                case 39:
                    subsystemItemElectricBehavior.OnInteract(raycastResult, componentMiner);
                    break;
                case 102:break;

            }
                    break;
            }
            return false;
        }
        public override void OnBlockRemoved(int value, int newValue, int x, int y, int z)
        {
            Point3 point = new Point3(x, y, z);
            Vector3 p = new Vector3(point);
            if (Animate_Points.ContainsKey(p)) Animate_Points.Remove(p);
            if (ComponentPlayUpdate.Data.ContainsKey(point))
            {//独立模型方块地形贴图删除
                ComponentPlayUpdate.Data.Remove(point);
            }
            subsystemItemElectricBehavior.OnBlockRemoved(value, newValue, x, y, z);
        }
        public override void OnBlockAdded(int value, int oldValue, int x, int y, int z)
        {
            int id = Terrain.ExtractContents(value);
            int data = Terrain.ExtractData(value);
            if (data == 102 && id == 1007) {
                Vector3 p = new Vector3(x, y, z);
               if (!Animate_Points.ContainsKey(p))Animate_Points.Add(p,0);
            }
            subsystemItemElectricBehavior.OnBlockAdded(value, oldValue, x, y, z);
        }
        public override void OnNeighborBlockChanged(int x, int y, int z, int neighborX, int neighborY, int neighborZ)
        {
            Point3 point3 = new Point3(x,y,z);
            if (ComponentPlayUpdate.Data.ContainsKey(point3)) ComponentPlayUpdate.Data.Remove(point3);
            int value = SubsystemTerrain.Terrain.GetCellValueFast(x,y,z);
            int id = Terrain.ExtractContents(value);
            int data = Terrain.ExtractData(value);
            if (id == 1007 && data == 26) {
                BlocksManager.Blocks[id].GenerateTerrainVertices(SubsystemTerrain.BlockGeometryGenerator,new TerrainGeometrySubsets(),value,x,y,z);
            }
        }

        public void Draw(Camera camera, int drawOrder)
        {
            /*
            foreach (Vector3 v in Animate_Points.Keys) {
                fengliAnimate.Draw(camera,v);
                fontBatch2D.QueueText("drawing",Vector2.Zero,0f,Color.White);
                primitivesRenderer.Flush();
            }            
            */
        }
    }
}
