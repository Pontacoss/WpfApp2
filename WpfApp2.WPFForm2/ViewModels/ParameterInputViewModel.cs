using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using SQLWriter;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using WpfApp2.Domain.DTO;
using WpfApp2.Domain.Entities;
using WpfApp2.WPFForm2.Views;

namespace WpfApp2.WPFForm2.ViewModels
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class ParameterInputViewModel : BindableBase, INavigationAware
    {
        private ObservableCollection<InspectionDTO> _appliedInspection
            = new ObservableCollection<InspectionDTO>();
        public ObservableCollection<InspectionDTO> AppliedInspection
        {
            get { return _appliedInspection; }
            set { SetProperty(ref _appliedInspection, value); }
        }

        private InspectionDTO _selectedInspectionDTO;
        public InspectionDTO SelectedInspectionDTO
        {
            get { return _selectedInspectionDTO; }
            set { SetProperty(ref _selectedInspectionDTO, value); }
        }


        private TemplateCombinationEntity _targetCombi;
        public TemplateCombinationEntity TargetCombi
        {
            get => _targetCombi;
            set { SetProperty(ref _targetCombi, value); }
        }

        private ProjectListDTO _targetDevOrder;
        public ProjectListDTO TargetDevOrder
        {
            get => _targetDevOrder;
            set { SetProperty(ref _targetDevOrder, value); }
        }

        public static SpecSheetBaseEntity TargetSpecSheet { get; set; }

        private ObservableCollection<UserControl> _previewControl = new ObservableCollection<UserControl>();
        public ObservableCollection<UserControl> PreviewControl
        {
            get => _previewControl;
            set { SetProperty(ref _previewControl, value); }
        }

        private ObservableCollection<UserControl> _previewNewRev = new ObservableCollection<UserControl>();
        public ObservableCollection<UserControl> PreviewNewRev
        {
            get { return _previewNewRev; }
            set { SetProperty(ref _previewNewRev, value); }
        }

        private IRegionManager _regionManager;
        public ParameterInputViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            GoBackButton_Click = new DelegateCommand(GoBackButton_ClickExecute);
            DataGrid_SelectedItemChanged = new DelegateCommand(DataGrid_SelectedItemChangedExecute);
            //#if DEBUG
            //			constructorCounter++;
            //			Console.WriteLine("ParameterInputViewModel Constructor" + constructorCounter);
            //#endif

        }

        //		private static int constructorCounter = 0;
        //		private static int destructorCounter = 0;

        //		~ParameterInputViewModel()
        //		{
        //#if DEBUG
        //			destructorCounter++;
        //			Console.WriteLine("ParameterInputViewModel Destructor" + destructorCounter);
        //#endif
        //		}

        public DelegateCommand DataGrid_SelectedItemChanged { get; }
        private void DataGrid_SelectedItemChangedExecute()
        {
            var f = new List<SQLFilter>()
            {
                new SQLFilter(nameof(TemplateEntity.ParentId), SelectedInspectionDTO.Id),
                new SQLFilter(nameof(TemplateEntity.Revision), SelectedInspectionDTO.SelectedRevision)
            };
            var te = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TemplateEntity>(f).Single();

            //var pp = new NavigationParameters();
            //pp.Add(nameof(PreviewControlViewModel.TargetTemplate), te);
            //_regionManager.RequestNavigate("PreviewRegion", nameof(PreviewControlView), pp);
            PreviewNewRev.Clear();
            PreviewNewRev.Add(new PreviewControlView(te));
        }

        public DelegateCommand GoBackButton_Click { get; }
        private void GoBackButton_ClickExecute()
        {
            var pp = new NavigationParameters();
            pp.Add(nameof(TemplateApplicationViewModel.TargetDevOrder), TargetDevOrder);
            _regionManager.RequestNavigate("ContentRegion", nameof(TemplateApplicationView), pp);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => false;

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            TargetSpecSheet = navigationContext.Parameters.
                GetValue<SpecSheetBaseEntity>(nameof(TargetSpecSheet));
            TargetDevOrder = navigationContext.Parameters.
                GetValue<ProjectListDTO>(nameof(TargetDevOrder));

            var f = new SQLFilter(nameof(TemplateCombinationEntity.Id), TargetSpecSheet.TemplateCombinationId);
            TargetCombi = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TemplateCombinationEntity>(f).SingleOrDefault();

            foreach (var id in TargetCombi.TemplateCombination)
            {
                var ff = new SQLFilter(nameof(TemplateEntity.Id), id);
                var te = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TemplateEntity>(ff).Single();
                var fff = new SQLFilter(nameof(InspectionEntity.Id), te.ParentId);
                var ie = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<InspectionEntity>(fff).Single();
                AppliedInspection.Add(new InspectionDTO(ie, te));
            }

            //PreviewControl.Add(new PreviewControlView(targetTemplate,true));
        }
    }
}
