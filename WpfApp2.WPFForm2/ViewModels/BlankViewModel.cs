using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfApp2.WPFForm2.ViewModels
{
	[RegionMemberLifetime(KeepAlive = false)]
	public class BlankViewModel : BindableBase, INavigationAware
	{
		
		public BlankViewModel()
		{

#if DEBUG
			constructorCounter++;
			seq = constructorCounter;
			Console.WriteLine("BlankViewModel Constructor" + constructorCounter);
#endif

		}

		private static int constructorCounter = 0;
		private int seq = 0;

		~BlankViewModel()
		{
#if DEBUG
			Console.WriteLine("BlankViewModel Destructor" + seq);
#endif
		}

		public bool IsNavigationTarget(NavigationContext navigationContext) => false;
		
		public void OnNavigatedFrom(NavigationContext navigationContext)
		{
		}

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
		}
	}
}
