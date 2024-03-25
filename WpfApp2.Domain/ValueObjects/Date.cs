using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Domain.ValueObjects
{
	public sealed class Date : ValueObject<Date>
	{
		public  DateTime Value { get; set; }= DateTime.MinValue;

		protected override bool EqualsCore(Date other)
		{
			return this.Value == other.Value;
		}

		public string DisplayValue
		{
			get
			{
				if (Value == DateTime.MinValue) return "----";
				return string.Format("{0:yy/MM/dd HH:mm}", Value);
			}
		}

		public Date() { }
		public Date(DateTime date)
		{
			Value = date;
		}
	}
}
