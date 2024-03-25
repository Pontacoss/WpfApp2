using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp2.Domain.Exceptions;

namespace WpfApp2.Domain.ValueObjects
{
	public sealed class Name : ValueObject<Name>
	{
		public  string Value { get; }

		public Name(string value)
		{
			Value = value;
		}

		public string DisplayValue { get => Value; }

		protected override bool EqualsCore(Name other)
		{
			return this.Value == other.Value;
		}

		public static string GetName(bool language, Name nameJP, Name nameEN)
		{
			return language ? nameJP.Value : nameEN.Value == string.Empty ? nameJP.Value : nameEN.Value;
		}
	}
}
