using System.Collections.Generic;
using WpfApp2.Domain.Entities;
using WpfApp2.Domain;
using SQLWriter;

namespace WpfApp2.WPFForm2.Helpers
{
	public class TestMockHelper
	{
		public static void Initialize()
		{

			SQLWriterFacade.Initialize();

			List<DevelopmentOrderEntity> deo = new List<DevelopmentOrderEntity>();

			deo.Add(new DevelopmentOrderEntity("0-76-HFR603"));
			deo.Add(new DevelopmentOrderEntity("0-76-HFR607"));
			deo.Add(new DevelopmentOrderEntity("0-76-HEQ003"));
			deo.Add(new DevelopmentOrderEntity("0-76-GER558"));
			deo.Add(new DevelopmentOrderEntity("1-76-NLD445"));
			deo.Add(new DevelopmentOrderEntity("0-76-CHN603"));
			deo.Add(new DevelopmentOrderEntity("0-76-CHN631"));
			deo.Add(new DevelopmentOrderEntity("1-76-CHN677"));
			deo.Add(new DevelopmentOrderEntity("0-76-CHN739"));
			deo.Add(new DevelopmentOrderEntity("0-76-CAN111"));
			deo.Add(new DevelopmentOrderEntity("1-76-ESP214"));
			deo.Add(new DevelopmentOrderEntity("0-76-ESP215"));
			deo.Add(new DevelopmentOrderEntity("0-76-ESP216"));
			deo.Add(new DevelopmentOrderEntity("0-76-HNK111"));
			foreach (DevelopmentOrderEntity p in deo)
			{
				SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(p);
			}

			List<CustomerEntity> ce = new List<CustomerEntity>();

			ce.Add(new CustomerEntity("JR東海"));
			ce.Add(new CustomerEntity("東京METRO"));
			ce.Add(new CustomerEntity("シーメンス"));
			ce.Add(new CustomerEntity("アルストム"));
			ce.Add(new CustomerEntity("中国中車"));
			ce.Add(new CustomerEntity("ボンバルディア"));
			ce.Add(new CustomerEntity("CAF"));
			ce.Add(new CustomerEntity("川崎重工"));
			foreach (CustomerEntity p in ce)
			{
				SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(p);
			}

			List<ProjectEntity> pe = new List<ProjectEntity>();
			pe.Add(new ProjectEntity(1, 1, "東海道新幹線 N700系"));
			pe.Add(new ProjectEntity(2, 1, "東北新幹線 E6系"));
			pe.Add(new ProjectEntity(3,2, "有楽町線・副都心線17000系"));
			pe.Add(new ProjectEntity(4, 3, "ベルリン高速鉄道 新線"));
			pe.Add(new ProjectEntity(5, 4, "ロッテルダム観光鉄道"));
			pe.Add(new ProjectEntity(6, 5, "北京地下鉄 3号線"));
			pe.Add(new ProjectEntity(7, 5, "北京地下鉄 4号線"));
			pe.Add(new ProjectEntity(8, 5, "上海地下鉄 2号線"));
			pe.Add(new ProjectEntity(9, 5, "上海地下鉄 13号線"));
			pe.Add(new ProjectEntity(10, 6, "カナダ横断鉄道"));
			pe.Add(new ProjectEntity(11, 7, "バルセロナ地下鉄 1号線"));
			pe.Add(new ProjectEntity(12, 7, "バルセロナ地下鉄 2号線"));
			pe.Add(new ProjectEntity(13, 7, "バルセロナ地下鉄 3号線"));
			pe.Add(new ProjectEntity(14, 8, "阪急神戸線 通勤特急"));
			foreach (ProjectEntity p in pe)
			{
				SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(p);
			}

			List<ModelEntity> me = new List<ModelEntity>();

			me.Add(new ModelEntity("MAP-243-15VD288"));
			me.Add(new ModelEntity("MAP-243-15VD303"));
			me.Add(new ModelEntity("MAP-243-15VD224"));
			me.Add(new ModelEntity("MAP-214-10V335"));
			me.Add(new ModelEntity("MAP-114-11V035"));
			me.Add(new ModelEntity("MAP-314-12V125"));
			//me.Add(new ModelEntity("MAP-414-13V400"));
			//me.Add(new ModelEntity("MAP-514-14V401"));
			//me.Add(new ModelEntity("MAP-614-15V556"));
			//me.Add(new ModelEntity("MAP-714-16V789"));
			//me.Add(new ModelEntity("MAP-814-17V999"));

			foreach (ModelEntity p in me)
			{
				SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(p);
			}

			//List<SpecSheetBaseEntity> ss = new List<SpecSheetBaseEntity>();

			//ss.Add(new SpecSheetBaseEntity("AAA001", 1, 1, 1));
			//ss.Add(new SpecSheetBaseEntity("AAA001", 1, 1, 2));
			//ss.Add(new SpecSheetBaseEntity("AAA001", 1, 1, 3));
			//ss.Add(new SpecSheetBaseEntity("AAA003", 3));
			//ss.Add(new SpecSheetBaseEntity("AAA004", 4));
			//ss.Add(new SpecSheetBaseEntity("AAA005", 5));
			//ss.Add(new SpecSheetBaseEntity("AAA006", 6));
			//ss.Add(new SpecSheetBaseEntity("AAA007", 7));

			//foreach (SpecSheetBaseEntity p in ss)
			//{
			//	Factories.CreateEntity().Save(p);
			//}

			//List<SpecSheetTemplateCombinationEntity> sst = new List<SpecSheetTemplateCombinationEntity>();
			//sst.Add(new SpecSheetTemplateCombinationEntity(1, "-"));
			//sst.Add(new SpecSheetTemplateCombinationEntity(1, "A"));
			//sst.Add(new SpecSheetTemplateCombinationEntity(1, "B"));
			//sst.Add(new SpecSheetTemplateCombinationEntity(2, "-"));
			//sst.Add(new SpecSheetTemplateCombinationEntity(2, "A"));
			//sst.Add(new SpecSheetTemplateCombinationEntity(3, "A"));
			//sst.Add(new SpecSheetTemplateCombinationEntity(4, "-"));
			//sst.Add(new SpecSheetTemplateCombinationEntity(5, "-"));
			//sst.Add(new SpecSheetTemplateCombinationEntity(6, "-"));
			//sst.Add(new SpecSheetTemplateCombinationEntity(6, "A"));
			//sst.Add(new SpecSheetTemplateCombinationEntity(6, "B"));
			//sst.Add(new SpecSheetTemplateCombinationEntity(6, "C"));
			//sst.Add(new SpecSheetTemplateCombinationEntity(7, "-"));
			//sst.Add(new SpecSheetTemplateCombinationEntity(7, 7, "A"));
			//sst.Add(new SpecSheetTemplateCombinationEntity(8, 8, "-"));
			//sst.Add(new SpecSheetTemplateCombinationEntity(9, 8, "-"));
			//sst.Add(new SpecSheetTemplateCombinationEntity(10, 9, "-"));
			//sst.Add(new SpecSheetTemplateCombinationEntity(10, 9, "A"));
			//sst.Add(new SpecSheetTemplateCombinationEntity(11, 10, "-"));
			//sst.Add(new SpecSheetTemplateCombinationEntity(11, 11, "-"));
			//sst.Add(new SpecSheetTemplateCombinationEntity(12, 11, "-"));
			//sst.Add(new SpecSheetTemplateCombinationEntity(13, 11, "-"));

			//foreach (SpecSheetTemplateCombinationEntity p in sst)
			//{
			//	Factories.CreateEntity().Save(p);
			//}

			//List<SpecSheetTemplateDataEntity> ssd = new List<SpecSheetTemplateDataEntity>();

			//ssd.Add(new SpecSheetTemplateDataEntity(1, "1"));
			//ssd.Add(new SpecSheetTemplateDataEntity(1, "2"));
			//ssd.Add(new SpecSheetTemplateDataEntity(1, "3"));
			//ssd.Add(new SpecSheetTemplateDataEntity(2, "1"));
			//ssd.Add(new SpecSheetTemplateDataEntity(3, "1"));
			//ssd.Add(new SpecSheetTemplateDataEntity(4, "1"));
			//ssd.Add(new SpecSheetTemplateDataEntity(5, "1"));
			//ssd.Add(new SpecSheetTemplateDataEntity(6, "1"));
			//ssd.Add(new SpecSheetTemplateDataEntity(6, "2"));
			//ssd.Add(new SpecSheetTemplateDataEntity(8, "1"));
			//ssd.Add(new SpecSheetTemplateDataEntity(9, "1"));
			//ssd.Add(new SpecSheetTemplateDataEntity(9, "2"));
			//ssd.Add(new SpecSheetTemplateDataEntity(11, "1"));
			//ssd.Add(new SpecSheetTemplateDataEntity(11, "2"));
			//ssd.Add(new SpecSheetTemplateDataEntity(11, "3"));
			//ssd.Add(new SpecSheetTemplateDataEntity(12, "1"));
			//ssd.Add(new SpecSheetTemplateDataEntity(12, "2"));
			//ssd.Add(new SpecSheetTemplateDataEntity(13, "1"));
			//ssd.Add(new SpecSheetTemplateDataEntity(13, "2"));
			//ssd.Add(new SpecSheetTemplateDataEntity(14, "1"));
			//ssd.Add(new SpecSheetTemplateDataEntity(14, "2"));
			//ssd.Add(new SpecSheetTemplateDataEntity(14, "3"));
			//ssd.Add(new SpecSheetTemplateDataEntity(15, "1"));
			//ssd.Add(new SpecSheetTemplateDataEntity(16, "1"));
			//ssd.Add(new SpecSheetTemplateDataEntity(17, "1"));
			//ssd.Add(new SpecSheetTemplateDataEntity(18, "1"));
			//ssd.Add(new SpecSheetTemplateDataEntity(19, "1"));
			//ssd.Add(new SpecSheetTemplateDataEntity(20, "1"));

			//foreach (SpecSheetTemplateDataEntity p in ssd)
			//{
			//	Factories.CreateEntity().Save(p);
			//}


			List<ParameterEntity> pp = new List<ParameterEntity>();

			pp.Add(new ParameterEntity(0, "自動インクリメント", 1));
			pp.Add(new ParameterEntity(0, "計算式", 1));
			pp.Add(new ParameterEntity(-1, "架線電圧", 0));
			pp.Add(new ParameterEntity(-1, "伝送方式", 0));
			pp.Add(new ParameterEntity(-1, "P／U【CONV】", 0));
			pp.Add(new ParameterEntity(-1, "P／U【CONV】台数", 0));
			pp.Add(new ParameterEntity(-1, "P／U【INV】", 0));
			pp.Add(new ParameterEntity(-1, "P／U【INV】台数", 0));
			pp.Add(new ParameterEntity(1, "検出器名称", 0));
			pp.Add(new ParameterEntity(1, "記号", 0));
			pp.Add(new ParameterEntity(1, "定格電圧", 0));

			foreach (ParameterEntity p in pp)
			{
				SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(p);
			}

			List<InspectionEntity> pi = new List<InspectionEntity>
			{
				new InspectionEntity(999,"","寒冷地試験",""),
				new InspectionEntity(3,"4.5.3.1","目視検査","Visual inspection"),
				new InspectionEntity(3,"4.5.3.2","寸法及び許容値検査","Verification of dimensions and tolerances "),
				new InspectionEntity(3,"4.5.3.3","質量測定","Weighting"),
				new InspectionEntity(3,"4.5.3.4","表示の検査","Marking inspection"),
				new InspectionEntity(3,"4.5.3.5","冷却システムの性能検査","Cooling system performance tests"),
				new InspectionEntity(3,"4.5.3.5.4","リークテスト","Leakage test"),
				new InspectionEntity(3,"4.5.3.6","散水試験","Test of the degree of protection "),
				new InspectionEntity(3,"4.5.3.7","耐電圧試験","Dielectric test"),
				new InspectionEntity(3,"4.5.3.8","絶縁抵抗","Insulation resistance test"),
				new InspectionEntity(3,"4.5.3.9","検出器入出力特性試験","Tests of mechanical and electrical protection and measuring equipment "),
				new InspectionEntity(3,"4.5.3.10","軽負荷試験","Light load test"),
				new InspectionEntity(3,"4.5.3.12","保護動作試験","Acoustic noise measurement"),
				new InspectionEntity(3,"4.5.3.14","積算電力機能確認試験","Power loss determination"),
				new InspectionEntity(3,"4.5.3.17","放電時間測定","Safety requirements inspection"),
				new InspectionEntity(3,"4.5.3.18","シーケンス試験","Tests for withstanding vibration and shock"),
				new InspectionEntity(3,"4.5.3.19","パワーリミッタ動作","Test of electromagnetic compatibility"),
				new InspectionEntity(4,"7.2","電動機の熱時トルク特性試験","Torque characteristics test at motor hot "),
				new InspectionEntity(4,"7.3","電動機の冷時トルク特性試験"," Torque characteristics test at motor cold "),
				new InspectionEntity(4,"8.2","効率特性試験","Efficiency characteristics "),
				new InspectionEntity(4,"10.1","後退起動試験","Start from backward-reverse motion "),
				new InspectionEntity(4,"10.2","力行状態とブレーキ状態との移行","Motoring-braking transition "),
				new InspectionEntity(4,"11","電車線電圧の変化","Variation of line voltage "),
				new InspectionEntity(4,"12.2","瞬時電圧変化試験(架線急変試験)","Rapid voltage changes test "),
				new InspectionEntity(4,"12.3","電車線の一時停電試験","Traction supply voltage interruption "),
				new InspectionEntity(4,"12.4","電車線の瞬時停電試験(離線試験)","Traction supply contact loss "),
				new InspectionEntity(4,"12.5","回生負荷の遮断試験","Sudden loss of regeneration capability ")

			};
			foreach (InspectionEntity p in pi)
			{
				SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(p);
			}

			List<StandardEntity> st = new List<StandardEntity>
			{
				new StandardEntity() {StandardType = "特殊試験", Numbering = "---"},
				new StandardEntity() {StandardType = "IEC", Numbering = "60571"},
				new StandardEntity() {StandardType = "IEC", Numbering = "61287-1" },
				new StandardEntity() {StandardType = "IEC", Numbering = "61377" },
				new StandardEntity() {StandardType = "EN", Numbering = "50155" },
				new StandardEntity() {StandardType = "EN", Numbering = "50121-3-2" },
				new StandardEntity() {StandardType = "伊ｼｷ", Numbering = "6321" },
				new StandardEntity() {StandardType = "伊ｼｷ", Numbering = "6321-T" }
			};
			foreach (StandardEntity p in st)
			{
				SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(p);
			}
		}
	}
}
