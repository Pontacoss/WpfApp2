using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Domain.Entities;
using WpfApp2.Domain.InterFaces;

namespace WpfApp2.Domain.Entities
{
	public sealed class TextAreaEntity : IAreaEntity
	{
		public int Id { get; set; }

		[StringLength(500)]
		public string Value { get; set; } = string.Empty;

		[StringLength(1000)]
		public string ValueEN { get; set; } = string.Empty;
		public int arg0 { get; set; } = -1;
		public int arg1 { get; set; } = -1;
		public int arg2 { get; set; } = -1;
		public int arg3 { get; set; } = -1;
		public int arg4 { get; set; } = -1;
		public int arg5 { get; set; } = -1;
		public int arg6 { get; set; } = -1;
		public int arg7 { get; set; } = -1;
		public int arg8 { get; set; } = -1;
		public int arg9 { get; set; } = -1;

		public TextAreaEntity()	{	}

		public TextAreaEntity(TextAreaEntity BaseEntity,Dictionary<int,int> OldNewParamIdPairs)
		{
			Value = BaseEntity.Value;
			ValueEN = BaseEntity.ValueEN;

			Set_Args(BaseEntity.Get_Args().Select(x=> OldNewParamIdPairs[x]).ToArray());
		}

		public TextAreaEntity(int _id, string _value, string _valueEN, int[] args)
		{
			Id = _id;
			Value = _value;
			ValueEN = _valueEN;
			int index = 0;
			arg0 = args[index++];
			arg1 = args[index++];
			arg2 = args[index++];
			arg3 = args[index++];
			arg4 = args[index++];
			arg5 = args[index++];
			arg6 = args[index++];
			arg7 = args[index++];
			arg8 = args[index++];
			arg9 = args[index++];
		}

		public int[] Get_Args()
		{
			int[] args = new int[10];
			int index = 0;
			args[index++] = arg0;
			args[index++] = arg1;
			args[index++] = arg2;
			args[index++] = arg3;
			args[index++] = arg4;
			args[index++] = arg5;
			args[index++] = arg6;
			args[index++] = arg7;
			args[index++] = arg8;
			args[index++] = arg9;

			return args;
		}

		public void Set_Args(int[] args)
		{
			int index = 0;
			arg0 = args[index++];
			arg1 = args[index++];
			arg2 = args[index++];
			arg3 = args[index++];
			arg4 = args[index++];
			arg5 = args[index++];
			arg6 = args[index++];
			arg7 = args[index++];
			arg8 = args[index++];
			arg9 = args[index++];
		}

		public string ExtractString(bool language)
		{
			string str;
			if (language) str = Value;
			else str = ValueEN;

			int[] args = Get_Args();
			for (int i= 0;i < 10;i++)
			{
				if (args[i] > 0) str=str.Replace("{" + i + "}", "【" + args[i] + "】");
			}
			return str;
		}
	}
}
