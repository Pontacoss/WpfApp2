using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Domain.Entities;

namespace WpfApp2.Domain.DTO
{
	public sealed class TextParamDTO : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private bool RaisePropertyChanged<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
		{
			if (Equals(field, value)) return false;

			field = value;

			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			return true;
		}

		public int Id;

		private int _paramId;
		public int ParamId
		{
			get => _paramId;
			set { RaisePropertyChanged(ref _paramId, value); }
		}
		private string _name;
		public string Name
		{
			get => _name;
			set { RaisePropertyChanged(ref _name, value); }
		}
		private string _value;
		public string Value
		{
			get => _value;
			set { RaisePropertyChanged(ref _value, value); }
		}

		public TextParamDTO(int id, string name, int paramId,string value)
		{
			Id = id;
			Name = name;
			ParamId = paramId;
			Value = value;
		}
	}
}
