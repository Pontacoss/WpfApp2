using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.WPFForm2.ViewModels
{
	public abstract class ViewModelBase: BindableBase
	{
		private string _viewModelName;
		private static int constructorCounter = 0;
		private int seq = 0;

		public ViewModelBase(string viewModelName)
		{
#if DEBUG
			_viewModelName = viewModelName;
			constructorCounter++;
			seq = constructorCounter;
			Console.WriteLine(_viewModelName + " Constructor " + constructorCounter);
#endif
		}
		

		~ViewModelBase()
		{
#if DEBUG
			Console.WriteLine(_viewModelName+" Destructor " + seq);
#endif
		}
	}
}
