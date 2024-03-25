using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using WpfApp2.Domain.DTO;

using WpfApp2.Infrastructure;
using WpfApp2.WPFForm2.Views;
using WpfApp2.Domain.Services.DiffMatchPatch;
using Prism.Regions;

using WpfApp2.Domain.ValueObjects;
using SQLWriter;
using WpfApp2.WPFForm2.Helpers;
using WpfApp2.Domain.Entities;

namespace WpfApp2.WPFForm2.ViewModels
{
	[RegionMemberLifetime(KeepAlive = false)]
	public class ReviewAndApproveViewModel : BindableBase,INavigationAware
	{
		private string _authority;
		public string Authority
		{
			get { return _authority; }
			set { SetProperty(ref _authority, value); }
		}

		private bool _isDiffEnabled = false;
		public bool IsDiffEnabled
		{
			get { return _isDiffEnabled; }
			set { SetProperty(ref _isDiffEnabled, value); }
		}

		private TemplateDTO _selectedDTO;
		public TemplateDTO SelectedDTO
		{
			get { return _selectedDTO; }
			set { SetProperty(ref _selectedDTO, value); }
		}

		private ObservableCollection<TemplateDTO> _waitingCheckList = new ObservableCollection<TemplateDTO>();
		public ObservableCollection<TemplateDTO> WaitingCheckList
		{
			get { return _waitingCheckList; }
			set { SetProperty(ref _waitingCheckList, value); }
		}

		private ObservableCollection<UserControl> _previewNewRev = new ObservableCollection<UserControl>();
		public ObservableCollection<UserControl> PreviewNewRev
		{
			get { return _previewNewRev; }
			set { SetProperty(ref _previewNewRev, value); }
		}

		private ObservableCollection<WebBrowser> _diffMatchPatchViewJP = new ObservableCollection<WebBrowser>();
		public ObservableCollection<WebBrowser> DiffMatchPatchViewJP
		{
			get { return _diffMatchPatchViewJP; }
			set { SetProperty(ref _diffMatchPatchViewJP, value); }
		}

		private ObservableCollection<WebBrowser> _diffMatchPatchViewEN = new ObservableCollection<WebBrowser>();
		public ObservableCollection<WebBrowser> DiffMatchPatchViewEN
		{
			get { return _diffMatchPatchViewEN; }
			set { SetProperty(ref _diffMatchPatchViewEN, value); }
		}

		private string _checkerComment;
		public string CommentTextBox
		{
			get { return _checkerComment; }
			set { SetProperty(ref _checkerComment, value); }
		}
		

		//private IRegionManager _regionManager;

		public ReviewAndApproveViewModel()
		{
			ReviewGrid_SelectionChanged = new DelegateCommand(ReviewGrid_SelectionChangedExecute);
			ReviewGrid_MouseDoubleClick = new DelegateCommand(ReviewGrid_MouseDoubleClickExecute);
			Button_Click = new DelegateCommand<string>(Button_ClickExecute);

//#if DEBUG
//			constructorCounter++;
//			Console.WriteLine("ReviewAndApproveViewModel Constructor" + constructorCounter);
//#endif

		}

		//		private static int constructorCounter = 0;
		//		private static int destructorCounter = 0;

		//		~ReviewAndApproveViewModel()
		//		{
		//#if DEBUG
		//			destructorCounter++;
		//			Console.WriteLine("ReviewAndApproveViewModel Destructor" + destructorCounter);
		//#endif
		//		}


		public DelegateCommand<string> Button_Click { get; }
		private void Button_ClickExecute(string result)
		{
			if (SelectedDTO == null) return;

			string str;
			if (result == "OK") str = Status.StatusAdvance(SelectedDTO.Template, CommentTextBox);
			else str = Status.StatusRegression(SelectedDTO.Template, CommentTextBox);

			if (Authority == "照査")
				SelectedDTO.Template.CheckerComment = str + SelectedDTO.Template.CheckerComment;
			else
			{
				SelectedDTO.Template.ApproverComment = str + SelectedDTO.Template.ApproverComment;
				if (result == "OK")
				{
					SelectedDTO.Template.IssueDate = new Date(DateTime.Now);
				}
			}

			SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(SelectedDTO.Template);

			if (Authority == "検認" && result == "OK")
			{
				var ins = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<InspectionEntity>(
						new SQLFilter(nameof(InspectionEntity.Id), SelectedDTO.Template.ParentId)).Single();

				SQLFilter p = new SQLFilter(nameof(TemplateEntity.ParentId), SelectedDTO.Template.ParentId);
				ins.GetLatestRevision(SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TemplateEntity>(p));
				SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(ins);
			}
			WaitingListRefresh();
		}

		public DelegateCommand ReviewGrid_SelectionChanged { get; }
		private void ReviewGrid_SelectionChangedExecute()
		{
			PreviewNewRev.Clear();

			if (SelectedDTO == null) return;

			//var p = new NavigationParameters();
			//p.Add(nameof(PreviewControlViewModel.TargetTemplate), SelectedDTO.Template);
			//_regionManager.RequestNavigate("PreviewRegion", nameof(PreviewControlView), p);

			PreviewNewRev.Add(new PreviewControlView(SelectedDTO.Template));

			DiffMatchPatchViewJP.Clear();
			DiffMatchPatchViewEN.Clear();
			if (SelectedDTO.Template.BaseTemplateId > 0)
			{
				IsDiffEnabled = true;
				DiffMatchPatchViewJP.Add(DiffMatchPatchHelper.ExtractDifference(SelectedDTO.Template, true));
				DiffMatchPatchViewEN.Add(DiffMatchPatchHelper.ExtractDifference(SelectedDTO.Template, false));
			}
			else IsDiffEnabled = false;

		}

		public DelegateCommand ReviewGrid_MouseDoubleClick { get; }
		private void ReviewGrid_MouseDoubleClickExecute()
		{
			Button_ClickExecute("OK");
		}

		private void WaitingListRefresh()
		{
			DiffMatchPatchViewJP.Clear();
			DiffMatchPatchViewEN.Clear();
			PreviewNewRev.Clear();
			WaitingCheckList.Clear();
			CommentTextBox = string.Empty;

			List<TemplateEntity> tempList = new List<TemplateEntity>();

			if (Authority == "照査")
			{
				var f = new SQLFilter(nameof(TemplateEntity.Status), Status.Request);
				tempList = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TemplateEntity>(f);
			}
			else
			{
				var f = new SQLFilter(nameof(TemplateEntity.Status), Status.Checked);
				tempList = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TemplateEntity>(f);
			}

			foreach (var te in tempList)
			{
				var f = new SQLFilter(nameof(InspectionEntity.Id), te.ParentId);
				var ie = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<InspectionEntity>(f).SingleOrDefault();
				WaitingCheckList.Add(new TemplateDTO(te, ie));
			}
		}

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			Authority = navigationContext.Parameters.GetValue<string>(nameof(Authority));
			WaitingListRefresh();
		}

		public bool IsNavigationTarget(NavigationContext navigationContext)=>false;

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{
		}
	}
}
