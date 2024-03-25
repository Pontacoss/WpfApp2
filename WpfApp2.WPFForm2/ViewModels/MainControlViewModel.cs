using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using SQLWriter;
using System;
using System.Windows;
using WpfApp2.WPFForm2.Helpers;
using WpfApp2.WPFForm2.Views;

namespace WpfApp2.WPFForm2.ViewModels
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class MainControlViewModel : BindableBase, INavigationAware
    {
        private IRegionManager _regionManager;
        public MainControlViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            CreateTemplateButtonClick = new DelegateCommand(CreateTemplateButtonClickExecute);
            StartUp_SQLWriter_Click = new DelegateCommand(StartUp_SQLWriter_ClickExecute);
            ResetButton_Click = new DelegateCommand(ResetButton_ClickExecute);
            StartUp_DBMaster_Click = new DelegateCommand(StartUp_DBMaster_ClickExecute);
            ReviewAndApproval_Click = new DelegateCommand(ReviewAndApproval_ClickExecute);
            CreateSpecSheetButtonClick = new DelegateCommand(CreateSpecSheetButtonClickExecute);
            SQLWriterResetButton_Click = new DelegateCommand(SQLWriterResetButton_ClickExecute);

#if DEBUG
            constructorCounter++;
            Console.WriteLine("MainControlViewModel Constructor" + constructorCounter);
#endif

        }

        private static int constructorCounter = 0;
        private static int destructorCounter = 0;

        ~MainControlViewModel()
        {
#if DEBUG
            destructorCounter++;
            Console.WriteLine("MainControlViewModel Destructor" + destructorCounter);
#endif
        }
        public DelegateCommand SQLWriterResetButton_Click { get; }
        private void SQLWriterResetButton_ClickExecute()
        {
            SQLWriterFacade.InitializeInnerDB();
        }
        public DelegateCommand CreateSpecSheetButtonClick { get; }
        private void CreateSpecSheetButtonClickExecute()
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(SearchDevelopmentOrderView));
        }

        public DelegateCommand ReviewAndApproval_Click { get; }
        private void ReviewAndApproval_ClickExecute()
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(ReviewAndApproveView));
        }

        public DelegateCommand StartUp_SQLWriter_Click { get; }
        private void StartUp_SQLWriter_ClickExecute()
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(SQLWriterTableSettingView));
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
            TestMockHelper.Initialize();
        }

        public DelegateCommand CreateTemplateButtonClick { get; }
        private void CreateTemplateButtonClickExecute()
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(TemplateSelectView));
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;


        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }


    }
}
