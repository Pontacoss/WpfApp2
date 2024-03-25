using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp2.Domain.Entities;

namespace WpfApp2.Domain.ValueObjects
{
	public sealed class Status : ValueObject<Status>
	{
		public static readonly Status Editing = new Status(0);
		public static readonly Status SendBackByChecker = new Status(1);
		public static readonly Status SendBackByApprover = new Status(2);
		public static readonly Status Request = new Status(3);
		public static readonly Status Checked = new Status(4);
		public static readonly Status Approved = new Status(5);

		public Status(int value)
		{
			Value = value;
		}

		public  int Value { get; }

		public string DisplayValue
		{
			get
			{
				if(this == Editing) return "編集";
				if (this == SendBackByChecker) return "照査差戻";
				if (this == SendBackByApprover) return "検認差戻";
				if (this == Request) return "依頼";
				if (this == Checked) return "照査";
				return "承認";
			}
		}

		public static string StatusAdvance(TemplateEntity te, string comment)
		{
			if (te.Status  == Editing || te.Status == SendBackByChecker || 
				te.Status == SendBackByApprover) te.Status = Request;

			else if (te.Status == Request) te.Status = Checked;
			else if (te.Status == Checked) te.Status = Approved;

			return "Rev." + te.Revision.DisplayValue + "(" 
				+ te.Status.DisplayValue + "):  " 
				+ comment + "\n(" + DateTime.Now + ")\n";
		}

		public static string StatusRegression(TemplateEntity te, string comment)
		{
			if (te.Status == Request) te.Status = SendBackByChecker;
			else if (te.Status == Checked) te.Status = SendBackByApprover;

			return "Rev." + te.Revision.DisplayValue + "(" 
				+ te.Status.DisplayValue + "):  "
				+ comment + "\n(" + DateTime.Now + ")\n";
		}

		protected override bool EqualsCore(Status other)
		{
			return this.Value == other.Value;
		}

		public static bool CanSelect(Status status)
		{
			if (status == Checked)
			{
				MessageBox.Show("照査済みデータは選択できません。");
				return false;
			}
			else if (status == Request)
			{
				var result = MessageBox.Show("照査依頼済みですが、" +
					"データを取り戻しますか？", "", MessageBoxButton.OKCancel);
				if (result == MessageBoxResult.Cancel) return false;
			}
			else if (status == Approved)
			{
				var result = MessageBox.Show("承認済みデータを使って新しい" +
					"テンプレートを作成しますか？", "", MessageBoxButton.OKCancel);
				if (result == MessageBoxResult.Cancel) return false;
			}
			return true;
		}
	}
}
