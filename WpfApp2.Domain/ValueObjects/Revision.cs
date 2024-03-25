using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Domain.ValueObjects
{
	public sealed class Revision : ValueObject<Revision>, IComparable<Revision>
	{
		public  string Value { get; }

		public Revision(string value)
		{
			Value = value;
		}

		public int CompareTo(Revision other)
		{
			if (Value.Length > other.Value.Length) return 1;
			if (Value.Length < other.Value.Length) return -1;
			return Value.CompareTo(other.Value);
		}

		//public static bool operator < (Revision x, Revision y)
		//{
		//	if(x.Value.Length!=y.Value.Length)
		//		return x.Value.Length < y.Value.Length;
		//	return x.Value.CompareTo(y.Value) < 0;
		//}

		//public static bool operator <= (Revision x, Revision y)
		//{
		//	if (x.Value.Length != y.Value.Length)
		//		return x.Value.Length < y.Value.Length;
		//	return x.Value.CompareTo(y.Value) <= 0;
		//}

		//public static bool operator > (Revision x, Revision y)
		//{
		//	if (x.Value.Length != y.Value.Length)
		//		return x.Value.Length > y.Value.Length;
		//	return x.Value.CompareTo(y.Value) > 0;
		//}

		//public static bool operator >= (Revision x, Revision y)
		//{
		//	if (x.Value.Length != y.Value.Length)
		//		return x.Value.Length > y.Value.Length;
		//	return x.Value.CompareTo(y.Value) >= 0;
		//}

		public string DisplayValue { get => Value; }

		protected override bool EqualsCore(Revision other)
		{
			return this.Value == other.Value;
		}

		public static string GetNextRev(string revision)
		{
			if (revision == "ZZ") new NotImplementedException();
			if (revision == string.Empty) return "-";
			if (revision == "-") return "A";
			if (revision == "Z") return "AA";

			var c = revision.ToCharArray();
			if (c.Length == 1) return (++c[0]).ToString();
			else
			{
				if (c[1] == 'Z') return (++c[0]).ToString() + "A";
				else return c[0].ToString() + (++c[1]).ToString();
			}
		}

		public static Revision GetNextRev(Revision revision)
		{
			return new Revision(GetNextRev(revision.Value));
		}
	}
}
