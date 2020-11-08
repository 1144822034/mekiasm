using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.Graphics;
using Game;
using GameEntitySystem;
using TemplatesDatabase;

namespace Mekiasm
{
    public class SubsystemItemElectricBehavior:SubsystemBlockBehavior,IUpdateable
    {
        public override int[] HandledBlocks => new int[] {1003};
        public int UpdateOrder => 100;
        public ComponentBody mcomponentBody;
        public bool tranFLag = false;
        public bool leftFLag = true;
        public DynamicArray<Vector3> tranca = new DynamicArray<Vector3>();
        public Dictionary<Point3,List<Point3>> MutiDevices = new Dictionary<Point3, List<Point3>>();//储存节点及组成部分
        public Dictionary<Point3, NewMutiBlockCheck.TranmitResult> tranmitPoints = new Dictionary<Point3, NewMutiBlockCheck.TranmitResult>();
        public NewMutiBlockCheck mutiBlockCheck = new NewMutiBlockCheck();
        public List<Point3> devices = new List<Point3>();
        public SubsystemItemElectric subsystemItemElectric=new SubsystemItemElectric();
        public List<int> dynamic_insideaccept = new List<int> {0};
        public List<int> dynamic_outsideaccept = new List<int> {Terrain.MakeBlockValue(1003, 0, 40), Terrain.MakeBlockValue(1003, 0, 41), Terrain.MakeBlockValue(1007, 0, 39)};
        public List<int> dynamic_faceaccept = new List<int> {Terrain.MakeBlockValue(1003, 0, 40), Terrain.MakeBlockValue(1003, 0, 41), Terrain.MakeBlockValue(1007, 0, 39) };
        public List<int> dynamic_sideaccept = new List<int> {Terrain.MakeBlockValue(1003, 0, 41)};
        public SubsystemBlockEntities subsystemBlockEntities;
        public ComponentPlayer component;
        public override bool OnUse(Vector3 start, Vector3 direction, ComponentMiner componentMiner)
        {
            component = componentMiner.ComponentPlayer;
            int id = Terrain.ExtractContents(componentMiner.ActiveBlockValue);
            TerrainRaycastResult? raycastResult = componentMiner.PickTerrainForDigging(start,direction);
            int data = Terrain.ExtractData(componentMiner.ActiveBlockValue);
            switch (data) {
                case 0:
                    componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage("a",false,false);
                    if (raycastResult.HasValue) { //桶的使用
                        Point3 point = raycastResult.Value.CellFace.Point;
                        SubsystemTerrain.Terrain.SetCellValueFast(point.X,point.Y+2,point.Z,Terrain.MakeBlockValue(1009,0,0));
                    }
                    break;
                case 1:
                    if (raycastResult.HasValue)
                    {
                        Point3 point = raycastResult.Value.CellFace.Point;
                        SubsystemTerrain.Terrain.SetCellValueFast(point.X, point.Y+2, point.Z, Terrain.MakeBlockValue(1009, 0, 1));
                    }
                    break;
                default:
                    
                    break;
            }
            return false;
        }
        public override void OnBlockAdded(int value, int oldValue, int x, int y, int z)
        {
            try
            {
                Point3 point = new Point3(x, y, z);
                
                int id = ILibrary.getItemId(value);
                int bid = Terrain.ExtractContents(value);
                if (bid==1003) switch (id)
                    {//39,40,41为动态储罐
                        case 27://燃煤发电机
                            ILibrary.addCoalGeneratorEntity(Project, point, 1024);
                            devices.Add(point);
                            break;
                        case 30://太阳能发电
                            ILibrary.addBaseEnergyEntity(Project, "SolarEnergyBlock", point, 1024);//1k eu电量
                            devices.Add(point);
                            break;
                        case 40:
                           
                            NewMutiBlockCheck.Result result = mutiBlockCheck.checkMutiBlocks(SubsystemTerrain.Terrain, point, dynamic_outsideaccept, dynamic_insideaccept, dynamic_sideaccept, dynamic_faceaccept);
                            if (result.finish)
                            {
                                int v = result.Size.X * result.Size.Y * result.Size.Z;
                                if (!MutiDevices.ContainsKey(result.savaPoint)) { MutiDevices.Add(result.savaPoint, result.blocks); ILibrary.addMekDynamicEntity(Project, result.savaPoint, 1000 * v); }
                                if (component != null) component.ComponentGui.DisplaySmallMessage($"动态储罐成型，储量{1000 * result.Size.X * result.Size.Y * result.Size.Z}MB t:{result.Size}", false, false);
                            }

                            break;
                        case 41:
                           
                            result= mutiBlockCheck.checkMutiBlocks(SubsystemTerrain.Terrain, point, dynamic_outsideaccept,dynamic_insideaccept,dynamic_sideaccept,dynamic_faceaccept);
                            if (result.finish)
                            {
                                int v = result.Size.X * result.Size.Y * result.Size.Z;
                                if (!MutiDevices.ContainsKey(result.savaPoint))
                                {
                                    MutiDevices.Add(result.savaPoint, result.blocks);
                                    ILibrary.addMekDynamicEntity(Project, result.savaPoint, 1000 * v);
                                }
                                if (component != null) component.ComponentGui.DisplaySmallMessage($"动态储罐成型，储量{1000 * result.Size.X * result.Size.Y * result.Size.Z}MB t:{result.Size}", false, false);
                            }
                            break;
                        case 63://基础能量立方
                            ILibrary.addBaseEnergyEntity(Project, "BaseEnergyBlock", point, 1024 * 1024);//1k eu电量
                            devices.Add(point);
                            break;
                        case 64://传送机
                            NewMutiBlockCheck.TranmitResult tranmitResult= mutiBlockCheck.checkTransmit(SubsystemTerrain, point, new List<int>() { Terrain.MakeBlockValue(1003, 0, 64), Terrain.MakeBlockValue(1003, 0, 65) }, Terrain.MakeBlockValue(1007));
                            if (tranmitResult.finished) {
                                component.ComponentGui.DisplaySmallMessage("传送门成型",false,false);
                                if(!tranmitPoints.ContainsKey(tranmitResult.savePoint))tranmitPoints.Add(tranmitResult.savePoint,tranmitResult);
                            }
                            break;
                        case 65://传送框架
                            tranmitResult = mutiBlockCheck.checkTransmit(SubsystemTerrain, point, new List<int>() { Terrain.MakeBlockValue(1003, 0, 64), Terrain.MakeBlockValue(1003, 0, 65) }, Terrain.MakeBlockValue(1007));
                            if (tranmitResult.finished)
                            {
                                component.ComponentGui.DisplaySmallMessage("传送门成型", false, false);
                                if (!tranmitPoints.ContainsKey(tranmitResult.savePoint)) tranmitPoints.Add(tranmitResult.savePoint,tranmitResult);
                            }
                            break;


                        case 47://粉碎机
                            ILibrary.addCrusherEntity(Project, point, 1024);//1k eu电量
                            devices.Add(point);
                            break;
                        case 72://充能冶炼炉
                            ILibrary.addMekSmeltEntity(Project, point, 1024);//1k eu电量
                            devices.Add(point);
                            break;
                        case 78:
                            ILibrary.addEnrichEntity(Project, point, 1024);//1k eu电量
                            devices.Add(point);
                            break;
                        case 80://合金炉
                            ILibrary.addAlloyEntity(Project, point, 1024);//1k eu电量
                            devices.Add(point);
                            break;
                        case 82://制造厂
                            ILibrary.addManufactorymeltEntity(Project, point, 1024);//1k eu电量
                            devices.Add(point);
                            break;
                    }
                else if (bid==1007) {
                    switch (id) {
                        case 39:
                            NewMutiBlockCheck.Result result = mutiBlockCheck.checkMutiBlocks(SubsystemTerrain.Terrain, point, dynamic_outsideaccept, dynamic_insideaccept, dynamic_sideaccept, dynamic_faceaccept);
                            if (result.finish)
                            {
                                int v = result.Size.X * result.Size.Y * result.Size.Z;
                                if (!MutiDevices.ContainsKey(result.savaPoint))
                                {
                                    MutiDevices.Add(result.savaPoint, result.blocks);
                                    ILibrary.addMekDynamicEntity(Project, result.savaPoint, 1000 * v);
                                }
                                if (component != null) component.ComponentGui.DisplaySmallMessage($"动态储罐成型，储量{1000 * result.Size.X * result.Size.Y * result.Size.Z}MB t:{result.Size}", false, false);
                            }
                            break;
                    
                    }
                }
                updateList();
            }
            catch (Exception e)
            {
                if (component != null) component.ComponentGui.DisplaySmallMessage(e.ToString(), false, false);
            }

        }
        public override void OnChunkInitialized(TerrainChunk chunk)
        {
            updateList();
            /*
            foreach (KeyValuePair<Point3,NewMutiBlockCheck.TranmitResult> tranmitResult in tranmitPoints) {
                mutiBlockCheck.checkTransmit(SubsystemTerrain,tranmitResult.Key, new List<int>() { Terrain.MakeBlockValue(1003, 0, 64), Terrain.MakeBlockValue(1003, 0, 65) }, Terrain.MakeBlockValue(1007));
            }
            */
        }
        public void removeMutiBlocks(int x,int y,int z) {
            Point3 p=new Point3(x, y, z);
            foreach (KeyValuePair<Point3, List<Point3>> po in MutiDevices) {
                if (po.Value.Contains(p)) {
                    ILibrary.removeBaseEnergyEntity(base.Project, subsystemBlockEntities, po.Key.X, po.Key.Y, po.Key.Z);
                    MutiDevices.Remove(po.Key);
                    return;
                }
            }
        
        }
        public override void OnBlockRemoved(int value, int newValue, int x, int y, int z)
        {
            try
            {
                Point3 point = new Point3(x, y, z);
                //checkTransmitHasBlock(point);
                int id = ILibrary.getItemId(value);
                switch (id) {
                    case 40:removeMutiBlocks(x,y,z); break;
                    case 41:removeMutiBlocks(x,y,z); break;
                    case 64: removeTransmitPoints(point);break;
                    case 65: removeTransmitPoints(point);break;
                    default:break;
                }
                if (devices.Contains(point))devices.Remove(point);
                ILibrary.removeBaseEnergyEntity(base.Project, subsystemBlockEntities, x, y, z);
                updateList();
            }
            catch (Exception e)
            {
                if (component != null) component.ComponentGui.DisplaySmallMessage(e.ToString(), false, false);
            }
        }
        public bool checkTransmitPoints(Point3 point)
        {
            foreach (NewMutiBlockCheck.TranmitResult tranmitResult in tranmitPoints.Values)
            {
                if (tranmitResult.connectPoint==point)
                {
                    return true;
                }
            }
            return false;
        }
        public bool checkTransmitHasBlock(Point3 point)
        {    foreach (NewMutiBlockCheck.TranmitResult tranmitResult in tranmitPoints.Values)
                {
                    int va = SubsystemTerrain.Terrain.GetCellValueFast(tranmitResult.pa.X, tranmitResult.pa.Y, tranmitResult.pa.Z);
                    va = ILibrary.getPrimaryValue(va);
                    if (va != Terrain.MakeBlockValue(1007)) removeTransmitPoints(tranmitResult.savePoint);
                    int vb = SubsystemTerrain.Terrain.GetCellValueFast(tranmitResult.pb.X, tranmitResult.pb.Y, tranmitResult.pb.Z);
                    vb = ILibrary.getPrimaryValue(vb);
                    if (vb != Terrain.MakeBlockValue(1007)) removeTransmitPoints(tranmitResult.savePoint);

                }
            
            return false;
        }
        public void removeTransmitPoints(Point3 point) {

                foreach (NewMutiBlockCheck.TranmitResult tranmitResult in tranmitPoints.Values)
                {
                    if (tranmitResult.blocks.Contains(point))
                    {
                        SubsystemTerrain.Terrain.SetCellValueFast(tranmitResult.pa.X, tranmitResult.pa.Y, tranmitResult.pa.Z, 0);
                        SubsystemTerrain.Terrain.SetCellValueFast(tranmitResult.pb.X, tranmitResult.pb.Y, tranmitResult.pb.Z, 0);
                        tranmitPoints.Remove(tranmitResult.savePoint);
                        return;
                    }
                }

        }
        public override bool OnInteract(TerrainRaycastResult raycastResult, ComponentMiner componentMiner)
        {
            ComponentEnergyMachine mcomponent;
            ComponentFluidStore componentFluidStore;
            PanelWidget panelWidget = new PanelWidget();
            int value = raycastResult.Value;
            Point3 point = raycastResult.CellFace.Point;
            int ID = ILibrary.getItemId(value);
            if (Terrain.ExtractContents(value) == 1003) switch (ID) {
                    case 27:
                        mcomponent = subsystemBlockEntities.GetBlockEntity(point.X, point.Y, point.Z).Entity.FindComponent<ComponentCoalGenerator>(true);
                        componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = new CoalGeneratorWidget((ComponentCoalGenerator)mcomponent);
                        break;
                    case 30:
                        mcomponent = subsystemBlockEntities.GetBlockEntity(point.X, point.Y, point.Z).Entity.FindComponent<CptEgySolarGtor>(true);
                        componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = new AspSolarWidget((CptEgySolarGtor)mcomponent);
                        break;
                    case 40:
                        NewMutiBlockCheck.Result result = mutiBlockCheck.checkMutiBlocks(SubsystemTerrain.Terrain, point, dynamic_outsideaccept, dynamic_insideaccept, dynamic_sideaccept, dynamic_faceaccept);
                        if (result.finish)
                        {
                            componentFluidStore = subsystemBlockEntities.GetBlockEntity(result.savaPoint.X, result.savaPoint.Y, result.savaPoint.Z).Entity.FindComponent<ComponentMekDynamic>(true);
                            componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = new MekDynamicWidget((ComponentMekDynamic)componentFluidStore);
                        }
                        break;
                    case 41:
                        result = mutiBlockCheck.checkMutiBlocks(SubsystemTerrain.Terrain, point, dynamic_outsideaccept, dynamic_insideaccept, dynamic_sideaccept, dynamic_faceaccept);
                        if (result.finish)
                        {
                            componentFluidStore = subsystemBlockEntities.GetBlockEntity(result.savaPoint.X, result.savaPoint.Y, result.savaPoint.Z).Entity.FindComponent<ComponentMekDynamic>(true);
                            componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = new MekDynamicWidget((ComponentMekDynamic)componentFluidStore);
                        }
                        break;
                    case 47:
                        mcomponent = subsystemBlockEntities.GetBlockEntity(point.X, point.Y, point.Z).Entity.FindComponent<ComponentCrusher>();
                        componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = new CrusherWidget((ComponentCrusher)mcomponent);
                        break;
                    case 48: break;
                    case 49: break;
                    case 50: break;
                    case 51: break;
                    case 52: break;
                    case 63://基础能量立方
                        mcomponent = subsystemBlockEntities.GetBlockEntity(point.X, point.Y, point.Z).Entity.FindComponent<ComponentEnergyMachine>(true);
                        componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = new EnergyCubeWidget(mcomponent);
                        break;
                    case 64:
                        if (Terrain.ExtractContents(componentMiner.ActiveBlockValue) == 1006 && Terrain.ExtractData(componentMiner.ActiveBlockValue) == 500) {
                            return true;
                        }
                        if (tranmitPoints.TryGetValue(point, out NewMutiBlockCheck.TranmitResult tranmitResult))
                        {
                            componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage("conn:"+tranmitResult.connectPoint.ToString(),false,false);
                        }
                        break;
                    case 65:

                        break;
                         case 72://充能冶炼炉
                        mcomponent = subsystemBlockEntities.GetBlockEntity(point.X, point.Y, point.Z).Entity.FindComponent<ComponentSmelt>(true);
                        componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = new MekSmeltWidget((ComponentSmelt)mcomponent);
                        break;
                    case 78://富集仓
                        mcomponent = subsystemBlockEntities.GetBlockEntity(point.X, point.Y, point.Z).Entity.FindComponent<ComponentEnrich>(true);
                        componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = new EnrichWidget((ComponentEnrich)mcomponent);
                        break;
                    case 80://合金炉
                        mcomponent = subsystemBlockEntities.GetBlockEntity(point.X, point.Y, point.Z).Entity.FindComponent<ComponentAlloy>(true);
                        componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = new AlloyWidget((ComponentAlloy)mcomponent);
                        break;
                    case 82://制造厂
                        mcomponent = subsystemBlockEntities.GetBlockEntity(point.X, point.Y, point.Z).Entity.FindComponent<ComponentManufactory>(true);
                        componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = new ManufactoryWidget((ComponentManufactory)mcomponent);
                        break;
                    default:
                        panelWidget.setText(subsystemItemElectric.scanSlot(SubsystemTerrain.Terrain, point));
                        component.ComponentGui.ModalPanelWidget = panelWidget;
                        break;
                }
            else if (Terrain.ExtractContents(value)==1007) {
                switch (ID) {
                    case 39:
                        NewMutiBlockCheck.Result result = mutiBlockCheck.checkMutiBlocks(SubsystemTerrain.Terrain, point, dynamic_outsideaccept, dynamic_insideaccept, dynamic_sideaccept, dynamic_faceaccept);
                        if (result.finish)
                        {
                            componentFluidStore = subsystemBlockEntities.GetBlockEntity(result.savaPoint.X, result.savaPoint.Y, result.savaPoint.Z).Entity.FindComponent<ComponentMekDynamic>(true);
                            componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = new MekDynamicWidget((ComponentMekDynamic)componentFluidStore);
                        }
                        break;
                }
            }
            return false;
        }
        public override void OnNeighborBlockChanged(int x, int y, int z, int neighborX, int neighborY, int neighborZ)
        {//附近方块改变

            updateList();

        }
        public override void OnCollide(CellFace cellFace, float velocity, ComponentBody componentBody)
        {
            int id = ILibrary.getItemId(SubsystemTerrain.Terrain.GetCellValueFast(cellFace.Point.X, cellFace.Point.Y, cellFace.Point.Z));
            switch (id)
            {                
                case 64:
                    if (cellFace.Face == 4)
                    {
                        if (tranmitPoints.TryGetValue(cellFace.Point, out NewMutiBlockCheck.TranmitResult result))
                        {

                            if (component == null) component = componentBody.Entity.FindComponent<ComponentPlayer>();
                            if (component != null && !tranFLag && leftFLag)
                            {
                                tranFLag = true;
                                leftFLag = false;
                                    mcomponentBody = componentBody;
                                   if(result.directionIsX) tranca.Add(new Vector3(result.connectPoint.X , result.connectPoint.Y + 1, result.connectPoint.Z+1));
                                   else tranca.Add(new Vector3(result.connectPoint.X +1, result.connectPoint.Y + 1, result.connectPoint.Z));

                            }
                        }
                    }
                    break;
            }
        }
        public override void Load(ValuesDictionary valuesDictionary)
        {
            SubsystemTerrain = base.Project.FindSubsystem<SubsystemTerrain>(true);
            subsystemBlockEntities = base.Project.FindSubsystem<SubsystemBlockEntities>(true);
            base.Load(valuesDictionary);
            foreach (Point3 item in valuesDictionary.GetValue<ValuesDictionary>("Devices").Values) {
                devices.Add(item);
            }
            foreach (ValuesDictionary pairs in valuesDictionary.GetValue<ValuesDictionary>("MutiDevices").Values)
            {
                List<Point3> points = new List<Point3>();
                foreach (Point3 po in pairs.GetValue<ValuesDictionary>("Data").Values) {
                        points.Add(po);
                 }
                MutiDevices.Add(pairs.GetValue<Point3>("Point"),points);
            }
            foreach (ValuesDictionary keyValuePairs in valuesDictionary.GetValue<ValuesDictionary>("tranmitPoints").Values) {
                NewMutiBlockCheck.TranmitResult tranmitResult = new NewMutiBlockCheck.TranmitResult();
                List<Point3> blocks = new List<Point3>();
                tranmitResult.savePoint = keyValuePairs.GetValue<Point3>("savePoint");
                tranmitResult.connectPoint = keyValuePairs.GetValue<Point3>("connPoint");
                tranmitResult.pa = keyValuePairs.GetValue<Point3>("pa");
                tranmitResult.directionIsX = keyValuePairs.GetValue<bool>("directionIsX");
                tranmitResult.pb = keyValuePairs.GetValue<Point3>("pb");
                foreach (Point3 point in keyValuePairs.GetValue<ValuesDictionary>("blocks").Values) {
                    blocks.Add(point);                
                }
                tranmitResult.blocks = blocks;
                tranmitResult.finished = true;
                tranmitPoints.Add(tranmitResult.savePoint,tranmitResult);
            }

        }
        public void updateList() {
            foreach (Point3 point in devices) {
                ComponentEnergyMachine cptBaseEgy = subsystemBlockEntities.GetBlockEntity(point.X, point.Y, point.Z).Entity.FindComponent<ComponentEnergyMachine>(true);
                Dictionary<Point3,int>  temp = subsystemItemElectric.getConlist(SubsystemTerrain.Terrain,point);
                cptBaseEgy.connections = temp;
                cptBaseEgy.thisPosition = new Vector3(point);
            }
        }
        public override void Save(ValuesDictionary valuesDictionary)
        {
            int num = 0;
            ValuesDictionary keyValues = new ValuesDictionary();
            foreach (Point3 point in devices) {
                keyValues.SetValue<Point3>(num.ToString(),point);
                ++num;
            }
            valuesDictionary.SetValue<ValuesDictionary>("Devices",keyValues);
            num = 0;
            ValuesDictionary valuePairs = new ValuesDictionary();
            foreach (KeyValuePair<Point3,List<Point3>> point3s in MutiDevices) {
                ValuesDictionary datas = new ValuesDictionary();
                ValuesDictionary itemv = new ValuesDictionary();
                int nnum = 0;
                foreach (Point3 point in point3s.Value) {
                    itemv.SetValue(nnum.ToString(),point);
                    ++nnum;
                }
                datas.SetValue("Point",point3s.Key);
                datas.SetValue("Data", itemv);
                valuePairs.SetValue(num.ToString(),datas);
                ++num;
            }
            valuesDictionary.SetValue("MutiDevices", valuePairs);
            ValuesDictionary valuePairs1 = new ValuesDictionary();
            num = 0;
            foreach (KeyValuePair<Point3,NewMutiBlockCheck.TranmitResult> keyValuePair in tranmitPoints) {
                ValuesDictionary valuePairs2 = new ValuesDictionary();
                int y = 0;
                ValuesDictionary poins = new ValuesDictionary();
                foreach (Point3 point in keyValuePair.Value.blocks) {
                    poins.Add(y.ToString(),point);
                    ++y;
                }
                valuePairs2.SetValue("savePoint",keyValuePair.Value.savePoint);
                valuePairs2.SetValue("connPoint", keyValuePair.Value.connectPoint);
                valuePairs2.SetValue("pa", keyValuePair.Value.pa);
                valuePairs2.SetValue("pb", keyValuePair.Value.pb);
                valuePairs2.SetValue("directionIsX",keyValuePair.Value.directionIsX);
                valuePairs2.SetValue("blocks", poins);
                valuePairs1.SetValue(num.ToString(),valuePairs2);
                ++num;
            }
            valuesDictionary.SetValue("tranmitPoints",valuePairs1);

        }

        public void Update(float dt)
        {
            if (tranca.Array.Length > 0 ) {
                if (tranFLag)
                {
                    try
                    {
                        mcomponentBody.m_position = tranca.Array[0];
                        SubsystemTerrain.TerrainUpdater.UpdateChunkSingleStep(SubsystemTerrain.Terrain.GetChunkAtCell(Terrain.ToCell(tranca.Array[0].X), Terrain.ToCell(tranca.Array[0].Z)), 15);
                        mcomponentBody.Entity.FindComponent<ComponentPlayer>().ComponentGui.DisplaySmallMessage($"传送开始 {tranca.Array[0].ToString()}", false, false);
                        tranFLag = false;
                    }
                    catch (Exception e)
                    {
                        Log.Information(e.ToString());
                    }
                }
                else {
                    if (Vector3.Distance(mcomponentBody.Position, tranca.Array[0]) > 2f)
                    {
                        leftFLag = true;
                        tranca.Clear();
                    }
                }
            }
        }
    }
}
