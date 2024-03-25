using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using SQLWriter;
using System.Collections.ObjectModel;
using System.Linq;
using WpfApp2.Domain.Entities;


using WpfApp2.Domain.ValueObjects;

using WpfApp2.WPFForm2.Helpers;
using WpfApp2.WPFForm2.Views;

namespace WpfApp2.WPFForm2.ViewModels
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class TemplateSelectViewModel : ViewModelBase, INavigationAware
    {
        private IDialogService _dialogService;
        private IRegionManager _regionManager;

        #region コンストラクター
        public TemplateSelectViewModel(IDialogService dialogService, IRegionManager regionManager)
            : base(nameof(TemplateSelectViewModel))
        {
            _dialogService = dialogService;
            _regionManager = regionManager;

            StandardNames.AddRange(StandardEntity.DistinctNameList
                (SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<StandardEntity>()));

            StandardSelectionChange = new DelegateCommand(StandardSelectionChangeExcute);
            NumberSelectionChange = new DelegateCommand(StandardNumberSelectionChangeExcute);
            ShowDetailButtonClick = new DelegateCommand(ShowDetailButtonClickExcute);
            ModifyInspectionInfo = new DelegateCommand(ModifyInspectionInfoExcute);
            DeleteInspectionInfo = new DelegateCommand(DeleteInspectionInfoExcute);
            AddnewInspectionInfo = new DelegateCommand(AddnewInspectionInfoExcute);
            DataGridMouseDoubleClick = new DelegateCommand(DataGridMouseDoubleClickExcute);
            DataGriSelectionChanged = new DelegateCommand(DataGriSelectionChangedExecute);
            UserControl_Unloaded = new DelegateCommand(UserControl_UnloadedExecute);
            //#if DEBUG
            //			constructorCounter++;
            //			Console.WriteLine("TemplateSelectViewModel Constructor" + constructorCounter);
            //#endif

        }

        //		private static int constructorCounter = 0;
        //		private static int destructorCounter = 0;

        //		~TemplateSelectViewModel()
        //		{
        //#if DEBUG
        //			destructorCounter++;
        //			Console.WriteLine("TemplateSelectViewModel Destructor" + destructorCounter);
        //#endif
        //		}
        #endregion

        #region プロパティ設定
        private bool _isDrawerOpen = true;
        public bool IsDrawerOpen
        {
            get { return _isDrawerOpen; }
            set { SetProperty(ref _isDrawerOpen, value); }
        }

        private ObservableCollection<string> _StandardNames = new ObservableCollection<string>();
        public ObservableCollection<string> StandardNames
        {
            get { return _StandardNames; }
            set { SetProperty(ref _StandardNames, value); }
        }

        private bool _isPopupBoxEnabled = false;
        public bool IsPopupBoxEnabled
        {
            get { return _isPopupBoxEnabled; }
            set { SetProperty(ref _isPopupBoxEnabled, value); }
        }

        private string _selectedName = string.Empty;
        public string SelectedName
        {
            get { return _selectedName; }
            set { SetProperty(ref _selectedName, value); }
        }

        private ObservableCollection<StandardEntity> _standardNumbers = new ObservableCollection<StandardEntity>();
        public ObservableCollection<StandardEntity> StandardNumbers
        {
            get { return _standardNumbers; }
            set { SetProperty(ref _standardNumbers, value); }
        }

        private StandardEntity _selectedNumber = null;
        public StandardEntity SelectedNumber
        {
            get { return _selectedNumber; }
            set { SetProperty(ref _selectedNumber, value); }
        }

        private ObservableCollection<InspectionEntity> _inspectionEntiies = new ObservableCollection<InspectionEntity>();
        public ObservableCollection<InspectionEntity> InspectionEntities
        {
            get { return _inspectionEntiies; }
            set { SetProperty(ref _inspectionEntiies, value); }
        }

        private InspectionEntity _selectedInspection = null;
        public InspectionEntity SelectedInspection
        {
            get { return _selectedInspection; }
            set { SetProperty(ref _selectedInspection, value); }
        }

        private TemplateEntity _selectedRevision = null;
        public TemplateEntity SelectedRevision
        {
            get { return _selectedRevision; }
            set { SetProperty(ref _selectedRevision, value); }
        }
        #endregion

        public DelegateCommand UserControl_Unloaded { get; }
        private void UserControl_UnloadedExecute()
        {
            _regionManager.Regions.Remove("CompositionRegion");
        }

        public DelegateCommand StandardSelectionChange { get; }
        private void StandardSelectionChangeExcute()
        {
            StandardNumbers.Clear();
            var f = new SQLFilter(nameof(StandardEntity.StandardType), SelectedName);
            StandardNumbers.AddRange((SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<StandardEntity>(f)));
        }

        public DelegateCommand NumberSelectionChange { get; }
        private void StandardNumberSelectionChangeExcute()
        {
            InspectionEntities.Clear();
            if (SelectedNumber == null) return;

            var f = new SQLFilter(nameof(InspectionEntity.ParentId), SelectedNumber.Id);
            var list = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<InspectionEntity>(f)
                .OrderBy(x => x.Clause).ToList();

            list.ForEach(x => x.GetLatestRevision(SQLWriterFactories.CreateEntity(EnumRdbms.SQLite)
                .Load<TemplateEntity>(new SQLFilter(nameof(TemplateEntity.ParentId), x.Id))));

            InspectionEntities.AddRange(list);
        }

        public DelegateCommand DataGriSelectionChanged { get; }
        private void DataGriSelectionChangedExecute()
        {
            if (SelectedInspection != null)
                IsPopupBoxEnabled = true;
            else IsPopupBoxEnabled = false;
        }

        public DelegateCommand DeleteInspectionInfo { get; }
        private void DeleteInspectionInfoExcute()
        {
            if (SelectedInspection == null) return;

            EntityDataHelper.DeleteInspection(SelectedInspection);

            StandardNumberSelectionChangeExcute();
        }

        #region リビジョンセレクターの呼び出しからのテンプレート設定画面呼び出し
        public DelegateCommand ShowDetailButtonClick { get; }
        internal void ShowDetailButtonClickExcute()
        {
            var templateList = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TemplateEntity>(
                new SQLFilter(nameof(TemplateEntity.ParentId), SelectedInspection.Id));

            if (templateList.Count == 0)
            {
                var temp = new TemplateEntity(SelectedInspection);
                SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(temp);
                StartUpTemplateSettingView(temp);
                return;
            }
            else if (templateList.Count == 1)
            {
                if (templateList[0].Status == Status.Editing)
                {
                    StartUpTemplateSettingView(templateList[0]);
                    return;
                }
            }

            var p = new DialogParameters();
            p.Add(nameof(RevisionSelectorViewModel.Entities), templateList);
            _dialogService.ShowDialog(nameof(RevisionSelectorView), p, RevisionSelectorViewClose);
        }

        private void RevisionSelectorViewClose(IDialogResult dialogResult)
        {
            // プレビューウィンドウ呼び出し
            if (dialogResult.Result == ButtonResult.Retry)
            {
                //IDialogParameters p = new DialogParameters();
                //p.Add(nameof(PreviewWindowViewModel.TargetTemplate), SelectedRevision);
                //_dialogService.ShowDialog(nameof(PreviewBaseView), p,PreviewWindowCloseExecute,"PreviewWindow");

                SelectedRevision = dialogResult.Parameters.
                    GetValue<TemplateEntity>(nameof(RevisionSelectorViewModel.SelectedRevision));
                var fm = new PreviewWindowView(SelectedRevision);
                fm.Show();

                return;
            }

            else if (dialogResult.Result != ButtonResult.OK) return;

            // リビジョンセレクターによる選択結果を取得
            SelectedRevision = dialogResult.Parameters.
                GetValue<TemplateEntity>(nameof(RevisionSelectorViewModel.SelectedRevision));

            if (Status.CanSelect(SelectedRevision.Status) == false) return;

            // 承認済みのテンプレートを基に新たなRevisionのテンプレートを作成
            if (SelectedRevision.Status == Status.Approved)
            {
                var maxRev = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TemplateEntity>(
                    new SQLFilter(nameof(TemplateEntity.ParentId), SelectedInspection.Id))
                    .OrderBy(x => x.Revision).Last().Revision;

                var newTemplate = CreateNextRevTemplateHelper.Execute(SelectedRevision, maxRev);
                StartUpTemplateSettingView(newTemplate);
            }
            else
            {
                SelectedRevision.Status = Status.Editing;
                SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(SelectedRevision);
                StartUpTemplateSettingView(SelectedRevision);
            }
        }


        private void StartUpTemplateSettingView(TemplateEntity te)
        {
            //_regionManager.RequestNavigate("TemplateRegion", nameof(BlankView));
            var p = new NavigationParameters();
            p.Add(nameof(TemplateSettingViewModel.TargetTemplate.Id), te.Id);
            _regionManager.RequestNavigate("TemplateRegion", nameof(TemplateSettingView), p);
        }
        #endregion

        #region 試験情報(条項、名称、TypeTest,RotinTestの有無)の修正
        public DelegateCommand DataGridMouseDoubleClick { get; }
        private void DataGridMouseDoubleClickExcute()
        {
            ModifyInspectionInfoExcute();
        }

        public DelegateCommand ModifyInspectionInfo { get; }
        private void ModifyInspectionInfoExcute()
        {
            if (SelectedInspection == null) return;

            var p = new DialogParameters();
            p.Add(nameof(ChangeInspectionInfoViewModel.Entity), SelectedInspection);
            _dialogService.ShowDialog(nameof(ChangeInspectionInfoView), p, ModifyInspectionInfoViewClose);
        }

        public DelegateCommand AddnewInspectionInfo { get; }
        private void AddnewInspectionInfoExcute()
        {
            if (SelectedNumber == null) return;

            var newInspection = new InspectionEntity(SelectedNumber);

            var p = new DialogParameters();
            p.Add(nameof(ChangeInspectionInfoViewModel.Entity), newInspection);
            _dialogService.ShowDialog(nameof(ChangeInspectionInfoView), p, ModifyInspectionInfoViewClose);
        }

        private void ModifyInspectionInfoViewClose(IDialogResult dialogResult)
        {
            if (dialogResult.Result == ButtonResult.OK)
            {
                SelectedInspection = dialogResult.Parameters.
                    GetValue<InspectionEntity>(nameof(ChangeInspectionInfoViewModel.Entity));
                SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(SelectedInspection);

                StandardNumberSelectionChangeExcute();
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => false;


        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //_regionManager.Regions.Remove("TemplateRegion");
        }
        #endregion

    }
}