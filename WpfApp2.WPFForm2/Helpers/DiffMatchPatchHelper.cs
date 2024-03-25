using SQLWriter;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using WpfApp2.Domain.Entities;
using WpfApp2.Domain.InterFaces;
using WpfApp2.Domain.Services.DiffMatchPatch;

using WpfApp2.Domain.ValueObjects;

namespace WpfApp2.WPFForm2.Helpers
{
	public class DiffMatchPatchHelper
	{
		public static WebBrowser ExtractDifference(TemplateEntity newTemp, bool language)
		{
			WebBrowser diff = new WebBrowser();
			TemplateEntity oldTemp = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TemplateEntity>(
				new SQLFilter(nameof(TemplateEntity.Id), newTemp.BaseTemplateId)).Single();

			var compositions = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite)
				.Load<CompositionEntity>(new SQLFilter(nameof(CompositionEntity.ParentId), newTemp.Id))
				.OrderBy(x => x.Level).ThenBy(x => x.Sequence).ToList();
			var sequence1 = PreviewHelper.GetSequence(compositions);
			var sb1 = new StringBuilder();

			var ie = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<InspectionEntity>(
				new SQLFilter(nameof(InspectionEntity.Id), newTemp.ParentId)).Single(); ;
			sb1.Append(Name.GetName(language, ie.NameJP, ie.NameEN));
			sb1.Append("\n\n");

			foreach (var comp in sequence1)
			{
				sb1.Append(GetStringFromComposition(comp.Composition, language));
				sb1.Append("\n\n");
			}

			compositions = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite)
				.Load<CompositionEntity>(new SQLFilter(nameof(CompositionEntity.ParentId), oldTemp.Id))
				.OrderBy(x => x.Level).ThenBy(x => x.Sequence).ToList();

			var sequence2 = PreviewHelper.GetSequence(compositions);
			var sb2 = new StringBuilder();

			ie = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<InspectionEntity>(
				new SQLFilter(nameof(InspectionEntity.Id),oldTemp.ParentId)).Single();
			sb2.Append(Name.GetName(language, ie.NameJP, ie.NameEN));
			sb2.Append("\n\n");


			foreach (var comp in sequence2)
			{
				sb2.Append(GetStringFromComposition(comp.Composition, language));
				sb1.Append("\n\n");
			}

			diff.NavigateToString(DiffMatchPatchService.Execute(sb2.ToString(), sb1.ToString()));

			return diff;
		}

		public static string GetStringFromComposition(CompositionEntity ce,bool language)
		{
			IAreaEntity ae = AreaEntityDataHelper.GetAreaData<IAreaEntity>(ce);
			var sb = ae.ExtractString(language);
			MatchCollection matches = Regex.Matches(sb, "【[0-9]+】"); 
			foreach(Match match in matches)
			{
				int id = Convert.ToInt32(Regex.Match(match.ToString(), "[0-9]+").ToString());
				var pe = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<ParameterEntity>(
					new SQLFilter(nameof(ParameterEntity.Id),id)).Single();
			
				string name = ParamName.GetName(language, pe.NameJP, pe.NameEN);

				sb = sb.Replace(match.ToString(), "【" + name + "】");
			}
			return sb;
		}
	}
}
