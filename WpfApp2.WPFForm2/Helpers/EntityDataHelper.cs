using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Domain.Entities;
using WpfApp2.Domain.DTO;
using WpfApp2.Domain.Helpers;
using SQLWriter;

namespace WpfApp2.WPFForm2.Helpers
{
	public class EntityDataHelper
	{
		public static void ParameterEntityListSave(List<ParameterDTO> list,int templateId)
		{
			List<ParameterEntity> peList=new List<ParameterEntity>();
			foreach (ParameterDTO c in list)
			{
				c.ParentId = templateId;
				var pe= new ParameterEntity(c);
				peList.Add(pe);
				SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(pe);
				c.Id = pe.Id;
			}

			var plist = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<ParameterEntity>(
				new SQLFilter(nameof(CompositionEntity.ParentId), templateId));
			IEqualityComparer<ParameterEntity> comparer = new ParameterComparer();
			var slist = plist.Except(peList, comparer);

			foreach (ParameterEntity c in slist)
			{
				SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Delete(c);
			}
		}

		private class ParameterComparer : IEqualityComparer<ParameterEntity>
		{
			public bool Equals(ParameterEntity i_lhs, ParameterEntity i_rhs)
			{
				return i_lhs.Id == i_rhs.Id;
			}

			public int GetHashCode(ParameterEntity i_obj)
			{
				return i_obj.Id.GetHashCode();
			}
		}

		public static void CompositionListSave(List<CompositionDTO> entities)
		{
			var list = CompositionDTOHelper.ConvertFromDtoToTemplateTreeData(entities);
			foreach (CompositionEntity ce in list)
			{
				SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(ce);
			}
		}

		public static void DeleteTemplate(TemplateEntity te)
		{
			var plist = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<ParameterEntity>(
				new SQLFilter(nameof(ParameterEntity.ParentId), te.Id));
			foreach (ParameterEntity p in plist)
			{
				SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Delete(p);
			}

			var clist = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<CompositionEntity>(
				new SQLFilter(nameof(CompositionEntity.ParentId), te.Id));
			foreach (CompositionEntity c in clist)
			{
				SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Delete(c);
			}

			SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Delete(te);
		}

		public static void DeleteInspection(InspectionEntity ie)
		{
			var tList = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TemplateEntity>(
				new SQLFilter(nameof(TemplateEntity.ParentId), ie.Id));

			foreach (TemplateEntity te in tList)
			{
				var plist = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<ParameterEntity>(
					new SQLFilter(nameof(ParameterEntity.ParentId), te.Id));
				foreach (ParameterEntity p in plist)
				{
					SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Delete(p);
				}

				var clist = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<CompositionEntity>(
					new SQLFilter(nameof(CompositionEntity.ParentId), te.Id));
				foreach (CompositionEntity c in clist)
				{
					SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Delete(c);
				}
				SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Delete(te);
			}
			SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Delete(ie);
		}
	} 
}
