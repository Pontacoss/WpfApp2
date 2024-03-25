using MaterialDesignColors.Recommended;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WpfApp2.Domain.Helpers;
using WpfApp2.Domain.DTO;
using WpfApp2.Domain.Entities;

using WpfApp2.Domain.ValueObjects;
using WpfApp2.Infrastructure;
using WpfApp2.WPFForm2.Views;
using VMHelper = WpfApp2.WPFForm2.Helpers.TemplateApplicationVMHelper;
using SQLWriter;

namespace WpfApp2.WPFForm2.ViewModels
{
	[RegionMemberLifetime(KeepAlive = false)]
	public class TemplateApplicationViewModel : BindableBase, INavigationAware
	{
		private readonly ObservableCollection<InspectionDTO> _baseList = new ObservableCollection<InspectionDTO>();
		private List<InspectionDTO> _appliedList = new List<InspectionDTO>();

		#region Property Setting
		private ProjectListDTO _targetDevOrder;
		public ProjectListDTO TargetDevOrder
		{
			get { return _targetDevOrder; }
			set { SetProperty(ref _targetDevOrder, value); }
		}

		private string _developOrderName;
		public string DevelopOrderName
		{
			get { return _developOrderName; }
			set { SetProperty(ref _developOrderName, value); }
		}
		private string _pojectName;
		public string ProjectName
		{
			get { return _pojectName; }
			set { SetProperty(ref _pojectName, value); }
		}
		private string _customerName;
		public string CustomerName
		{
			get { return _customerName; }
			set { SetProperty(ref _customerName, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		private ObservableCollection<TemplateCombinationEntity> _issuedSpecSheet=new ObservableCollection<TemplateCombinationEntity>();
		public ObservableCollection<TemplateCombinationEntity> IssuedSpecSheet
		{
			get { return _issuedSpecSheet; }
			set { SetProperty(ref _issuedSpecSheet, value); }
		}

		private TemplateCombinationEntity _issuedSpecSheetSelectedItems = new TemplateCombinationEntity();
		public TemplateCombinationEntity IssuedSpecSheetSelectedItems
		{
			get { return _issuedSpecSheetSelectedItems; }
			set { SetProperty(ref _issuedSpecSheetSelectedItems, value); }
		}
		/// <summary>
		/// Standard ComboBox Binding Items
		/// </summary>
		private ObservableCollection<string> _StandardNames = new ObservableCollection<string>();
		public ObservableCollection<string> StandardNames
		{
			get { return _StandardNames; }
			set { SetProperty(ref _StandardNames, value); }
		}

		private string _selectedName = string.Empty;
		public string SelectedName
		{
			get { return _selectedName; }
			set { SetProperty(ref _selectedName, value); }
		}

		/// <summary>
		/// Standard Number ComboBox Binding Items
		/// </summary>
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

		/// <summary>
		/// LeftGrid Binding Items
		/// </summary>
		private ObservableCollection<InspectionDTO> _leftGrid = new ObservableCollection<InspectionDTO>();
		public ObservableCollection<InspectionDTO> LeftGrid
		{
			get { return _leftGrid; }
			set { SetProperty(ref _leftGrid, value); }
		}

		private InspectionDTO _leftGridSelectedItems = null;
		public InspectionDTO LeftGridSelectedItems
		{
			get { return _leftGridSelectedItems; }
			set { SetProperty(ref _leftGridSelectedItems, value); }
		}

		/// <summary>
		/// RightGrid Binding Items
		/// </summary>
		private ObservableCollection<InspectionDTO> _rightGrid = new ObservableCollection<InspectionDTO>();
		public ObservableCollection<InspectionDTO> RightGrid
		{
			get { return _rightGrid; }
			set { SetProperty(ref _rightGrid, value); }
		}

		private InspectionDTO _rightGridSelectedItems = null;
		public InspectionDTO RightGridSelectedItems
		{
			get { return _rightGridSelectedItems; }
			set { SetProperty(ref _rightGridSelectedItems, value); }
		}

		private TemplateEntity _selectedRevision = null;
		public TemplateEntity SelectedRevision
		{
			get { return _selectedRevision; }
			set { SetProperty(ref _selectedRevision, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		private bool _attentionIsVisibility=true;
		public bool AttentionIsVisibility
		{
			get { return _attentionIsVisibility; }
			set { SetProperty(ref _attentionIsVisibility, value); }
		}

		private string _combinationCheckResult =string.Empty;
		public string CombinationCheckResult
		{
			get { return _combinationCheckResult; }
			set { SetProperty(ref _combinationCheckResult, value); }
		}
		private Revision _revCheckResult=null;
		/// <summary>
		/// 上のカードのコントロールのバインディングパラメータ
		/// </summary>
		private ObservableCollection<Revision> _revisionList=new ObservableCollection<Revision>();
		public ObservableCollection<Revision> RevisionList
		{
			get { return _revisionList; }
			set { SetProperty(ref _revisionList, value); }
		}

		private Revision _selectedSSRevision = null;
		public Revision SelectedSSRevision
		{
			get { return _selectedSSRevision; }
			set { SetProperty(ref _selectedSSRevision, value); }
		}
		#endregion

		IDialogService _dialogService;
		IRegionManager _regionManager;

		private List<TemplateCombinationEntity> _targetModelTempCombi = new List<TemplateCombinationEntity>();

		#region コンストラクター
		public TemplateApplicationViewModel(IDialogService dialogService,IRegionManager regionManager)
		{
			_dialogService= dialogService;
			_regionManager= regionManager;
			StandardSelectionChange = new DelegateCommand(StandardSelectionChangeExcute);
			NumberSelectionChange = new DelegateCommand(StandardNumberSelectionChangeExcute);
			LeftGridMouseDoubleClick = new DelegateCommand(LeftGridMouseDoubleClickExecute);
			LeftGridSelectionChanged = new DelegateCommand(LeftGridSelectionChangedExecute);
			RightGridMouseDoubleClick = new DelegateCommand(RightGridMouseDoubleClickExecute);
			RightGridSelectionChanged = new DelegateCommand(RightGridSelectionChangedExecute);
			MoveButton_Click = new DelegateCommand<string>(MoveButton_ClickExecute);
			SelectRevisionButtonClick = new DelegateCommand(SelectRevisionButtonClickExecute);
			GoBackButton_Click = new DelegateCommand(GoBackButton_ClickExecute);
			SpecSheetRevChanged = new DelegateCommand(SpecSheetRevChangedExecute);
			GoNextButton_Click=new DelegateCommand(GoNextButton_ClickExecute);



#if DEBUG
			constructorCounter++;
			seq = constructorCounter;
			Console.WriteLine("TemplateApplicationViewModel Constructor" + constructorCounter);
#endif

		}

		private static int constructorCounter = 0;
		private int seq = 0;

		~TemplateApplicationViewModel()
		{
#if DEBUG
			Console.WriteLine("TemplateApplicationViewModel Destructor" + seq);
#endif
		}
		#endregion

		#region DelegateCommand
		public DelegateCommand GoNextButton_Click { get; }
		private void GoNextButton_ClickExecute()
		{
			var targetSpecSheet = new SpecSheetBaseEntity();
			
			if (_revCheckResult == null)
			{
				var newCombiId = VMHelper.CreateNewTemplateCombination(RightGrid.ToList(), TargetDevOrder.ModelId);
				targetSpecSheet=VMHelper.SpecSheetBaseUpdate(TargetDevOrder, newCombiId);
			}
			else
			{
				List<SQLFilter> p = new List<SQLFilter>();
				p.Add(new SQLFilter(nameof(TemplateCombinationEntity.ModelId), TargetDevOrder.ModelId));
				p.Add(new SQLFilter(nameof(TemplateCombinationEntity.Revision), _revCheckResult));

				var tempCombi = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TemplateCombinationEntity>(p).SingleOrDefault();

				if (tempCombi == null)
				{
					throw new NotImplementedException();
					//targetSpecSheet = VMHelper.SpecSheetBaseUpdate(TargetDevOrder, tempCombi.Id);
				}

				List<SQLFilter> f = new List<SQLFilter>
				{
					new SQLFilter(nameof(SpecSheetBaseEntity.ModelId), TargetDevOrder.ModelId),
					new SQLFilter(nameof(SpecSheetBaseEntity.DevelopmentOrderId), TargetDevOrder.DevelopmentOrderId),
					new SQLFilter(nameof(SpecSheetBaseEntity.TemplateCombinationId), tempCombi.Id)
				};
				targetSpecSheet = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<SpecSheetBaseEntity>(f).SingleOrDefault();

				if (targetSpecSheet == null)
				{
					targetSpecSheet = new SpecSheetBaseEntity(
						TargetDevOrder.SpecSheetName,
						TargetDevOrder.DevelopmentOrderId,
						TargetDevOrder.ModelId,
						tempCombi.Id);
					SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(targetSpecSheet);
				}
			}
			var np = new NavigationParameters();
			np.Add(nameof(ParameterInputViewModel.TargetSpecSheet), targetSpecSheet);
			np.Add(nameof(ParameterInputViewModel.TargetDevOrder), TargetDevOrder);
			_regionManager.RequestNavigate("ContentRegion", nameof(ParameterInputView), np);
		}

		public DelegateCommand SpecSheetRevChanged { get; }
		/// <summary>
		/// 選択したRevisionのTemplateConbinationを呼び出し、右側のグリッドに表示する。
		/// </summary>
		private void SpecSheetRevChangedExecute()
		{
			if (SelectedSSRevision == null || SelectedSSRevision.Value ==string.Empty) return;

			var fs = new List<SQLFilter>();
			fs.Add(new SQLFilter(nameof(TemplateCombinationEntity.ModelId), TargetDevOrder.ModelId));
			fs.Add(new SQLFilter(nameof(TemplateCombinationEntity.Revision),　SelectedSSRevision));

			var entity = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TemplateCombinationEntity>(fs).SingleOrDefault();
			if (entity == null) throw new NotImplementedException();

			_appliedList.Clear();
			RightGrid.Clear();
			foreach (var temp in entity.TemplateCombination)
			{
				var f = new SQLFilter(nameof(TemplateEntity.Id), temp);
				var te = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TemplateEntity>(f).Single();
				var f1 = new SQLFilter(nameof(InspectionEntity.Id), te.ParentId);
				var ie= SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<InspectionEntity>(f1).Single();
				_appliedList.Add(new InspectionDTO(ie, te));
				RightGrid.Add(new InspectionDTO(ie, te));
			}
			CombinationCheckResult = string.Empty;
			_revCheckResult = SelectedSSRevision;
			StandardNumberSelectionChangeExcute();
		}

		public DelegateCommand GoBackButton_Click { get; }
		private void GoBackButton_ClickExecute()
		{
			_regionManager.RequestNavigate("ContentRegion",nameof(SearchDevelopmentOrderView));
		}

		public DelegateCommand SelectRevisionButtonClick { get; }
		private void SelectRevisionButtonClickExecute()
		{
			if (LeftGridSelectedItems == null) return;

			var fs=new List<SQLFilter>();
			fs.Add(new SQLFilter(nameof(TemplateEntity.ParentId), LeftGridSelectedItems.Id));
			fs.Add(new SQLFilter(nameof(TemplateEntity.Status), Status.Approved));
			var templateList = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TemplateEntity>(fs);

			var p = new DialogParameters();
			p.Add(nameof(RevisionSelectorViewModel.Entities), templateList);
			_dialogService.ShowDialog(nameof(RevisionSelectorView), p, RevisionSelectorViewClose);
		}

		private void RevisionSelectorViewClose(IDialogResult dialogResult)
		{
			if (dialogResult.Result == ButtonResult.Retry)
			{
				SelectedRevision = dialogResult.Parameters.
					GetValue<TemplateEntity>(nameof(RevisionSelectorViewModel.SelectedRevision));

				var fm = new PreviewWindowView(SelectedRevision);
				fm.Show();
				return;
			}
			else if (dialogResult.Result != ButtonResult.OK) return;

			SelectedRevision = dialogResult.Parameters.
				GetValue<TemplateEntity>(nameof(RevisionSelectorViewModel.SelectedRevision));

			LeftGridSelectedItems.SelectedRevision = SelectedRevision.Revision;
		}


		public DelegateCommand<string> MoveButton_Click { get; }
		private void MoveButton_ClickExecute(string action) {
			if (action.Contains("Apply"))
			{
				if (action == "Apply")
				{
					if (LeftGridSelectedItems == null) return;
					RightGrid.Add(LeftGridSelectedItems);
					LeftGrid.Remove(LeftGridSelectedItems);
				}
				else if (action == "ApplyAll")
				{
					RightGrid.AddRange(LeftGrid);
					LeftGrid.Clear();
				}
				var list = RightGrid.ToList();
				RightGrid.Clear();
				RightGrid.AddRange(list.OrderBy(x => x.Clause));
			}
			else
			{
				if (action == "Exclude")
				{
					if (RightGridSelectedItems == null) return;
					LeftGrid.Add(RightGridSelectedItems);
					RightGrid.Remove(RightGridSelectedItems);
				}
				else
				{
					LeftGrid.AddRange(RightGrid);
					RightGrid.Clear();
				}
				var list = LeftGrid.ToList();
				LeftGrid.Clear();
				LeftGrid.AddRange(list.OrderBy(x => x.Clause));
			}

			#region Combinationのチェック
			// ① 選択したRevから変わったかどうかだけのチェック
			//CombinationIsChanged = !InspectionDTO.CombinationEqals(_appliedList, RightGrid.ToList());

			// ② テンプレートの選択状態がいづれのRevに該当するかまでをチェック。
			(CombinationCheckResult, _revCheckResult) =VMHelper.CombinationCheck(_targetModelTempCombi, RightGrid.ToList());
			#endregion

			StandardNumberSelectionChangeExcute();
		}

 		public DelegateCommand LeftGridMouseDoubleClick { get; }
		private void LeftGridMouseDoubleClickExecute() 
		{
			MoveButton_ClickExecute("Apply");
		}

		public DelegateCommand LeftGridSelectionChanged { get; }
		private void LeftGridSelectionChangedExecute() { }

		public DelegateCommand RightGridMouseDoubleClick { get; }
		private void RightGridMouseDoubleClickExecute() 
		{
			MoveButton_ClickExecute("Exclude");
		}

		public DelegateCommand RightGridSelectionChanged { get; }
		private void RightGridSelectionChangedExecute() { }

		public DelegateCommand StandardSelectionChange { get; }
		private void StandardSelectionChangeExcute()
		{
			StandardNumbers.Clear();

			var f = new SQLFilter(nameof(StandardEntity.StandardType), SelectedName);
			StandardNumbers.AddRange(SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<StandardEntity>(f));
		}

		public DelegateCommand NumberSelectionChange { get; }
		private void StandardNumberSelectionChangeExcute()
		{
			LeftGrid.Clear();
			if (SelectedNumber == null) return;

			IEqualityComparer<InspectionDTO> comparer =new InspectionDTOComparer();
			var exceptList=_baseList.Except(RightGrid.ToList(), comparer);
			var list = exceptList.Where(x => x.ParentId == SelectedNumber.Id)
				.OrderBy(x => x.Clause).ToList();

			LeftGrid.AddRange(list);
			AttentionIsVisibility=false;
		}
		#endregion

		#region INavigationAwaer
		public bool IsNavigationTarget(NavigationContext navigationContext) => false;

		public void OnNavigatedFrom(NavigationContext navigationContext) 
		{
		}

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			var f = new SQLFilter(nameof(InspectionEntity.LatestRevision), string.Empty, "!=");
			var list = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<InspectionEntity>(f);
			list.ForEach(x => _baseList.Add(new InspectionDTO(x)));

			StandardNames.AddRange(StandardEntity.DistinctNameList(
				SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<StandardEntity>()));

			TargetDevOrder = navigationContext.Parameters.
				GetValue<ProjectListDTO>(nameof(TargetDevOrder));

			var listSe= SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<SpecSheetBaseEntity>(
				new SQLFilter(nameof(SpecSheetBaseEntity.DevelopmentOrderId),
				TargetDevOrder.DevelopmentOrderId));
			var listTe = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TemplateCombinationEntity>();
			IssuedSpecSheet.AddRange(listSe.Join(listTe, x => x.TemplateCombinationId, y => y.Id,
				(x, y) => new TemplateCombinationEntity
				(
					y.ModelId,
					y.Revision,
					y.TemplateCombination
					)
				{ }));

			_targetModelTempCombi = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TemplateCombinationEntity>(
				new SQLFilter(nameof(TemplateCombinationEntity.ModelId), TargetDevOrder.ModelId));

			if (_targetModelTempCombi.Count > 0)
			{
				RevisionList.AddRange(_targetModelTempCombi
					.Select(x => x.Revision).Distinct());
			}

			SelectedSSRevision = TargetDevOrder.SelectedRevision;
			_revCheckResult = SelectedSSRevision;

			SpecSheetRevChangedExecute();
		}
		#endregion
	}
}
