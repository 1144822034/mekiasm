using Engine;
using Game;
using System.Collections.Generic;

namespace Mekiasm
{
    public class NewMutiBlockCheck
    {
        public List<Point3> cache = new List<Point3>();
        public struct TranmitResult {
            public Point3 pa;
            public Point3 pb;
            public Point3 savePoint;
            public bool finished;
            public Point3 connectPoint;
            public List<Point3> blocks;
            public bool directionIsX;
        
        }
        public struct Result{
            public List<Point3> blocks;
            public Point3 Size;
            public bool finish;
            public string err;
            public Point3 savaPoint;
        }
        public void findNeibors(Terrain terrain, Point3 start,List<int> accepts) {
            for (int i=0;i<6;i++) {
                Point3 nowp = start + SubsystemItemElectric.direction[i];
                int vv = terrain.GetCellValue(nowp.X, nowp.Y, nowp.Z);
                vv = ILibrary.getPrimaryValue(vv);
                if (accepts.Contains(vv)&&!cache.Contains(nowp))
                { //包含允许的方块且不重复
                    cache.Add(nowp);
                    findNeibors(terrain,nowp,accepts);
                }
                else {
                    continue;
                }
            }
        }
        public Result createResult() {
            Result result = new Result();
            result.finish = false;
            return result;
        }
        public static Point3 caculateV(List<Point3> point3s)
        {//计算长宽高
            if (point3s.Count < 25) return Point3.Zero;
            return new Point3(point3s[0].X - point3s[point3s.Count - 1].X +1, point3s[0].Y - point3s[point3s.Count - 1].Y + 1, point3s[0].Z - point3s[point3s.Count - 1].Z +1);
        }
        public static bool checkExists(List<Point3> point3s,List<int> outsideBlocks, List<int> insideBlocks, List<int> sideBlocks, List<int> faceBlocks, Terrain terrain)
        {
            Point3 point;int vv;
            if (point3s.Count < 25) return false;
            int j = point3s.Count - 1;
            for (int y = 0; y <= point3s.Count; y++)
            {
                for (int i = point3s[j].X; i <= point3s[0].X; i++)
                {
                    for (int m = point3s[j].Y; m <= point3s[0].Y; m++)
                    {
                        for (int n = point3s[j].Z; n <= point3s[0].Z; n++)
                        {
                            point = new Point3(i, m, n);
                            if (!point3s.Contains(point)&&!insideBlocks.Contains(0)) return false;
                            vv = terrain.GetCellValue(i, m, n);
                            vv = Terrain.MakeBlockValue(Terrain.ExtractContents(vv), 0, Terrain.ExtractData(vv));
                            if (i == point3s[j].X || i == point3s[0].X || m == point3s[j].Y || m == point3s[0].Y || n == point3s[j].Z || n == point3s[0].Z)
                            {//外部点

                                if ((i == point3s[j].X || i == point3s[0].X) && (m > point3s[j].Y && m < point3s[0].Y) && (n > point3s[j].Z && n < point3s[0].Z))
                                {//面心点
                                    if (faceBlocks != null) if (!faceBlocks.Contains(vv)) { Log.Information($"{Terrain.ExtractData(vv)}不是面心所需:{point3s.Count}:{i}:{m}:{n}"); return false; }
                                }
                                else if ((i > point3s[j].X && i < point3s[0].X) && (m == point3s[j].Y || m == point3s[0].Y) && (n > point3s[j].Z && n < point3s[0].Z))
                                {//面心点
                                    if (faceBlocks != null) if (!faceBlocks.Contains(vv)) { Log.Information($"{Terrain.ExtractData(vv)}不是面心所需:{point3s.Count}:{i}:{m}:{n}"); return false; }
                                }
                                else if ((i > point3s[j].X && i < point3s[0].X) && (m > point3s[j].Y && m < point3s[0].Y) && (n == point3s[j].Z || n == point3s[0].Z))
                                {//面心点
                                    if (faceBlocks != null) if (!faceBlocks.Contains(vv)) { Log.Information($"{Terrain.ExtractData(vv)}不是面心所需:{point3s.Count}:{i}:{m}:{n}"); return false; }
                                }
                                else
                                {
                                    //框架上的点
                                    if (!sideBlocks.Contains(vv))
                                        {
                                            Log.Information($"{Terrain.ExtractData(vv)}不是框架所需"); return false;
                                        }
                                }
                                if (sideBlocks == null && faceBlocks == null) if (!outsideBlocks.Contains(vv)){
                                        Log.Information($"{Terrain.ExtractData(vv)}不是外部所需"); return false;
                            }

                            }
                            else { //内部点
                                if (!insideBlocks.Contains(vv)) { Log.Information($"{Terrain.ExtractData(vv)}不是内部所需"); return false; }
                            }
                        }
                    }

                }
            }
            return true;
        }
        public void scanBlocks(Terrain terrain,Point3 point,List<int> accepts){
            cache.Clear();
            findNeibors(terrain,point,accepts);
        }
        public Result checkMutiBlocks(Terrain terrain, Point3 point, List<int> outsideBlocks, List<int> insideBlocks, List<int> sideBlocks, List<int> faceBlocks) {
            Result result = createResult();int j = 0;
            scanBlocks(terrain,point,outsideBlocks);
            sortPoint3(cache);
            j = cache.Count-1;
            result.finish=checkExists(cache,outsideBlocks,insideBlocks,sideBlocks,faceBlocks,terrain);
            result.Size = caculateV(cache);
            result.blocks = cache;
            if(j>25) result.savaPoint =cache[j];
            return result;
        }
        public static void sortPoint3(List<Point3> point3s)
        {//由大到小
            for (int i = 0; i < point3s.Count; i++)
            {
                for (int j = 1; j < point3s.Count - i; j++)
                {
                    if (point3s[j - 1].X < point3s[j].X)
                    {
                        Point3 point = point3s[j - 1];
                        point3s[j - 1] = point3s[j];
                        point3s[j] = point;
                    }
                    else if (point3s[j - 1].X == point3s[j].X)
                    {
                        if (point3s[j - 1].Y < point3s[j].Y)
                        {
                            Point3 point = point3s[j - 1];
                            point3s[j - 1] = point3s[j];
                            point3s[j] = point;
                        }
                        else if (point3s[j - 1].Y == point3s[j].Y)
                        {
                            if (point3s[j - 1].Z < point3s[j].Z)
                            {
                                Point3 point = point3s[j - 1];
                                point3s[j - 1] = point3s[j];
                                point3s[j] = point;
                            }
                        }
                    }
                }
            }
        }
        public TranmitResult checkTransmit(SubsystemTerrain subsystemTerrain, Point3 point, List<int> acceptBlocks,int centerBlock) {
            cache.Clear();
            TranmitResult tranmitResult = new TranmitResult();
            tranmitResult.blocks = new List<Point3>();
            Terrain terrain = subsystemTerrain.Terrain;
            findNeibors(terrain,point,acceptBlocks);
            sortPoint3(cache);
            if (cache.Count != 10) { tranmitResult.finished= false; return tranmitResult; }
            Point3 pa = cache[0],pb=cache[cache.Count-1];
            int h = pa.Y - pb.Y;
            if (h != 3) { tranmitResult.finished = false; }
            int va = terrain.GetCellValueFast(pb.X + 1, pb.Y, pb.Z);
            va = ILibrary.getPrimaryValue(va);
            int vb= terrain.GetCellValueFast(pb.X, pb.Y, pb.Z + 1);
            vb = ILibrary.getPrimaryValue(vb);
            for (int i = 0; i < 4; i++)
            {
                int vc = terrain.GetCellValueFast(pb.X, pb.Y + i, pb.Z);
                vc = ILibrary.getPrimaryValue(vc);
                if (Terrain.ExtractContents(vc) != 1003 || Terrain.ExtractData(vc) != 65) { tranmitResult.finished = false; return tranmitResult; }
            }
            if (Terrain.ExtractContents(va) == 1003 && Terrain.ExtractData(va) == 64)
            {//传送机64 //x轴方向
                terrain.SetCellValueFast(pb.X + 1, pb.Y + 1, pb.Z, Terrain.MakeBlockValue(1007, 0, 0));
                tranmitResult.savePoint = new Point3(pb.X + 1, pb.Y, pb.Z);
                tranmitResult.pa = new Point3(pb.X + 1, pb.Y + 1, pb.Z);
                tranmitResult.directionIsX = true;
                terrain.SetCellValueFast(pb.X + 1, pb.Y + 2, pb.Z, Terrain.MakeBlockValue(1007, 0, 0));
                tranmitResult.pb = new Point3(pb.X + 1, pb.Y + 2, pb.Z);
                tranmitResult.finished = true;
                tranmitResult.blocks = cache;
                return tranmitResult;
            }
            else if (Terrain.ExtractContents(vb) == 1003 && Terrain.ExtractData(vb) == 64)
            { //z轴方向
                tranmitResult.savePoint = new Point3(pb.X, pb.Y, pb.Z + 1);
                terrain.SetCellValueFast(pb.X, pb.Y + 1, pb.Z + 1, Terrain.MakeBlockValue(1007, 0, 0));
                tranmitResult.pa = new Point3(pb.X, pb.Y + 1, pb.Z + 1);
                tranmitResult.pb = new Point3(pb.X, pb.Y + 2, pb.Z + 1);
                tranmitResult.directionIsX = false;
                terrain.SetCellValueFast(pb.X, pb.Y + 2, pb.Z + 1, Terrain.MakeBlockValue(1007, 0, 0));
                tranmitResult.blocks = cache;
                tranmitResult.finished = true;
                return tranmitResult;
            }
            else { tranmitResult.finished = false; return tranmitResult; }
        }        
    }
}
