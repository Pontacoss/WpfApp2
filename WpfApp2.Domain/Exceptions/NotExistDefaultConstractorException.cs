using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Domain.Exceptions
{
	public sealed class NotExistDefaultConstractorException : ExceptionBase
	{
		public override ExceptionKind Kind => ExceptionKind.Warning;

		public NotExistDefaultConstractorException(Type type, string value)
			: base("ValueObject ： " + value + "に " + type.Name + "を引数とするコンストラクターがありません。")
		{ }
	}
}
