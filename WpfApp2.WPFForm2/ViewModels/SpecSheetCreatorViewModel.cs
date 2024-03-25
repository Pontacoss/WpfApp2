using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using WpfApp2.WPFForm2.Views;

namespace WpfApp2.WPFForm2.ViewModels
{
	public class SpecSheetCreatorViewModel : BindableBase,INavigationAware
	{
		private IRegionManager _regionManager;
		public SpecSheetCreatorViewModel(IRegionManager regionManager)
		{
			_regionManager = regionManager;
			_regionManager.RegisterViewWithRegion("SpecSheetRegion", nameof(SearchDevelopmentOrderView));

		}

		public bool IsNavigationTarget(NavigationContext navigationContext)	=> false;

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{
		}

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
		}
	}
}
