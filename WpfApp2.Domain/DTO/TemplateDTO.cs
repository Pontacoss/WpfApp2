using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Domain.Entities;

namespace WpfApp2.Domain.DTO
{
	public sealed class TemplateDTO :INotifyPropertyChanged
	{
		public string NameJP { get; }
		public string NameEN { get; }
		public bool TypeTest { get; set; }
		public bool RoutinTest { get; set; }
		public string Clause { get; }
		public string Status { get; set; }
		public TemplateEntity Template { get; }

		public int ParentId { get; }
		//public string Sorter { get; }

		private string _revision;
		public string Revision
		{
			get => _revision;
			set { RaisePropertyChanged(ref _revision, value); }
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private bool RaisePropertyChanged<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
		{
			if (Equals(field, value)) return false;
			field = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			return true;
		}

		public TemplateDTO(TemplateEntity te,InspectionEntity ie)
		{
			NameJP=ie.NameJP.Value;
			NameEN=ie.NameEN.Value;
			TypeTest=ie.TypeTest;
			RoutinTest=ie.RoutinTest;
			Clause=ie.Clause.Value;
			Status=te.Status.DisplayValue;
			Template = te;
			ParentId = ie.ParentId;
			Revision = te.Revision.Value;
		}

		
	}
}
