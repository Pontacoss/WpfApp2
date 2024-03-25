using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Domain.Exceptions
{
	internal class ClauseValueCheckException : ExceptionBase
	{
		public override ExceptionKind Kind => ExceptionKind.Warning;

		public ClauseValueCheckException() : base("入力が不正です。" + Environment.NewLine +
			"使用できるのは数値と .(ピリオド)だけです。" + Environment.NewLine +
			"また、使用できるピリオドの数は"+Shared.ClauseLevelMax+"個までです。")	{}

	}
}
