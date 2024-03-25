using SQLWriter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Domain.Entities;
using WpfApp2.Domain.InterFaces;
using WpfApp2.Domain.ValueObjects;

namespace WpfApp2.WPFForm2.Helpers
{
	public class CreateNextRevTemplateHelper
	{
		public static TemplateEntity Execute(TemplateEntity baseTemplate, Revision maxRev)
		{
			var newTemplate = new TemplateEntity(baseTemplate, maxRev);
			SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Insert(newTemplate);

			SQLFilter p = new SQLFilter(nameof(ParameterEntity.ParentId), baseTemplate.Id);
			var parameters = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<ParameterEntity>(p);

			var OldNewParamIdPairs = new Dictionary<int, int>();
			OldNewParamIdPairs.Add(-1, -1);

			SQLFilter p1 = new SQLFilter(nameof(ParameterEntity.ParentId), 0, "<=");
			var commonParameter = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<ParameterEntity>(p1);
			foreach (var param in commonParameter)
			{
				OldNewParamIdPairs.Add(param.Id, param.Id);
			}

			foreach (var param in parameters)
			{
				var pe = new ParameterEntity(param, newTemplate.Id);
				SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(pe);
				OldNewParamIdPairs.Add(param.Id, pe.Id);
			}

			var comps = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<CompositionEntity>(
				new SQLFilter(nameof(CompositionEntity.ParentId), baseTemplate.Id));


			var OldNewCompIdPairs = new Dictionary<int, int>();
			foreach (var comp in comps)
			{
				var newComp = new CompositionEntity(comp, newTemplate.Id, OldNewParamIdPairs, OldNewCompIdPairs);
				IAreaEntity newEntity;
				if (comp.AreaType==AreaType.TextArea)
				{
					newEntity = new TextAreaEntity(SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TextAreaEntity>(
						new SQLFilter(nameof(TextAreaEntity.Id),comp.ContentId)).Single(), OldNewParamIdPairs);
				}
				else if (comp.AreaType == AreaType.TableArea)
				{
					newEntity = new TableAreaEntity(SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TableAreaEntity>(
						new SQLFilter(nameof(TableAreaEntity.Id), comp.ContentId)).Single(), OldNewParamIdPairs);
				}
				else if (comp.AreaType == AreaType.FigureArea)
				{
					newEntity = new FigureAreaEntity(SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<FigureAreaEntity>(
						new SQLFilter(nameof(FigureAreaEntity.Id), comp.ContentId)).Single(), OldNewParamIdPairs);
				}
				else throw new NotImplementedException();

				newComp.ContentId = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(newEntity);
				SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(newComp);
				OldNewCompIdPairs.Add(comp.Id, newComp.Id);
			}
			return newTemplate;
		}
	}
}
