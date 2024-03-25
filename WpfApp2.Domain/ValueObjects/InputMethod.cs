using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Domain.ValueObjects
{
	public sealed class InputMethod : ValueObject<InputMethod>
	{
		public static readonly InputMethod Manual = new InputMethod(0);
		public static readonly InputMethod Calc = new InputMethod(1);
		public static readonly InputMethod Select = new InputMethod(2);
		public static readonly InputMethod Reference = new InputMethod(3);

		public InputMethod(int value)
		{
			Value = value;
		}

		public  int Value { get; }

		public string DisplayValue
		{
			get
			{
				if (this == Manual)	return "入力";
				else if (this == Calc) return "計算";
				else if (this == Select) return "選択";
				else return "参照";
			}
		}

		protected override bool EqualsCore(InputMethod other)
		{
			return this.Value == other.Value;
		}

		public static InputMethod ChangeMethod(InputMethod im)
		{
			if (im == InputMethod.Manual) return InputMethod.Calc;
			else if (im == InputMethod.Calc) return InputMethod.Manual;
			return im;
		}
	}
}
