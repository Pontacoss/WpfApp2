using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using WpfApp2.Domain.Helpers;
using WpfApp2.Domain.Entities;
using WpfApp2.Domain.DTO;
using WpfApp2.Domain.ValueObjects;
using WpfApp2.WPFForm2.Views;
using SQLWriter;

namespace WpfApp2.WPFForm2.ViewModels
{
	[RegionMemberLifetime(KeepAlive = false)]
	public class SearchDevelopmentOrderViewModel : BindableBase, INavigationAware
	{
		#region Property Setting
		private string _searchWardDevOrder = string.Empty;
		public string SearchWardDevOrder
		{
			get { return _searchWardDevOrder; }
			set { SetProperty(ref _searchWardDevOrder, value); }
		}

		private string _searchWardSepcSheetID = string.Empty;
		public string SearchWardSepcSheetID
		{
			get { return _searchWardSepcSheetID; }
			set { SetProperty(ref _searchWardSepcSheetID, value); }
		}

		private ObservableCollection<ProjectListDTO> _revList 
			= new ObservableCollection<ProjectListDTO>();
		public ObservableCollection<ProjectListDTO> RevList
		{
			get { return _revList; }
			set { SetProperty(ref _revList, value); }
		}
		private List<string> _searchWardSeriesNo = new List<string>();
		public List<string> SearchWardSeriesNo
		{
			get { return _searchWardSeriesNo; }
			set { SetProperty(ref _searchWardSeriesNo, value); }
		}
		private List<string> _statusList = new List<string>();
		public List<string> StatusList
		{
			get { return _statusList; }
			set { SetProperty(ref _statusList, value); }
		}
		private string _searchWardProductType = string.Empty;
		public string SearchWardProductType
		{
			get { return _searchWardProductType; }
			set { SetProperty(ref _searchWardProductType, value); }
		}
		private string _searchWardOrder = string.Empty;
		public string SearchWardOrder
		{
			get { return _searchWardOrder; }
			set { SetProperty(ref _searchWardOrder, value); }
		}
		private ObservableCollection<ProjectListDTO> _customerList
			= new ObservableCollection<ProjectListDTO>();
		public ObservableCollection<ProjectListDTO> CustomerList
		{
			get { return _customerList; }
			set { SetProperty(ref _customerList, value); }
		}
		private ObservableCollection<ProjectListDTO> _projectNameList
			= new ObservableCollection<ProjectListDTO>();
		public ObservableCollection<ProjectListDTO> ProjectNameList
		{
			get { return _projectNameList; }
			set { SetProperty(ref _projectNameList, value); }
		}
		private List<string> _authorList = new List<string>();
		public List<string> AuthorList
		{
			get { return _authorList; }
			set { _authorList = value; }
		}

		private ObservableCollection<ProjectListDTO> _projectList
			= new ObservableCollection<ProjectListDTO>();
		public ObservableCollection<ProjectListDTO> ProjectList
		{
			get { return _projectList; }
			set { SetProperty(ref _projectList, value); }
		}
		private ObservableCollection<ProjectListDTO> _seriesList 
			= new ObservableCollection<ProjectListDTO>();
		public ObservableCollection<ProjectListDTO> SeriesList
		{
			get { return _seriesList; }
			set { SetProperty(ref _seriesList, value); }
		}

		private ProjectListDTO _selectedCustomer;
		public ProjectListDTO SelectedCustomer
		{
			get { return _selectedCustomer; }
			set { SetProperty(ref _selectedCustomer, value); }
		}
		private ProjectListDTO _selectedProject;
		public ProjectListDTO SelectedProject
		{
			get { return _selectedProject; }
			set { SetProperty(ref _selectedProject, value); }
		}
		private ProjectListDTO _selectedRevision;
		public ProjectListDTO SelectedRevision
		{
			get { return _selectedRevision; }
			set { SetProperty(ref _selectedRevision, value); }
		}

		private ProjectListDTO _selectedSeries;
		public ProjectListDTO SelectedSeries
		{
			get { return _selectedSeries; }
			set { SetProperty(ref _selectedSeries, value); }
		}

		private ProjectListDTO _selectedDevOrder;
		public ProjectListDTO SelectedDevOrder
		{
			get { return _selectedDevOrder; }
			set { SetProperty(ref _selectedDevOrder, value); }
		}
		#endregion

		private IRegionManager _regionManager;
		private IDialogService _dialogService;
		private List<ProjectListDTO> _allprojectList
			= new List<ProjectListDTO>();

		#region　コンストラクター
		public SearchDevelopmentOrderViewModel(IRegionManager regionManager, IDialogService dialogService)
		{
			_regionManager = regionManager;
			_dialogService = dialogService;
			DevOrderSelectButtonClick = new DelegateCommand(DevOrderSelectButtonClickExecute);
			ConditionClearButtonClick = new DelegateCommand(ConditionClearButtonClickExecute);

			CustomerSelectionChanged = new DelegateCommand(CustomerSelectionChangedExecute);
			ProjectSelectionChanged = new DelegateCommand(ProjectSelectionChangedExecute);
			SeriesSelectionChanged = new DelegateCommand(SeriesSelectionChangedExecute);
			RevisionSelectionChanged = new DelegateCommand(RevisionSelectionChangedExecute);

			DevOrderTextChanged = new DelegateCommand(DevOrderTextChangedExecute);
			SpecSheetIdTextChanged = new DelegateCommand(SpecSheetIdChangedExecute);
			ProductTypeTextChanged = new DelegateCommand(ProductTypeChangedExecute);

			DataGridMouseDoubleClick = new DelegateCommand(DataGridMouseDoubleClickExecute);
//#if DEBUG
//			constructorCounter++;
//			seq = constructorCounter;
//			Console.WriteLine("SearchDevelopmentOrderViewModel Constructor" + constructorCounter);
//#endif

		}

//		private static int constructorCounter = 0;
//		private int seq = 0;

//		~SearchDevelopmentOrderViewModel()
//		{
//#if DEBUG
//			Console.WriteLine("SearchDevelopmentOrderViewModel Destructor" + seq);
//#endif
//		}
		#endregion

		#region DelegateCommand
		public DelegateCommand DataGridMouseDoubleClick { get; }
		private void DataGridMouseDoubleClickExecute()
		{
			DevOrderSelectButtonClickExecute();
		}

		public DelegateCommand DevOrderSelectButtonClick { get; }
		private void DevOrderSelectButtonClickExecute()
		{
			if (!(SelectedDevOrder is ProjectListDTO pl)) return;

			if (string.IsNullOrEmpty(pl.ModelName))
			{
				var p = new DialogParameters();
				p.Add(nameof(ModelSelectorViewModel.TargetDevOrder), SelectedDevOrder);
				_dialogService.ShowDialog(nameof(ModelSelectorView), p, DevelopOrderSelectorViewClose);
			}

			if (string.IsNullOrEmpty(pl.ModelName)) return;

			var pp = new NavigationParameters();
			pp.Add(nameof(TemplateApplicationViewModel.TargetDevOrder), SelectedDevOrder);
			_regionManager.RequestNavigate("ContentRegion", nameof(TemplateApplicationView), pp);

		}

		public DelegateCommand ConditionClearButtonClick { get; }
		private void ConditionClearButtonClickExecute()
		{
			ProjectList.Clear();
			ProjectList.AddRange(_allprojectList);

			ClearComboBoxSource();
			SetComboBoxSource(_allprojectList);

			SelectedCustomer = null;
			SearchWardProductType = string.Empty;
			SearchWardSepcSheetID = string.Empty;
			SearchWardDevOrder = string.Empty;
		}

		public DelegateCommand DevOrderTextChanged { get; }
		private void DevOrderTextChangedExecute()
		{
			SelectionChangeExecute();
		}
		public DelegateCommand ProductTypeTextChanged { get; }
		private void ProductTypeChangedExecute()
		{
			SelectionChangeExecute();
		}
		public DelegateCommand SpecSheetIdTextChanged { get; }
		private void SpecSheetIdChangedExecute()
		{
			SelectionChangeExecute();
		}
		public DelegateCommand CustomerSelectionChanged { get; }
		private void CustomerSelectionChangedExecute()
		{
			SelectionChangeExecute();
		}
		public DelegateCommand ProjectSelectionChanged { get; }
		private void ProjectSelectionChangedExecute()
		{
			SelectionChangeExecute();
		}
		public DelegateCommand SeriesSelectionChanged { get; }
		private void SeriesSelectionChangedExecute()
		{
			SelectionChangeExecute();
		}
		public DelegateCommand RevisionSelectionChanged { get; }
		private void RevisionSelectionChangedExecute()
		{
			SelectionChangeExecute();
		}
		#endregion

		private static string CreateRegexPattern(string bindingParameter)
		{
			var str = bindingParameter.Split(' ');
			string result = str[0];
			for (int i = 1; i < str.Count(); i++)
			{
				result += "[A-Za-z-0-9]*?" + str[i];
			}
			return result;
		}

		private void SelectionChangeExecute()
		{
			List<ProjectListDTO> list = _allprojectList;
			if (!String.IsNullOrEmpty(SearchWardDevOrder))
			{
				list = list.Where(x => Regex.IsMatch(x.DevelopmentOrderNo,
					CreateRegexPattern(SearchWardDevOrder), RegexOptions.IgnoreCase)).ToList();
			}
			if (!String.IsNullOrEmpty(SearchWardSepcSheetID))
			{
				list = list.Where(x => Regex.IsMatch(x.SpecSheetName,
					CreateRegexPattern(SearchWardSepcSheetID), RegexOptions.IgnoreCase)).ToList();
			}
			if (!String.IsNullOrEmpty(SearchWardProductType))
			{
				list = list.Where(x => Regex.IsMatch(x.ModelName,
					CreateRegexPattern(SearchWardProductType), RegexOptions.IgnoreCase)).ToList();
			}

			if (SelectedCustomer != null)
				list = list.Where(x => x.CustomerName == SelectedCustomer.CustomerName).ToList();
			if (SelectedProject != null)
				list = list.Where(x => x.ProjectName == SelectedProject.ProjectName).ToList();
			if (SelectedSeries != null)
				list = list.Where(x => x.SeriesNo == SelectedSeries.SeriesNo).ToList();
			if (SelectedRevision != null)
				list = list.Where(x => x.SelectedRevision == SelectedRevision.SelectedRevision).ToList();

			ProjectList.Clear();
			ProjectList.AddRange(list);

			SetComboBoxSource(list);

		}

		private void ClearComboBoxSource()
		{
			CustomerList.Clear();
			ProjectNameList.Clear();
			RevList.Clear();
			SeriesList.Clear();
		}

		private void SetComboBoxSource(List<ProjectListDTO> list)
		{
			if (SelectedCustomer == null)
			{
				CustomerList.Clear();
				CustomerList.AddRange(list.Distinct(x => x.CustomerName).OrderBy(x => x.CustomerName));
			}
			if (SelectedProject == null)
			{
				ProjectNameList.Clear();
				ProjectNameList.AddRange(list.Distinct(x => x.ProjectName).OrderBy(x => x.ProjectName));
			}
			if (SelectedRevision == null)
			{
				RevList.Clear();
				RevList.AddRange(list.Distinct(x => x.SelectedRevision.Value).OrderBy(x => x.SelectedRevision.Value));
			}
			if (SelectedSeries == null)
			{
				SeriesList.Clear();
				//SeriesList.AddRange(list.Distinct(x => x.SeriesNo).OrderBy(x=>x.SeriesNo));
			}
		}

		private void DevelopOrderSelectorViewClose(IDialogResult result)
		{
			if (result.Result == ButtonResult.OK)
			{
				var selectedModel = result.Parameters.GetValues<ModelEntity>(
					nameof(ModelSelectorViewModel.SelectedModel)).First();

				SelectedDevOrder.ModelId = selectedModel.Id;
				SelectedDevOrder.ModelName = selectedModel.ModelName.Value;
				SelectedDevOrder.SpecSheetName = SpecSheetName.GetNewSpecSheetName(selectedModel,
					SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<SpecSheetBaseEntity>());

			}
		}

		private void InitializeForm()
		{
			var developOrderList = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<DevelopmentOrderEntity>();
			var customerList = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<CustomerEntity>();
			var projectList = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<ProjectEntity>();
			var modelList = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<ModelEntity>();
			var specSheetList = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<SpecSheetBaseEntity>();
			var specSheetRevList = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TemplateCombinationEntity>();
			var spenSheetSeriesNo = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TemplateDataEntity>();

			ModelEntity.GetAndSettingLatestRevision(modelList, specSheetRevList);

			ProjectListDTO.Count = 0;
			_allprojectList = ProjectListDTO.SynthesizeTables(
				developOrderList, customerList, projectList, modelList, specSheetList, specSheetRevList, spenSheetSeriesNo)
				.Distinct(x => x.DevelopmentOrderNo)
				.OrderBy(x => x.No).ToList();

			ProjectList.AddRange(_allprojectList);
			SetComboBoxSource(_allprojectList);
		}
		#region INavigationAware
		public bool IsNavigationTarget(NavigationContext navigationContext) => false;

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{
			_allprojectList = null;
			_selectedDevOrder = null;
			CustomerList = null;
			ProjectList = null;
			ProjectNameList = null;
			RevList = null;
			SelectedDevOrder = null;
			SeriesList = null;
			GC.Collect();
		}

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			InitializeForm();
		}
		#endregion
	}
}
