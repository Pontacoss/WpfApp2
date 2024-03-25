using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Domain.ValueObjects
{
	public sealed class SeriesNo : ValueObject<SeriesNo>
	{
		public int Value { get; }

		public SeriesNo(int value)
		{
			Value = value;
		}

		protected override bool EqualsCore(SeriesNo other)
		{
			return this.Value == other.Value;
		}
	}
}
