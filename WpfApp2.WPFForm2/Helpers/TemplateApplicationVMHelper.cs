using SQLWriter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Domain.DTO;
using WpfApp2.Domain.Entities;

using WpfApp2.Domain.ValueObjects;

namespace WpfApp2.WPFForm2.Helpers
{
	public static class TemplateApplicationVMHelper
	{
		/// <summary>
		/// 新しくTemplateCombinationEntityを作成しDBに保存する。
		/// </summary>
		/// <param name="list"></param>
		/// <param name="target"></param>
		/// <returns>新しく作成したTemplateCombinationEntityのId</returns>
		public static int CreateNewTemplateCombination(List<InspectionDTO> list, int modelId)
		{
			var rev = UpDateLatestRevision(modelId);
			var tempCombi = new List<int>();
			foreach (var entity in list)
			{
				List<SQLFilter> filters = new List<SQLFilter>();
				filters.Add(new SQLFilter(nameof(TemplateEntity.ParentId), entity.Id));
				filters.Add(new SQLFilter(nameof(TemplateEntity.Revision), entity.SelectedRevision));

				var telist = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TemplateEntity>(filters);

				if (telist.Count != 1) throw new NotImplementedException();
				tempCombi.Add(telist.Single().Id);
			}
			var newCombi = new TemplateCombinationEntity(modelId, rev, tempCombi);
			return SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Insert(newCombi); 
		}

		/// <summary>
		/// 型式ごとに最新Rev.を採番して登録し、DBに保存する。
		/// </summary>
		/// <param name="modelEntityId"></param>
		/// <returns>最新のRevision</returns>
		private static Revision UpDateLatestRevision(int modelEntityId)
		{
			var me = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<ModelEntity>(
						new SQLFilter(nameof(ModelEntity.Id), modelEntityId)).Single(); 
			var nextRev = Revision.GetNextRev(me.LatestRevision);

			me.UpdateLatestRev(nextRev);
			SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(me);

			return nextRev;
		}

		/// <summary>
		/// SpecSheetBaseEntityにTemplateCombinationのIdを登録し、DBに保存する。
		/// </summary>
		/// <param name="targetDevOrder"></param>
		/// <param name="templateConbinationId"></param>
		public static SpecSheetBaseEntity SpecSheetBaseUpdate(ProjectListDTO targetDevOrder,int templateConbinationId)
		{
			//var specSheet = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<SpecSheetBaseEntity>(
			//			new SQLFilter(nameof(SpecSheetBaseEntity.Id), specSheetId)).Single();

			var specSheet = new SpecSheetBaseEntity(targetDevOrder.SpecSheetName,
				targetDevOrder.DevelopmentOrderId,targetDevOrder.ModelId,templateConbinationId);

			//specSheet.TemplateCombinationId = templateConbinationId;
			SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(specSheet);

			return specSheet;
		}

		public static (string,Revision) CombinationCheck(
			List<TemplateCombinationEntity> targetModelTempCombi, 
			List<InspectionDTO> RightGrid)
		{
			foreach(var tc in targetModelTempCombi)
			{
				if (tc.TemplateCombination.Count != RightGrid.Count) continue;

				var flag = true;
				for (int i=0; i < tc.TemplateCombination.Count; i++)
				{
					
					SQLFilter f = new SQLFilter(nameof(TemplateEntity.Id), tc.TemplateCombination[i]);
					var te = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TemplateEntity>(f);

					if (te.Count != 1) throw new NotImplementedException();
					var entity = te.Single();

					if(!RightGrid.Any(x => x.Id == entity.ParentId && x.SelectedRevision == entity.Revision))
					{
						flag = false;
						break;
					}
					
				}
				if(flag) return ("Rev." + tc.Revision.DisplayValue + "と同じ組み合わせです。", tc.Revision);

			}
			return ("組み合わせが変更されました。新たなRev.を採番します。", null);
		}
	}
}
