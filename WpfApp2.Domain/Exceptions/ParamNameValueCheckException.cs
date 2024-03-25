using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Domain.Exceptions
{
	public sealed class ParamNameValueCheckException : ExceptionBase
	{
		public ParamNameValueCheckException(string value)
			: base("禁則文字が含まれています。" + Environment.NewLine
				  + " /(スラッシュ) [ ](ブラケット) .(ピリオド) _(アンダーバー) は使えません。"
				  + Environment.NewLine + "また、名前を空白にすることもできません。"
				  + Environment.NewLine + Environment.NewLine + "　チェック対象　：" + value){}

		public override ExceptionKind Kind => ExceptionKind.Warning;
	}
}
