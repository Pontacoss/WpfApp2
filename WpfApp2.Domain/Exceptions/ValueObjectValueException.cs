using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Domain.Exceptions
{

	public sealed class ValueObjectValueException : ExceptionBase
	{
		// todo : obj=null の場合 何とかできないか？ ValueObject<> にnullが使えなくていいのか？
		public ValueObjectValueException(string className,string paramNam)
			: base("ValueObject<>にnull値が設定されました。" +
							"\n\n クラス名：" + className + "\n パラメータ名 : " + paramNam){ }

		public override ExceptionKind Kind => ExceptionKind.Warning;
	}
}
