using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Game;
namespace Mekiasm
{    
    public class SubsystemItemElectric
    {
        public List<Point3> readlist = new List<Point3>();//读取列表
        public Dictionary<Point3,int> conlist = new Dictionary<Point3, int>();
        public static Point3[] direction = new Point3[6] {
        new Point3(1,0,0),new Point3(-1,0,0),new Point3(0,1,0),new Point3(0,-1,0),new Point3(0,0,1),new Point3(0,0,-1),
        };
        public SubsystemItemElectric() { }        
        public void findConlist(Terrain terrain, Point3 point3) {
            for (int i=0;i<6;i++) {
                Point3 pp = direction[i] + point3;
                if (readlist.Contains(pp)) continue;
                readlist.Add(pp);
                int value = terrain.GetCellValueFast(pp.X,pp.Y,pp.Z);
                if (Terrain.ExtractContents(value) == 1003)
                {
                    int data = ILibrary.getItemId(value);
                    if (!conlist.Keys.Contains(pp)) {
                        int type = getEletype(data);
                        if (type != 0) conlist.Add(pp, type);
                        findConlist(terrain, pp);
                    }
                }
            }
        }
        public int getEletype(int data) {
            int type = 0;//0导线1输出2输入3输入输出
            switch (data) {
                case 30:type = 1;break;
                case 27:type = 1;break;
                case 47:type = 2;break;
                case 64:type = 3;break;
                case 78:type = 2;break;
                case 80: type = 2; break;
                case 82: type = 2; break;
            }
            return type;
        }
        public Dictionary<Point3, int> getConlist(Terrain terrain,Point3 point) {
            conlist.Clear(); readlist.Clear();
            findConlist(terrain,point);
            conlist.Remove(point);
            return conlist;
        }
        public string scanSlot(Terrain terrain,Point3 position) {
            conlist.Clear();readlist.Clear();
            findConlist(terrain, position);
            Dictionary<Point3,int> conlistd = conlist;
            if(conlistd.Keys.Contains(position))conlistd.Remove(position);
            string str = "";
            foreach (KeyValuePair<Point3,int> item in conlistd) {
                int value= terrain.GetCellValueFast(item.Key.X, item.Key.Y, item.Key.Z);
                    str += BlocksManager.Blocks[Terrain.ExtractContents(value)].GetDisplayName(null,value)+"\n";
                }
            return str;
        }
    }
}
