using SQLWriter;
using System;
using System.Linq;
using WpfApp2.Domain.Entities;

using WpfApp2.Domain.ValueObjects;

namespace WpfApp2.WPFForm2.Helpers
{
	internal static class AreaEntityDataHelper
	{
		internal static T GetAreaData<T>(CompositionEntity ce) where T : class
		{
			if (ce.AreaType == AreaType.TextArea)
			{
				var f = new SQLFilter(nameof(TextAreaEntity.Id), ce.ContentId);
				return SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TextAreaEntity>(f).Single() as T; 
			}
			else if (ce.AreaType == AreaType.TableArea)
			{
				var f = new SQLFilter(nameof(TableAreaEntity.Id), ce.ContentId);
				return SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TableAreaEntity>(f).Single() as T;
			}
			else if (ce.AreaType == AreaType.FigureArea)
			{
				var f = new SQLFilter(nameof(FigureAreaEntity.Id), ce.ContentId);
				return SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<FigureAreaEntity>(f).Single() as T;
			}
			else
				throw new NotImplementedException();
		}
	}
}
