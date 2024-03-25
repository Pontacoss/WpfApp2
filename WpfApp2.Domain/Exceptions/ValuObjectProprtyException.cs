using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Domain.Exceptions
{
	public sealed class ValuObjectProprtyException : Exception
	{
		public ValuObjectProprtyException(string name) : base(
			name +" にプロパティ Value がありません。")	{}
	}
}
