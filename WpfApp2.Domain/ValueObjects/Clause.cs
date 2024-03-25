using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WpfApp2.Domain.Exceptions;

namespace WpfApp2.Domain.ValueObjects
{
	public sealed class Clause : ValueObject<Clause>, IComparable<Clause>
	{
		public  string Value { get; } = string.Empty;

		public int CompareTo(Clause other)
		{
			var c1 = this.Value.Split('.');
			var c2 = other.Value.Split('.');

			var len = c1.Length < c2.Length ? c1.Length : c2.Length;
			for (int i = 0; i < len; i++)
			{
				if (int.Parse(c1[i]) < int.Parse(c2[i])) return -1;
				if (int.Parse(c1[i]) > int.Parse(c2[i])) return 1;
			}
			if (c1.Length == c2.Length) return 0;
			return c1.Length < c2.Length ? -1 : 1;
		}

		protected override bool EqualsCore(Clause other)
		{
			return this.Value == other.Value;
		}

		public Clause(string value)
		{
			if (IrregularCheck(value))	throw new ClauseValueCheckException();
			Value = value;
		}

		public string DisplayValue { get => Value; }


		private bool IrregularCheck(string value)
		{
			if( Regex.IsMatch(value, "[^0-9^.]")) return true;
			if(value.Count(x => x == '.')>Shared.ClauseLevelMax) return true;
			return false;
		}
	}
}
