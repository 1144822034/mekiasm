using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Game;
namespace Mekiasm
{
	public class MyElectricElement :ElectricElement
	{
		public CellFace cellFace;
		public ComponentEnergyMachine cptBase;//本身的Component
		public SubsystemBlockEntities subsystemBlockEntities;
		public List<Point3> conlist = new List<Point3>();//连接的机器列表
		public SubsystemItemElectric subsystemItemElectric=new SubsystemItemElectric();
		//假如以输入设备为中心
		public MyElectricElement(SubsystemElectricity subsystemElectricity, CellFace cellFace_)
			: base(subsystemElectricity, cellFace_)
		{
			cellFace = cellFace_;//保存输入设备的坐标
			subsystemBlockEntities = subsystemElectricity.Project.FindSubsystem<SubsystemBlockEntities>();
			//获取该设备的实体
			cptBase = subsystemBlockEntities.GetBlockEntity(cellFace.X, cellFace.Y, cellFace.Z).Entity.FindComponent<ComponentEnergyMachine>(true);
			cptBase.connections = subsystemItemElectric.getConlist(SubsystemElectricity.SubsystemTerrain.Terrain,cellFace.Point);
		}
		public override void OnNeighborBlockChanged(CellFace cellFace, int neighborX, int neighborY, int neighborZ)
		{
			cptBase.connections = subsystemItemElectric.getConlist(SubsystemElectricity.SubsystemTerrain.Terrain, cellFace.Point);
		}
		public override void OnRemoved()
		{

		}

	}
}
