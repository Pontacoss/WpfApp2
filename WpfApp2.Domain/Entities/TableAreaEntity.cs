using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Domain.Entities;
using WpfApp2.Domain.InterFaces;

namespace WpfApp2.Domain.Entities
{
	public sealed class TableAreaEntity :  IAreaEntity
	{
		public int Id { get; set; }

		[StringLength(20)]
		public string TitleString { get; set; } =String.Empty;

		[StringLength(40)]
		public string TitleStringEN { get; set; } = String.Empty;
		public bool Direction { get; set; } = true; //行方向：true、列方向：false

		[StringLength(70)]
		public string RowLabels { get; set; } = String.Empty;

		[StringLength(70)]
		public string ColumnLabels { get; set; } = String.Empty;

		[StringLength(70)]
		public string ValueLabels { get; set; } = String.Empty;

		public TableAreaEntity() { }

		public TableAreaEntity(TableAreaEntity BaseEntity, Dictionary<int, int> OldNewParamIdPairs)
		{
			TitleString = BaseEntity.TitleString;
			TitleStringEN = BaseEntity.TitleStringEN;
			Direction = BaseEntity.Direction;

			if (BaseEntity.RowLabels != String.Empty)
			{
				List<string> strings = BaseEntity.RowLabels.Split('\u002C').ToList();
				var ints = strings.Select(x => OldNewParamIdPairs[Convert.ToInt32(x)]).ToArray();
				RowLabels = string.Join(",", ints);
			}
			if (BaseEntity.ColumnLabels != String.Empty)
			{
				List<string> strings = BaseEntity.ColumnLabels.Split('\u002C').ToList();
				var ints = strings.Select(x => OldNewParamIdPairs[Convert.ToInt32(x)]).ToArray();
				ColumnLabels = string.Join(",", ints);
			}
			if (BaseEntity.ValueLabels != String.Empty)
			{
				List<string>  strings = BaseEntity.ValueLabels.Split('\u002C').ToList();
				var ints = strings.Select(x => OldNewParamIdPairs[Convert.ToInt32(x)]).ToArray();
				ValueLabels = string.Join(",", ints);
			}
	
		}

		public TableAreaEntity(int id, string titleString, string titleStringEN,
		bool direction, string rowLabels, string columnLabels, string valueLabels)
		{
			Id = id;
			TitleString = titleString;
			TitleStringEN = titleStringEN;
			Direction = direction;
			RowLabels = rowLabels;
			ColumnLabels = columnLabels;
			ValueLabels = valueLabels;
		}

		public string ExtractString(bool language) //DiffMatch用
		{
			return string.Empty;
		}

		//private string ReplaceLabels(Dictionary<string, string> oldNewParamIdPairs, string labels)
		//{
		//	var sb = new StringBuilder();
		//	var str = labels.Split(',');
		//	foreach (var oldId in str)
		//	{
		//		if (oldNewParamIdPairs.TryGetValue(oldId, out var newId))
		//		{
		//			if (newId == string.Empty) continue;
		//			else sb.Append(newId + ",");
		//		}
		//		else sb.Append(oldId + ",");
		//	}

		//	if (sb.Length > 0) sb.Length--;
		//	return sb.ToString();
		//}


		//public override void Copy(AreaEntityBase parent)//, Dictionary<string, string> oldNewParamIdPairs)
		//{
		//	var ts = parent as TableAreaEntity;
		//	TitleString = ts.TitleString;
		//	TitleStringEN = ts.TitleStringEN;
		//	Direction = ts.Direction;
		//	RowLabels = ts.RowLabels;// ReplaceLabels(oldNewParamIdPairs, ts.RowLabels);
		//	ColumnLabels = ts.ColumnLabels;// ReplaceLabels(oldNewParamIdPairs, ts.ColumnLabels);
		//	ValueLabels = ts.ValueLabels;// ReplaceLabels(oldNewParamIdPairs, ts.ValueLabels);
		//}

		//public override void ReplaceParamId(Dictionary<string, string> oldNewParamIdPairs)
		//{
		//	RowLabels = ReplaceLabels(oldNewParamIdPairs, RowLabels);
		//	ColumnLabels = ReplaceLabels(oldNewParamIdPairs, ColumnLabels);
		//	ValueLabels = ReplaceLabels(oldNewParamIdPairs, ValueLabels);
		//}


		//public override void DeleteParameter(ParameterEntity p)
		//{
		//	Dictionary<string, string> dic = new Dictionary<string, string>();
		//	dic.Add(p.Id.ToString(), string.Empty);
		//	RowLabels = ReplaceLabels(dic, RowLabels);
		//	ColumnLabels = ReplaceLabels(dic, ColumnLabels);
		//	ValueLabels = ReplaceLabels(dic, ValueLabels);

		//}
	}
}
