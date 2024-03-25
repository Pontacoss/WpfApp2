using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using WpfApp2.WPFForm2.Views;
using System.Windows;
using Prism.Regions;
using WpfApp2.Infrastructure;
using System;
using WpfApp2.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace WpfApp2.WPFForm2.ViewModels
{
	[RegionMemberLifetime(KeepAlive = false)]
	public class MainViewModel : BindableBase
	{
		private IDialogService _dialogService;
		private IRegionManager _regionManager;

		private bool _menuToggleButtonCheck = false;
		public bool MenuToggleButtonCheck
		{
			get { return _menuToggleButtonCheck; }
			set { SetProperty(ref _menuToggleButtonCheck, value); }
		}

		public MainViewModel(IDialogService dialogService,IRegionManager regionManager)
		{
			_dialogService=dialogService;
			_regionManager=regionManager;
			_regionManager.RegisterViewWithRegion("ContentRegion", nameof(MainControlView));

			GoHomeButton_Click = new DelegateCommand(GoHomeButton_ClickExecute);
			CreateTemplateButtonClick = new DelegateCommand(CreateTemplateButtonClickExecute);
			StartUp_SQLWriter_Click = new DelegateCommand(StartUp_SQLWriter_ClickExecute);
			ResetButton_Click = new DelegateCommand(ResetButton_ClickExecute);
			StartUp_DBMaster_Click = new DelegateCommand(StartUp_DBMaster_ClickExecute);
			ReviewAndApproval_Click = new DelegateCommand<string>(ReviewAndApproval_ClickExecute);
			CreateSpecSheetButtonClick=new DelegateCommand(CreateSpecSheetButtonClickExecute);
#if DEBUG
			constructorCounter++;
			Console.WriteLine("MainViewModel Constructor" + constructorCounter);
#endif
			
		}

		private static int constructorCounter = 0;
		private static int destructorCounter = 0;

		~MainViewModel()
		{
#if DEBUG
			destructorCounter++;
			Console.WriteLine("MainViewModel Destructor" + destructorCounter);
#endif
		}

		public DelegateCommand CreateSpecSheetButtonClick { get; }
		private void CreateSpecSheetButtonClickExecute()
		{
			MenuToggleButtonCheck = false;
			_regionManager.RequestNavigate("ContentRegion", nameof(SearchDevelopmentOrderView));
		}

		public DelegateCommand<string> ReviewAndApproval_Click { get; }
		private void ReviewAndApproval_ClickExecute(string authority)
		{
			MenuToggleButtonCheck = false;
			var p = new NavigationParameters();
			p.Add(nameof(ReviewAndApproveViewModel.Authority), authority);
			_regionManager.RequestNavigate("ContentRegion", nameof(ReviewAndApproveView),p);
		}

		public DelegateCommand StartUp_SQLWriter_Click { get; }
		private void StartUp_SQLWriter_ClickExecute()
		{
			_regionManager.RequestNavigate("ContentRegion", nameof(SQLWriterColumnView));
		}

		public DelegateCommand StartUp_DBMaster_Click { get; }
		private void StartUp_DBMaster_ClickExecute()
		{
			_regionManager.RequestNavigate("ContentRegion", nameof(DBControllerView));
		}

		public DelegateCommand ResetButton_Click { get; }
		private void ResetButton_ClickExecute()
		{
			var ans = MessageBox.Show("データベースに加えた変更を元に戻します。" +
				"よろしいですか？", "データベースの初期化", MessageBoxButton.OKCancel);
			if (ans == MessageBoxResult.Cancel) return;

			//TestMock.Initialize();
		}

		public DelegateCommand CreateTemplateButtonClick { get; }
		private void CreateTemplateButtonClickExecute()
		{
			MenuToggleButtonCheck=false;
			_regionManager.RequestNavigate("ContentRegion", nameof(TemplateSelectView));
		}

		public DelegateCommand GoHomeButton_Click { get; }
		private void GoHomeButton_ClickExecute()
		{
			MenuToggleButtonCheck = false;
			_regionManager.RequestNavigate("ContentRegion", nameof(MainControlView));
		}

	}
}
