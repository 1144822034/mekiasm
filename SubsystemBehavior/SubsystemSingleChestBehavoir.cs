using Engine;
using Engine.Graphics;
using Game;
using GameEntitySystem;
using System;
using System.Collections.Generic;
using TemplatesDatabase;

namespace Mekiasm
{
	public class SubsystemSingleChestBehavoir: SubsystemBlockBehavior, IDrawable
	{		
		public RenderTarget2D m_renderTarget = new RenderTarget2D(512, 512, ColorFormat.Rgba8888, DepthFormat.None);
		public static int tpint = 0;
		public SubsystemTerrain subsystemTerrain;
		public SubsystemAudio subsystemAudio;
		public ComponentPlayer componentPlayer;
		public PrimitivesRenderer3D primitivesRenderer3D = new PrimitivesRenderer3D();
		public PrimitivesRenderer2D primitivesRenderer2D = new PrimitivesRenderer2D();
		public Dictionary<Point3, int> pointlist = new Dictionary<Point3, int>();//方块id
		public SubsystemBlockEntities subsystemBlockEntities;
		public Dictionary<Point3, int> drawList = new Dictionary<Point3, int>();//绘制队列
		public Point3 renderPoint;//正在渲染的位置
		public bool renderflag=false;
		public int renderPos = 0;
		public int ll = 0;
		public List<Point3> renderList=new List<Point3>();		
		public int[] DrawOrders => new int[] { 4};
		public override int[] HandledBlocks => new int[] { 1002 };
		public void Draw(Camera camera, int drawOrder)
		{
			drawSign(camera);
		}
		public override bool OnUse(Vector3 start, Vector3 direction, ComponentMiner componentMiner)
		{
			++ll; if (ll >= 2) { ll = 0; return false;}
			componentPlayer = componentMiner.ComponentPlayer;
			return false;
		}
		public override bool OnInteract(TerrainRaycastResult raycastResult, ComponentMiner componentMiner)
		{//记录point
			++ll; if (ll >= 2) { ll = 0; return true; }
			componentPlayer = componentMiner.ComponentPlayer;
			ComponentBlockEntity componentBlockEntity = subsystemBlockEntities.GetBlockEntity(raycastResult.CellFace.X,raycastResult.CellFace.Y,raycastResult.CellFace.Z);
			if (componentBlockEntity == null) { toast("没有找到Blockentity"); return false; }
			ComponentSingleChest singleChestData = componentBlockEntity.Entity.FindComponent<ComponentSingleChest>(true);
			singleChestData.Droptems(new Vector3(raycastResult.CellFace.X,raycastResult.CellFace.Y,raycastResult.CellFace.Z)+new Vector3(1f,1f,1f),1);
			return true;
		}
		public int saveInventory(IInventory inventory,int value,int count,int max) {//返回剩余的数量
			if (inventory.GetSlotCount(0) == 0||inventory.GetSlotValue(0)==value) {
				//计算拿取物品数量
				int nownum = inventory.GetSlotCount(0);
				int neednum;//拿取数量
				int cannum = max - nownum;//可以容纳数量
				if (count <= cannum) {
					neednum = count;
				} else {
					neednum = cannum;
				}
				inventory.AddSlotItems(0,value,neednum);
				return count-neednum;
			}
			return count;
		}
		public override void OnHitByProjectile(CellFace cellFace, WorldItem worldItem)
		{//收集掉落物
			++ll; if (ll >= 2) { ll = 0; return; }
			int value = subsystemTerrain.Terrain.GetCellValueFast(cellFace.Point.X,cellFace.Point.Y,cellFace.Point.Z);
			int level = ILibrary.getItemId(value);
			int maxcnt =(int)Math.Pow((double)2,(double)level)*128;//计算最大容量
			if (Terrain.ExtractContents(value) == HandledBlocks[0]) {
				try {
					ComponentBlockEntity componentBlockEntity = subsystemBlockEntities.GetBlockEntity(cellFace.X, cellFace.Y, cellFace.Z);
					if (componentBlockEntity == null) { toast("没有找到Blockentity"); return; }
					ComponentSingleChest singleChestData = componentBlockEntity.Entity.FindComponent<ComponentSingleChest>(true);
					if (singleChestData == null) { toast("没有找到componentchest"); return; }
					if (worldItem.ToRemove) return;
					Pickable pickable = worldItem as Pickable;
					int num = pickable?.Count ?? 1;
					int num2 = saveInventory(singleChestData, worldItem.Value, num,maxcnt);
					if (num2 < num)
					{
						subsystemAudio.PlaySound("Audio/PickableCollected", 1f, 0f, worldItem.Position, 3f, autoDelay: true);
					}
					if (num2 <= 0)
					{
						worldItem.ToRemove = true;
					}
					else if (pickable != null)
					{
						pickable.Count = num2;
					}
				}
				catch (Exception e) {
					toast(e.ToString());
						}
				
				}
		}		
		public void toast(string str) {
			if (componentPlayer != null) componentPlayer.ComponentGui.DisplaySmallMessage(str, false, false);
		}
		public override void Dispose()
		{
			base.Dispose();
			Utilities.Dispose(ref m_renderTarget);
			renderList.Clear();
		}
		public override void OnBlockRemoved(int value, int newValue, int x, int y, int z)
		{
			++ll; if (ll >= 2) { ll = 0; return; }
			Point3 point = new Point3(x, y, z);
				if (pointlist.ContainsKey(point)){
					pointlist.Remove(point);
					renderList.Remove(point);
				ComponentBlockEntity blockEntity = subsystemBlockEntities.GetBlockEntity(x, y, z);
				if (blockEntity != null)
				{//移除实体
					Vector3 position = new Vector3(x, y, z) + new Vector3(0.5f);
					foreach (IInventory item in blockEntity.Entity.FindComponents<IInventory>())
					{
						item.DropAllItems(position);
					}
					subsystemTerrain.Project.RemoveEntity(blockEntity.Entity, disposeEntity: true);
				}
			}
		}
		public override void OnBlockAdded(int value, int oldValue, int x, int y, int z)
		{
			++ll; if (ll >= 2) { ll = 0; return; }

			//为箱子添加实体
			try
			{
					Point3 point = new Point3(x, y, z);
					if (!pointlist.TryGetValue(point, out int vv))
					{
						pointlist.Add(point,0);
					}
					DatabaseObject databaseObject = base.Project.GameDatabase.Database.FindDatabaseObject("SingleChest", base.Project.GameDatabase.EntityTemplateType, throwIfNotFound: true);
					ValuesDictionary valuesDictionary = new ValuesDictionary();
					valuesDictionary.PopulateFromDatabaseObject(databaseObject);
					valuesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue("Coordinates", new Point3(x, y, z));
					Entity entity = base.Project.CreateEntity(valuesDictionary);
					base.Project.AddEntity(entity);
				}
				catch{
					//if (componentPlayer != null) componentPlayer.ComponentGui.DisplaySmallMessage($"",false,false);
				}			
		}

		public override void Load(ValuesDictionary valuesDictionary)
		{
			//mesh-face
			//0-1//人正对黎明的太阳，方块正对人
			//1-0
			//2-5
			//3-5
			//4-11//上顶面
			//5-7//下顶面			
			subsystemTerrain = Project.FindSubsystem<SubsystemTerrain>(true);
			subsystemBlockEntities= Project.FindSubsystem<SubsystemBlockEntities>(true);
			subsystemAudio = Project.FindSubsystem<SubsystemAudio>(true);
			getData(valuesDictionary.GetValue<ValuesDictionary>("PointInfo"));
			foreach (KeyValuePair<Point3,int> item in pointlist) {
				Point3 point = item.Key;
				if(!renderList.Contains(item.Key))renderList.Add(point);
			}
		}
		public ValuesDictionary makeData(Dictionary<Point3, int> data) {
			int num = 0;
			ValuesDictionary keyValuePairs = new ValuesDictionary();
			foreach (KeyValuePair<Point3, int> item in data) {
				num++;
				ValuesDictionary valuePairs = new ValuesDictionary();
				valuePairs.SetValue<Point3>("point", item.Key);//储存的位置
				valuePairs.SetValue<int>("value", item.Value);//储存的物品ID
				keyValuePairs.SetValue<ValuesDictionary>(num.ToString(), valuePairs);
			}
			return keyValuePairs;
		}

		public void getData(ValuesDictionary valuePairs) {
			foreach (ValuesDictionary valuePairs1 in valuePairs.Values) {
				pointlist.Add(valuePairs1.GetValue<Point3>("point"), valuePairs1.GetValue<int>("value"));
			}
		}
		public override void Save(ValuesDictionary valuesDictionary)
		{
			valuesDictionary.SetValue<ValuesDictionary>("PointInfo", makeData(pointlist));			
		}
		public void drawSign(Camera camera)
		{	

			TexturedBatch3D texturedBatch3D = primitivesRenderer3D.TexturedBatch(m_renderTarget, false,0, DepthStencilState.None, RasterizerState.CullCounterClockwiseScissor,BlendState.AlphaBlend, SamplerState.PointWrap);	
			foreach (KeyValuePair<Point3, int> item in pointlist)
			{
				renderPoint = item.Key;
				if (Vector3.DistanceSquared(new Vector3(renderPoint), camera.ViewPosition) > 400f) continue;//超出20f视野
				ComponentSingleChest singleChest = subsystemBlockEntities.GetBlockEntity(renderPoint.X, renderPoint.Y, renderPoint.Z).Entity.FindComponent<ComponentSingleChest>(true);
				if (singleChest == null || singleChest.GetSlotCount(0) == 0)
				{
					continue;
				}
				int mvalue = subsystemTerrain.Terrain.GetCellValue(renderPoint.X, renderPoint.Y, renderPoint.Z);
				int mpos = ILibrary.GetDirection(mvalue);
				BlockMesh signSurfaceBlockMesh = MekiasmInit.faceMeshes.Array[ILibrary.normalpos[mpos]];
				float s = LightingManager.LightIntensityByLightValue[Terrain.ExtractLight(mvalue)];
				Color color = new Color(s,s,s);
				Vector3 signSurfaceNormal = MekiasmInit.faceNormals.Array[ILibrary.normalface[ILibrary.normalpos[mpos]]];
				Vector3 vector = new Vector3(renderPoint);
				float num3 = Vector3.Dot(camera.ViewPosition - (vector + new Vector3(0.5f)), signSurfaceNormal);
				Vector3 v = MathUtils.Max(0.01f * num3, 0.005f) * signSurfaceNormal;
				int indecies = signSurfaceBlockMesh.Indices.Count / 3;
				for (int i = 0; i < indecies; i += 2)
				{
					if (i >= indecies) { break; }
					BlockMeshVertex blockMeshVertex = signSurfaceBlockMesh.Vertices.Array[signSurfaceBlockMesh.Indices.Array[i * 3]];
					BlockMeshVertex blockMeshVertex2 = signSurfaceBlockMesh.Vertices.Array[signSurfaceBlockMesh.Indices.Array[i * 3 + 1]];
					BlockMeshVertex blockMeshVertex3 = signSurfaceBlockMesh.Vertices.Array[signSurfaceBlockMesh.Indices.Array[i * 3 + 2]];
					Vector3 p = blockMeshVertex.Position + vector + v;
					Vector3 p2 = blockMeshVertex2.Position + vector + v;
					Vector3 p3 = blockMeshVertex3.Position + vector + v;
					Vector2 textureCoordinates = ILibrary.getTexcoord(2);
					Vector2 textureCoordinates2 = ILibrary.getTexcoord(3);
					Vector2 textureCoordinates3 = ILibrary.getTexcoord(4);
					texturedBatch3D.QueueTriangle(p, p2, p3, textureCoordinates, textureCoordinates2, textureCoordinates3, color);
					BlockMeshVertex blockMeshVertex11 = signSurfaceBlockMesh.Vertices.Array[signSurfaceBlockMesh.Indices.Array[++i * 3]];
					BlockMeshVertex blockMeshVertex22 = signSurfaceBlockMesh.Vertices.Array[signSurfaceBlockMesh.Indices.Array[i * 3 + 1]];
					BlockMeshVertex blockMeshVertex33 = signSurfaceBlockMesh.Vertices.Array[signSurfaceBlockMesh.Indices.Array[i * 3 + 2]];
					Vector3 p11 = blockMeshVertex11.Position + vector + v;
					Vector3 p22 = blockMeshVertex22.Position + vector + v;
					Vector3 p33 = blockMeshVertex33.Position + vector + v;
					Vector2 textureCoordinates11 = ILibrary.getTexcoord(2);
					Vector2 textureCoordinates22 = ILibrary.getTexcoord(4);
					Vector2 textureCoordinates33 = ILibrary.getTexcoord(1);
					texturedBatch3D.QueueTriangle(p11, p22, p33, textureCoordinates11, textureCoordinates22, textureCoordinates33, color);
				}
				Updated(singleChest.GetSlotValue(0),singleChest.GetSlotCount(0));
				primitivesRenderer3D.Flush(camera.ViewProjectionMatrix);
				
			}
		}
		public void Updated(int value,int count)
			{
			RenderTarget2D renderTarget = Display.RenderTarget;
			Display.RenderTarget = m_renderTarget;
			Display.Clear(Color.Transparent);
			try
			{
				string numstr = count.ToString();
				int usedw = numstr.Length * 16;
				int padding = (256 - usedw) / 2;
				//base x 128 base y 384 efont 16 all w 256 
				//base x 4 y 12
				FontBatch2D fontBatch = primitivesRenderer2D.FontBatch(ILibrary.m_font, 1, DepthStencilState.None, null, BlendState.Opaque, SamplerState.PointClamp);		
				fontBatch.QueueText($"{numstr}", new Vector2(128 + padding, 384), 0f, Color.White, TextAnchor.HorizontalCenter, new Vector2(4, 4), Vector2.Zero);//不是纹理坐标				
				PrimitivesRenderer3D mprimitivesRenderer3D = new PrimitivesRenderer3D();
				int id = Terrain.ExtractContents(value);
				Block block = BlocksManager.Blocks[id];
				DrawBlockEnvironmentData blockEnvironmentData = new DrawBlockEnvironmentData();
				Matrix m2 = Matrix.CreateLookAt(block.GetIconViewOffset(value, blockEnvironmentData), Vector3.Zero, Vector3.UnitY);
				m2 *= Matrix.CreateOrthographic(3.6f, 3.6f, -10f - 1f * 1f, 10f - 1f * 1f);
				m2 *= Matrix.CreateTranslation(0, 0.2f, 0);
				Matrix matrix = Matrix.Identity;
				blockEnvironmentData.ViewProjectionMatrix = m2;
				block.DrawBlock(mprimitivesRenderer3D, value, Color.White, 0.8f, ref matrix, blockEnvironmentData);
				mprimitivesRenderer3D.Flush(matrix);
				primitivesRenderer2D.Flush();
				Display.RenderTarget = renderTarget;
			}
			catch (Exception e) {
				Log.Information("error display"+e.ToString());
			}			
			}
		}
	} 
