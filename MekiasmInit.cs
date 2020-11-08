using System;
using System.Collections.Generic;
using Engine;
using Engine.Media;
using Engine.Graphics;
using Game;
namespace Mekiasm
{
    [PluginLoader("Mekiasm", "通用机械Mod", 1u)]
    public class MekiasmInit
    {        
        public static DynamicArray<Item> items_ore = new DynamicArray<Item>();
        public static DynamicArray<Item> items_orechunk = new DynamicArray<Item>();
        public static DynamicArray<Item> items_tool = new DynamicArray<Item>();
        public static DynamicArray<Item> items_electric = new DynamicArray<Item>();
        public static DynamicArray<Item> items_flat = new DynamicArray<Item>();
        public static DynamicArray<Item> items_chest = new DynamicArray<Item>();//储物柜箱子
        public static DynamicArray<Item> items_alpha = new DynamicArray<Item>();//透明方块
        public static DynamicArray<Item> items_fluid = new DynamicArray<Item>();
        public static DynamicArray<Item> items_plant = new DynamicArray<Item>();
        public static Dictionary<string, Texture2D> imgres = new Dictionary<string, Texture2D>();
        public static DynamicArray<BlockMesh> faceMeshes = new DynamicArray<BlockMesh>();
        public static DynamicArray<Vector3> faceNormals = new DynamicArray<Vector3>();
        public static BitmapFont mfont;
        public static Model CubeModel;
        public static string itemFlatPath="Mekiasm/Textures/Items/";
        public static void Initialize()
        {
            DebugCamera.get_IsEntityControlEnabled1 = True;
            FlyCamera.get_IsEntityControlEnabled1 = True;
            RandomJumpCamera.get_IsEntityControlEnabled1 = True;
            StraightFlightCamera.get_IsEntityControlEnabled1 = True;
            MekCraftingRecipe.initRecipes();//初始化合成谱
            ScreensManager.Initialized += init_;
            ScreensManager.Initialized+=ILibrary.Init;
        }

        public static bool True(object obj)
        {
            return true;
        }
        public static void init_() {
             Model model = ContentManager.Get<Model>("Mekiasm/Models/Cube");           
            CubeModel = model;
            for (int i = 0; i < 6; i++)
            {
                Matrix m1 = Matrix.CreateRotationY((float)i * (float)Math.PI / 4f) * Matrix.CreateTranslation(0.5f, 0f, 0.5f);
                faceNormals.Add(-m1.Forward);
            }
            for (int i = 0; i < 6; i++)
            {
                Matrix m1 = Matrix.CreateRotationX((float)i * (float)Math.PI / 4f) * Matrix.CreateTranslation(0.5f, 0f, 0.5f);
                faceNormals.Add(-m1.Forward);
            }
            for (int i = 0; i < 6; i++)
            {
                Matrix m1 = Matrix.CreateRotationZ((float)i * (float)Math.PI / 4f) * Matrix.CreateTranslation(0.5f, 0f, 0.5f);
                faceNormals.Add(-m1.Forward);
            }
            Matrix matrix = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Front").ParentBone);
            BlockMesh blockMesh = new BlockMesh();
            blockMesh.AppendModelMeshPart(model.FindMesh("Front").MeshParts[0], matrix * Matrix.CreateTranslation(0.5f, 0.5f, 0.5f), false, false, false, false, Color.White);
            faceMeshes.Add(blockMesh);

            Matrix matrix1 = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Right").ParentBone);
            BlockMesh blockMesh1 = new BlockMesh();
            blockMesh1.AppendModelMeshPart(model.FindMesh("Right").MeshParts[0], matrix1 * Matrix.CreateTranslation(0.5f, 0.5f, 0.5f), false, false, false, false, Color.White);
            faceMeshes.Add(blockMesh1);


            Matrix matrix2 = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Behind").ParentBone);
            BlockMesh blockMesh2 = new BlockMesh();
            blockMesh2.AppendModelMeshPart(model.FindMesh("Behind").MeshParts[0], matrix2 * Matrix.CreateTranslation(0.5f, 0.5f, 0.5f), false, false, false, false, Color.White);
            faceMeshes.Add(blockMesh2);


            Matrix matrix3 = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Left").ParentBone);
            BlockMesh blockMesh3 = new BlockMesh();
            blockMesh3.AppendModelMeshPart(model.FindMesh("Left").MeshParts[0], matrix3 * Matrix.CreateTranslation(0.5f, 0.5f, 0.5f), false, false, false, false, Color.White);
            faceMeshes.Add(blockMesh3);


            Matrix matrix4 = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Top").ParentBone);
            BlockMesh blockMesh4 = new BlockMesh();
            blockMesh4.AppendModelMeshPart(model.FindMesh("Top").MeshParts[0], matrix4 * Matrix.CreateTranslation(0.5f, 0.5f, 0.5f), false, false, false, false, Color.White);
            faceMeshes.Add(blockMesh4);


            Matrix matrix5 = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Bottom").ParentBone);
            BlockMesh blockMesh5 = new BlockMesh();
            blockMesh5.AppendModelMeshPart(model.FindMesh("Bottom").MeshParts[0], matrix5 * Matrix.CreateTranslation(0.5f, 0.5f, 0.5f), false, false, false, false, Color.White);
            faceMeshes.Add(blockMesh5);
            //id表示textureslot位置
            //oreblock
            { 
            items_ore.Add(new ItemCube(0, "金矿石"));
            items_ore.Add(new ItemCube(1, "铀矿石"));
            items_ore.Add(new ItemCube(2, "锇矿石"));
            items_ore.Add(new ItemCube(3, "钚矿石"));
            items_ore.Add(new ItemCube(4, "锂矿石"));
            items_ore.Add(new ItemCube(5, "镁矿石"));
            items_ore.Add(new ItemCube(6, "硼矿石"));
            items_ore.Add(new ItemCube(7, "铅矿石"));
            items_ore.Add(new ItemCube(9, "钍矿石"));
            items_ore.Add(new ItemCube(10,"锡矿石"));
            items_ore.Add(new ItemCube(11,"红石矿石"));
            }

            //orechunkblock
            {
                items_orechunk.Add(new ItemOreChunk("金块", 0, Color.Yellow,false));
                items_orechunk.Add(new ItemOreChunk("铀块", 1, Color.LightGreen,false));
                items_orechunk.Add(new ItemOreChunk("锇块", 2, new Color(122,188,230),false));
                items_orechunk.Add(new ItemOreChunk("钚块", 3, new Color(188,203,213),false));
                items_orechunk.Add(new ItemOreChunk("锂块", 4, Color.White,false));
                items_orechunk.Add(new ItemOreChunk("镁块", 5, Color.LightMagenta,false));
                items_orechunk.Add(new ItemOreChunk("硼块", 6, Color.LightGray,false));
                items_orechunk.Add(new ItemOreChunk("铅块", 7, new Color(90,148,148),false));
                items_orechunk.Add(new ItemOreChunk("钍块", 9, Color.Black,false));
                items_orechunk.Add(new ItemOreChunk("锡块", 10, Color.DarkGray,false));
                items_orechunk.Add(new ItemOreChunk("金锭", 11, Color.Yellow, true));
                items_orechunk.Add(new ItemOreChunk("铀锭", 12, Color.LightGreen, true));
                items_orechunk.Add(new ItemOreChunk("锇锭", 13, new Color(122, 188, 230), true));
                items_orechunk.Add(new ItemOreChunk("钚锭", 14, new Color(188, 203, 213), true));
                items_orechunk.Add(new ItemOreChunk("锂锭", 15, Color.White, true));
                items_orechunk.Add(new ItemOreChunk("镁锭", 16, Color.LightMagenta, true));
                items_orechunk.Add(new ItemOreChunk("硼锭", 17, Color.LightGray, true));
                items_orechunk.Add(new ItemOreChunk("铅锭", 18, new Color(90, 148, 148), true));
                items_orechunk.Add(new ItemOreChunk("钍锭", 19, Color.Black, true));
                items_orechunk.Add(new ItemOreChunk("锡锭", 20, Color.DarkGray, true));
            }

            //electric
            {//1003
                items_electric.Add(new ItemBucket("氘桶", 0, Color.DarkBlue));
                items_electric.Add(new ItemBucket("氚桶", 1, Color.LightBlue));
                items_electric.Add(new ItemBucket("氦桶", 2, Color.Magenta));
                items_electric.Add(new MekCoalGenerator(27, "燃煤发电机"));//and26
                items_electric.Add(new AspSolarGenerator(30, "ASP基础太阳能发电"));//and29,28
                items_electric.Add(new ItemCube(31, "钢质框架"));
                items_electric.Add(new ItemCube(32, "基础能量输导元件"));
                items_electric.Add(new ItemCube(33, "高级能量输导元件"));
                items_electric.Add(new ItemCube(34, "精英能量输导元件"));
                items_electric.Add(new ItemCube(35, "终级能量输导元件"));
                items_electric.Add(new ItemCube(36, "输导端口"));//and37
                items_electric.Add(new ItemCube(38, "输导套管"));
                items_electric.Add(new ItemCube(40, "动态阀门"));
                items_electric.Add(new ItemCube(41, "动态储罐"));
                items_electric.Add(new ItemCube(42, "热力蒸馏方块"));
                items_electric.Add(new ItemCube(43, "热力蒸馏控制阀门"));
                items_electric.Add(new MekDistillController(44, "热力蒸馏控制器"));//and45
                items_electric.Add(new ItemCrusher(47, "粉碎机"));
                items_electric.Add(new ItemEnergyBase(63,"基础能量立方"));
                items_electric.Add(new ItemCube(64, "传送机"));
                items_electric.Add(new MekTransmitBox(65, "传送框架"));
                items_electric.Add(new MekWire(48, "基础能量传输导线"));
                items_electric.Add(new MekWire(49, "高级能量传输导线"));
                items_electric.Add(new MekWire(50, "精英能量传输导线"));
                items_electric.Add(new MekWire(51, "终极能量传输导线"));
                items_electric.Add(new ItemCube(66, "基础能量输导核心"));
                items_electric.Add(new ItemCube(67, "高级能量输导核心"));
                items_electric.Add(new ItemCube(68, "精英能量输导核心"));
                items_electric.Add(new ItemCube(70, "终极能量输导核心"));
                items_electric.Add(new AlloyFurnace(80, "合金炉"));
                items_electric.Add(new Manufactory(82, "制造厂"));
                items_electric.Add(new MekSmelt(72, "充能冶炼炉"));
                items_electric.Add(new MekEnrich(78, "富集仓"));
                items_electric.Add(new ItemCube(81, "裂变结构方块"));
                items_electric.Add(new FissionController(89, "裂变控制器"));
                items_electric.Add(new FusionElectromagnet(90, "电磁铁"));
                items_electric.Add(new ItemCube(96, "镁冷凝器"));
                items_electric.Add(new ItemCube(97, "金冷凝器"));
                items_electric.Add(new ItemCube(98, "青晶石冷凝器"));
                items_electric.Add(new ItemCube(99, "水冷凝器"));
                items_electric.Add(new ItemCube(101, "锡冷凝器"));
                items_electric.Add(new ItemCube(102, "氦冷凝器"));
                items_electric.Add(new ItemCube(103, "金刚石冷凝器"));
                items_electric.Add(new MekIsotopeSeparator(104, "同位素分离机"));
                items_electric.Add(new MekCombiner(107,"融合机"));
                items_electric.Add(new MekPurification(108, "净化仓"));
                items_electric.Add(new MekElectrolyzer(119, "电解机"));
                items_electric.Add(new MekChemicalReactor(121, "化学反应器"));
                items_electric.Add(new MekSaltMixer(123, "盐混合器"));
                items_flat.Add(new ItemFlat(500, "便携传送器", ContentManager.Get<Texture2D>(itemFlatPath + "portableteleporter")));
                items_flat.Add(new ItemFlat(501, "压缩碳", ContentManager.Get<Texture2D>(itemFlatPath+"compressedcarbon")));
                items_flat.Add(new ItemFlat(502, "压缩红石", ContentManager.Get<Texture2D>(itemFlatPath + "compressedredstone")));
                items_flat.Add(new ItemFlat(503, "压缩钻石", ContentManager.Get<Texture2D>(itemFlatPath + "compresseddiamond")));
                items_flat.Add(new ItemFlat(504, "压缩黑曜石", ContentManager.Get<Texture2D>(itemFlatPath + "compressedobsidian")));
                items_flat.Add(new ItemFlat(505, "基础电路板", ContentManager.Get<Texture2D>(itemFlatPath + "basiccontrolcircuit")));
                items_flat.Add(new ItemFlat(506, "高级电路板", ContentManager.Get<Texture2D>(itemFlatPath + "advancedcontrolcircuit")));
                items_flat.Add(new ItemFlat(507, "精英电路板", ContentManager.Get<Texture2D>(itemFlatPath + "elitecontrolcircuit")));
                items_flat.Add(new ItemFlat(508, "终极电路板", ContentManager.Get<Texture2D>(itemFlatPath + "ultimatecontrolcircuit")));
                items_flat.Add(new ItemFlat(510, "调整器", ContentManager.Get<Texture2D>(itemFlatPath + "configurator")));
                items_flat.Add(new ItemFlat(511, "石墨粉", ContentManager.Get<Texture2D>(itemFlatPath + "dust_graphite")));
                items_flat.Add(new ItemFlat(512, "石墨锭", ContentManager.Get<Texture2D>(itemFlatPath + "ingot_graphite")));
                items_flat.Add(new ItemFlat(513, "钢粉", ContentManager.Get<Texture2D>(itemFlatPath + "steeldust")));
                items_flat.Add(new ItemFlat(514, "钢锭", ContentManager.Get<Texture2D>(itemFlatPath + "steelingot")));                
                items_flat.Add(new ItemFlat(515, "能量升级", ContentManager.Get<Texture2D>(itemFlatPath + "upgrade_energy")));
                items_flat.Add(new ItemFlat(516, "速度升级", ContentManager.Get<Texture2D>(itemFlatPath + "upgrade_speed")));
                items_flat.Add(new ItemFlat(517, "盐", ContentManager.Get<Texture2D>(itemFlatPath + "salt")));
                items_flat.Add(new ItemFlat(518, "镅-241", ContentManager.Get<Texture2D>(itemFlatPath + "americium_241")));
                items_flat.Add(new ItemFlat(519, "镅-242", ContentManager.Get<Texture2D>(itemFlatPath + "americium_242")));
                items_flat.Add(new ItemFlat(520, "氧化镅-241", ContentManager.Get<Texture2D>(itemFlatPath + "americium_241_oxide")));
                items_flat.Add(new ItemFlat(521, "氧化镅-242", ContentManager.Get<Texture2D>(itemFlatPath + "americium_242_oxide")));
                items_flat.Add(new ItemFlat(522, "镅-243", ContentManager.Get<Texture2D>(itemFlatPath + "americium_243")));
                items_flat.Add(new ItemFlat(523, "氧化镅-243", ContentManager.Get<Texture2D>(itemFlatPath + "americium_243_oxide")));
                items_flat.Add(new ItemFlat(524, "锫-247", ContentManager.Get<Texture2D>(itemFlatPath + "berkelium_247")));
                items_flat.Add(new ItemFlat(525, "锫-248", ContentManager.Get<Texture2D>(itemFlatPath + "berkelium_248")));
                items_flat.Add(new ItemFlat(526, "氧化锫-247", ContentManager.Get<Texture2D>(itemFlatPath + "berkelium_247_oxide")));
                items_flat.Add(new ItemFlat(527, "氧化锫-248", ContentManager.Get<Texture2D>(itemFlatPath + "berkelium_248_oxide")));
                items_flat.Add(new ItemFlat(528, "生物燃料", ContentManager.Get<Texture2D>(itemFlatPath + "biofuel")));
                items_flat.Add(new ItemFlat(529, "硼-10", ContentManager.Get<Texture2D>(itemFlatPath + "boron_10")));
                items_flat.Add(new ItemFlat(530, "硼-11", ContentManager.Get<Texture2D>(itemFlatPath + "boron_11")));
                items_flat.Add(new ItemFlat(531, "锎-249", ContentManager.Get<Texture2D>(itemFlatPath + "californium_249")));
                items_flat.Add(new ItemFlat(532, "锎-250", ContentManager.Get<Texture2D>(itemFlatPath + "californium_250")));
                items_flat.Add(new ItemFlat(533, "锎-251", ContentManager.Get<Texture2D>(itemFlatPath + "californium_251")));
                items_flat.Add(new ItemFlat(534, "氧化锎-249", ContentManager.Get<Texture2D>(itemFlatPath + "californium_249_oxide")));
                items_flat.Add(new ItemFlat(535, "氧化锎-250", ContentManager.Get<Texture2D>(itemFlatPath + "californium_250_oxide")));
                items_flat.Add(new ItemFlat(536, "氧化锎-251", ContentManager.Get<Texture2D>(itemFlatPath + "californium_251_oxide")));
                items_flat.Add(new ItemFlat(537, "锔-243", ContentManager.Get<Texture2D>(itemFlatPath + "curium_243")));
                items_flat.Add(new ItemFlat(538, "锔-245", ContentManager.Get<Texture2D>(itemFlatPath + "curium_245")));
                items_flat.Add(new ItemFlat(539, "锔-246", ContentManager.Get<Texture2D>(itemFlatPath + "curium_246")));
                items_flat.Add(new ItemFlat(540, "锔-247", ContentManager.Get<Texture2D>(itemFlatPath + "curium_247")));
                items_flat.Add(new ItemFlat(541, "氧化锔-243", ContentManager.Get<Texture2D>(itemFlatPath + "curium_243_oxide")));
                items_flat.Add(new ItemFlat(542, "氧化锔-245", ContentManager.Get<Texture2D>(itemFlatPath + "curium_245_oxide")));
                items_flat.Add(new ItemFlat(543, "氧化锔-246", ContentManager.Get<Texture2D>(itemFlatPath + "curium_246_oxide")));
                items_flat.Add(new ItemFlat(544, "氧化锔-247", ContentManager.Get<Texture2D>(itemFlatPath + "curium_247_oxide")));
                items_flat.Add(new ItemFlat(545, "TBU-钍燃料", ContentManager.Get<Texture2D>(itemFlatPath + "fuel_thorium_tbu")));
                items_flat.Add(new ItemFlat(546, "TBU-氧化钍燃料", ContentManager.Get<Texture2D>(itemFlatPath + "fuel_thorium_tbu_oxide")));
                items_flat.Add(new ItemFlat(547, "HEU-铀233燃料", ContentManager.Get<Texture2D>(itemFlatPath + "fuel_uranium_heu_233")));
                items_flat.Add(new ItemFlat(548, "HEU-氧化铀233燃料", ContentManager.Get<Texture2D>(itemFlatPath + "fuel_uranium_heu_233_oxide")));
                items_flat.Add(new ItemFlat(549, "HEU-铀235燃料", ContentManager.Get<Texture2D>(itemFlatPath + "fuel_uranium_heu_235")));
                items_flat.Add(new ItemFlat(550, "HEU-氧化铀235燃料", ContentManager.Get<Texture2D>(itemFlatPath + "fuel_uranium_heu_235_oxide")));
                items_flat.Add(new ItemFlat(551, "LEU-铀233燃料", ContentManager.Get<Texture2D>(itemFlatPath + "fuel_uranium_leu_233")));
                items_flat.Add(new ItemFlat(552, "LEU-氧化铀233燃料", ContentManager.Get<Texture2D>(itemFlatPath + "fuel_uranium_leu_233_oxide")));
                items_flat.Add(new ItemFlat(553, "LEU-铀235燃料", ContentManager.Get<Texture2D>(itemFlatPath + "fuel_uranium_leu_235")));
                items_flat.Add(new ItemFlat(554, "LEU-氧化铀235燃料", ContentManager.Get<Texture2D>(itemFlatPath + "fuel_uranium_leu_235_oxide")));                
                items_flat.Add(new ItemFlat(555, "高级工厂安装器", ContentManager.Get<Texture2D>(itemFlatPath + "advancedtierinstaller")));
                items_flat.Add(new ItemFlat(556, "基础工厂安装器", ContentManager.Get<Texture2D>(itemFlatPath + "basictierinstaller")));
                items_flat.Add(new ItemFlat(557, "精英工厂安装器", ContentManager.Get<Texture2D>(itemFlatPath + "elitetierinstaller")));
                items_flat.Add(new ItemFlat(558, "终极工厂安装器", ContentManager.Get<Texture2D>(itemFlatPath + "compressedcarbon")));
                items_flat.Add(new ItemFlat(559, "能量板", ContentManager.Get<Texture2D>(itemFlatPath + "energytablet")));
                items_flat.Add(new ItemFlat(560, "富集合金", ContentManager.Get<Texture2D>(itemFlatPath + "enrichedalloy")));
                items_flat.Add(new ItemFlat(561, "富集铁", ContentManager.Get<Texture2D>(itemFlatPath + "enrichediron")));
                items_flat.Add(new ItemFlat(562, "富集铁锭", ContentManager.Get<Texture2D>(itemFlatPath + "enrichediron_alt")));
                items_flat.Add(new ItemFlat(563, "金粉", ContentManager.Get<Texture2D>(itemFlatPath + "golddust")));
                items_flat.Add(new ItemFlat(564, "金碎片", ContentManager.Get<Texture2D>(itemFlatPath + "goldshard")));
                items_flat.Add(new ItemFlat(565, "金块", ContentManager.Get<Texture2D>(itemFlatPath + "goldclump")));
                items_flat.Add(new ItemFlat(566, "金晶体", ContentManager.Get<Texture2D>(itemFlatPath + "goldclump")));
                items_flat.Add(new ItemFlat(567, "硫粉", ContentManager.Get<Texture2D>(itemFlatPath + "gem_dust_sulfur")));
                items_flat.Add(new ItemFlat(568, "铁粉", ContentManager.Get<Texture2D>(itemFlatPath + "irondust")));
                items_flat.Add(new ItemFlat(569, "铁碎片", ContentManager.Get<Texture2D>(itemFlatPath + "ironshard")));
                items_flat.Add(new ItemFlat(570, "铁块", ContentManager.Get<Texture2D>(itemFlatPath + "ironclump")));
                items_flat.Add(new ItemFlat(571, "铁晶体", ContentManager.Get<Texture2D>(itemFlatPath + "ironcrystal")));
                items_flat.Add(new ItemFlat(572, "铜粉", ContentManager.Get<Texture2D>(itemFlatPath + "copperdust")));
                items_flat.Add(new ItemFlat(573, "铜碎片", ContentManager.Get<Texture2D>(itemFlatPath + "coppershard")));
                items_flat.Add(new ItemFlat(574, "铜块", ContentManager.Get<Texture2D>(itemFlatPath + "copperclump")));
                items_flat.Add(new ItemFlat(575, "铜晶体", ContentManager.Get<Texture2D>(itemFlatPath + "coppercrystal")));
                items_flat.Add(new ItemFlat(576, "锇粉", ContentManager.Get<Texture2D>(itemFlatPath + "osmiumdust")));
                items_flat.Add(new ItemFlat(577, "锇碎片", ContentManager.Get<Texture2D>(itemFlatPath + "osmiumshard")));
                items_flat.Add(new ItemFlat(578, "锇块", ContentManager.Get<Texture2D>(itemFlatPath + "osmiumclump")));
                items_flat.Add(new ItemFlat(579, "锇晶体", ContentManager.Get<Texture2D>(itemFlatPath + "osmiumcrystal")));
                items_flat.Add(new ItemFlat(580, "锂-6", ContentManager.Get<Texture2D>(itemFlatPath + "lithium_6")));
                items_flat.Add(new ItemFlat(581, "锂-7", ContentManager.Get<Texture2D>(itemFlatPath + "lithium_7")));
                items_flat.Add(new ItemFlat(582, "污浊的铜粉", ContentManager.Get<Texture2D>(itemFlatPath + "dirtycopperdust")));
                items_flat.Add(new ItemFlat(583, "污浊的铁粉", ContentManager.Get<Texture2D>(itemFlatPath + "dirtyirondust")));
                items_flat.Add(new ItemFlat(584, "污浊的金粉", ContentManager.Get<Texture2D>(itemFlatPath + "dirtygolddust")));
                items_flat.Add(new ItemFlat(585, "污浊的锇粉", ContentManager.Get<Texture2D>(itemFlatPath + "dirtyosmiumdust")));
                items_flat.Add(new ItemFlat(586, "污浊的铅粉", ContentManager.Get<Texture2D>(itemFlatPath + "dirtyleaddust")));
                items_flat.Add(new ItemFlat(587, "铅粉", ContentManager.Get<Texture2D>(itemFlatPath + "leaddust")));
                items_flat.Add(new ItemFlat(588, "污浊的银粉", ContentManager.Get<Texture2D>(itemFlatPath + "dirtysilverdust")));
                items_flat.Add(new ItemFlat(589, "辐射药剂", ContentManager.Get<Texture2D>(itemFlatPath + "rad_x")));
                items_flat.Add(new ItemFlat(590, "辐射清除剂", ContentManager.Get<Texture2D>(itemFlatPath + "radaway")));
                items_flat.Add(new ItemFlat(591, "辐射清除剂(缓慢)", ContentManager.Get<Texture2D>(itemFlatPath + "radaway_slow")));
          
            }

            //chest
            {
                items_chest.Add(new BaseSingleChest(0, "基础储柜"));
                items_chest.Add(new BaseSingleChest(1, "高级储柜"));
                items_chest.Add(new BaseSingleChest(2, "精英储柜"));
                items_chest.Add(new BaseSingleChest(3, "终极储柜"));
            }
            //item-pipe
            items_electric.Add(new MekWire(52, "基础加压管道"));
            items_electric.Add(new MekWire(53, "高级加压管道"));
            items_electric.Add(new MekWire(54, "精英加压管道"));
            items_electric.Add(new MekWire(55, "终极加压管道"));
            items_electric.Add(new MekWire(56, "基础热导线缆"));
            items_electric.Add(new MekWire(57, "高级热导线缆"));
            items_electric.Add(new MekWire(58, "精英热导线缆"));
            items_electric.Add(new MekWire(59, "终极热导线缆"));


            //alphablock-1007
            items_alpha.Add(new ItemFliud(0, new Color(0,0,0,125),"传送门方块"));
            items_alpha.Add(new MekBaseSolarPanel(25, "基础太阳能板"));
            items_alpha.Add(new ItemMekWire(26, "线缆"));
            items_alpha.Add(new StructGlass(39, "结构玻璃"));
            items_alpha.Add(new ItemCube(100, "空的冷凝器"));
            items_alpha.Add(new MekAdvanceSolarPanel(101,"高级太阳能发电机"));
            items_alpha.Add(new MekFengliGenerator(102,"风力发电机"));

            items_fluid.Add(new ItemHFluid(0, "氘",7,Color.Gray,Color.Gray));
            items_fluid.Add(new ItemHFluid(1, "氚",7,Color.Magenta,Color.Magenta));

            items_plant.Add(new ItemPlant(0,"橡胶树苗"));
            BlocksManager.m_categories.Add("通用机械-地形");
            BlocksManager.m_categories.Add("通用机械-机器");
            BlocksManager.m_categories.Add("通用机械-工具");
            BlocksManager.m_categories.Add("通用机械-核电");
            BlocksManager.m_categories.Add("通用机械-管道");
            mfont = ContentManager.Get<BitmapFont>("Fonts/SignFont");
        }
    }
}
