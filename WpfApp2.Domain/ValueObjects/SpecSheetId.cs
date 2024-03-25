using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Domain.Entities;

namespace WpfApp2.Domain.ValueObjects
{
	public sealed class SpecSheetName : ValueObject<SpecSheetName>
	{
		public string Value { get; } = string.Empty;

		public SpecSheetName(string value)
		{
			Value = value;
		}

		protected override bool EqualsCore(SpecSheetName other)
		{
			return this.Value == other.Value;
		}

		// ToDo 仕様書IDの要否も含め、採番方法の修正
		public static string GetNewSpecSheetName(ModelEntity me, List<SpecSheetBaseEntity> list)
		{
			if (list.Any(x => x.ModelId == me.Id))
			{
				return list.First(x => x.ModelId == me.Id).SpecSheetName.Value;
			}
			else
			{
				if (list.Count > 0)
				{
					var last = list.OrderBy(x => x.SpecSheetName.Value).Last();
					var lastNumber = Convert.ToInt32(last.SpecSheetName.Value.Replace("AAA", ""));
					return "AAA" + String.Format("{0:000}", lastNumber + 1);
				}
				return "AAA001";
			}
		}
	}
}
