using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Domain.ValueObjects
{
	public sealed class AreaType : ValueObject<AreaType>
	{
		public static readonly AreaType None = new AreaType(0);
		public static readonly AreaType TextArea = new AreaType(1);
		public static readonly AreaType TableArea = new AreaType(2);
		public static readonly AreaType FigureArea = new AreaType(3);

		public int Value { get; }
		public string Name { get; set; } = string.Empty;

		protected override bool EqualsCore(AreaType other)
		{
			return this.Value == other.Value;
		}
		public AreaType() { }

		public AreaType(int value)
		{
			Value = value;
			Name = DisplayValue;
		}

		public AreaType(string value)
		{
			if (value == "文章エリア")
				Value = 1;
			else if (value == "表エリア")
				Value = 2;
			else if (value == "図エリア")
				Value = 3;
			else
				Value = 0;
		}

		public string DisplayValue
		{
			get
			{
				if (Name != string.Empty)
					return Name;
				if (this == TextArea)
					return "文章エリア";
				if (this == TableArea)
					return "表エリア";
				if (this == FigureArea)
					return "図エリア";
				return "文書構成";
			}
		}

	}
}
