using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Domain.Exceptions;

namespace WpfApp2.Domain.ValueObjects
{
	public sealed class ParamName : ValueObject<ParamName>
	{
		public  string Value { get; }

		public ParamName(string value)
		{
			if (ParamNameValueCheck(value))
				throw new ParamNameValueCheckException(value);

			Value = value;
		}

		public string DisplayValue { get => Value; }

		protected override bool EqualsCore(ParamName other)
		{
			return this.Value == other.Value;
		}

		public static string GetName(bool language, ParamName nameJP, ParamName nameEN)
		{
			return language ? nameJP.Value : nameEN.Value == string.Empty ? nameJP.Value : nameEN.Value;
		}

		/// <summary>
		/// DataGridColumnの Header になる可能性のあるstringは禁則文字チェックが必要。
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private static bool ParamNameValueCheck(string value)
		{
			if (value.Contains("/")) return true;
			if (value.Contains("[")) return true;
			if (value.Contains("]")) return true;
			if (value.Contains(".")) return true;
			if (value.Contains("_")) return true;
			if (value == " ") return true;

			return false;
		}
	}
}
