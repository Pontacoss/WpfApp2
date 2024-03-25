using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using SQLWriter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp2.Domain.DTO;
using WpfApp2.Domain.Entities;
using WpfApp2.Domain.ValueObjects;
using WpfApp2.WPFForm2.Helpers;
using WpfApp2.WPFForm2.Interfaces;
using WpfApp2.WPFForm2.Views;

namespace WpfApp2.WPFForm2.ViewModels
{
    [RegionMemberLifetime(KeepAlive = true)]
    public class TemplateSettingViewModel : ViewModelBase, INavigationAware
    {
        private static ObservableCollection<ParameterDTO> _paramDataGridSource
            = new ObservableCollection<ParameterDTO>();
        public ObservableCollection<ParameterDTO> ParamDataGridSource
        {
            get => _paramDataGridSource;
            set { SetProperty(ref _paramDataGridSource, value); }
        }

        private string _templateNameLabel = null;
        public string TemplateNameLabel
        {
            get => _templateNameLabel;
            set { SetProperty(ref _templateNameLabel, value); }
        }

        private string _templateNameLabelEN = null;
        public string TemplateNameLabelEN
        {
            get => _templateNameLabelEN;
            set { SetProperty(ref _templateNameLabelEN, value); }
        }

        private string _revisionLabel = null;
        public string RevisionLabel
        {
            get => _revisionLabel;
            set { SetProperty(ref _revisionLabel, value); }
        }

        private ObservableCollection<DBTreeViewDTO> _dBParamTreeViewSource
            = new ObservableCollection<DBTreeViewDTO>();
        public ObservableCollection<DBTreeViewDTO> DBParamTreeViewSource
        {
            get => _dBParamTreeViewSource;
            set { SetProperty(ref _dBParamTreeViewSource, value); }
        }

        private ObservableCollection<CompositionDTO> _compositionTreeViewSource
            = new ObservableCollection<CompositionDTO>();
        public ObservableCollection<CompositionDTO> CompositionTreeViewSource
        {
            get => _compositionTreeViewSource;
            set { SetProperty(ref _compositionTreeViewSource, value); }
        }

        private CompositionDTO _selectedItem = null;
        public CompositionDTO SelectedItem
        {
            get => _selectedItem;
            set { SetProperty(ref _selectedItem, value); }
        }

        private ObservableCollection<ParamTreeDTO> _paramTreeViewSource
            = new ObservableCollection<ParamTreeDTO>();
        public ObservableCollection<ParamTreeDTO> ParamTreeViewSource
        {
            get => _paramTreeViewSource;
            set { SetProperty(ref _paramTreeViewSource, value); }
        }

        private ParamTreeDTO _paramSelectedItem = null;
        public ParamTreeDTO ParamSelectedItem
        {
            get => _paramSelectedItem;
            set { SetProperty(ref _paramSelectedItem, value); }
        }
        private string _authorCommentNew = null;
        public string AuthorCommentNew
        {
            get => _authorCommentNew;
            set { SetProperty(ref _authorCommentNew, value); }
        }
        private string _authorComment = null;
        public string AuthorComment
        {
            get => _authorComment;
            set { SetProperty(ref _authorComment, value); }
        }
        private string _checkerComment = null;
        public string CheckerComment
        {
            get => _checkerComment;
            set { SetProperty(ref _checkerComment, value); }
        }
        private string _approverComment = null;
        public string ApproverComment
        {
            get => _approverComment;
            set { SetProperty(ref _approverComment, value); }
        }
        private string _description = null;
        public string Description
        {
            get => _description;
            set { SetProperty(ref _description, value); }
        }

        private bool isDeleted = false;
        private bool isSaved = false;

        private TemplateEntity _targetTemplate;
        public TemplateEntity TargetTemplate
        {
            get => _targetTemplate;
            set
            {
                SetProperty(ref _targetTemplate, value);
                if (value == null) UserControl_Visibility = false;
                else UserControl_Visibility = true;
            }
        }

        private bool _userControl_Visibility = true;
        public bool UserControl_Visibility
        {
            get => _userControl_Visibility;
            set { SetProperty(ref _userControl_Visibility, value); }
        }
        private bool _attentionIsVisibility = true;
        public bool AttentionIsVisibility
        {
            get => _attentionIsVisibility;
            set { SetProperty(ref _attentionIsVisibility, value); }
        }

        private IRegionManager _regionManager;


        public TemplateSettingViewModel(IDialogService dialogService, IRegionManager regionManager)
            : base(nameof(TemplateSettingViewModel))
        {
            _regionManager = regionManager;

            MenuItem_Click = new DelegateCommand<string>(MenuItem_ClickExecute);
            SelectedItemChanged = new DelegateCommand<CompositionDTO>(SelectedItemChangedExecute);
            UserControl_Unloaded = new DelegateCommand(UserControl_UnloadedExecute);
            ParamTreeView_DoubleClick = new DelegateCommand(ParamTreeView_DoubleClickExecute);
            ParamTreeSelectedItemChanged = new DelegateCommand<ParamTreeDTO>(ParamTreeSelectedItemChangedExecute);
            RequestCheckButton_Click = new DelegateCommand(RequestCheckButton_ClickExecute);
            DeleteTemplateButton_Click = new DelegateCommand(DeleteTemplateButton_ClickExecute);
            PreviewButton_Click = new DelegateCommand(PreviewButton_ClickExecute);
            TabSelectionChanged = new DelegateCommand<SelectionChangedEventArgs>(TabSelectionChangedExecute);
            ContextMenu_ContextMenuOpening = new DelegateCommand<ContextMenuEventArgs>(ContextMenu_ContextMenuOpeningExecute);
            DBParamTreeView_MouseDoubleClick = new DelegateCommand(DBParamTreeView_MouseDoubleClickExecute);
            DBParamTreeView_SelectedItemChanged = new DelegateCommand<DBTreeViewDTO>(DBParamTreeView_SelectedItemChangedExecute);
            ChangeOrderButton_Click = new DelegateCommand<string>(ChangeOrderButton_ClickExecute);
            DeleteParameterButton_Click = new DelegateCommand(DeleteParameterButton_ClickExecute);
            AddParameterButton_Click = new DelegateCommand(AddParameterButton_ClickExecute);
            ChangeMethodButton_Click = new DelegateCommand(ChangeMethodButton_ClickExecute);

#if DEBUG
            //constructorCounter++;
            //seq = constructorCounter;
            //Console.WriteLine("TemplateSettingViewModel Constructor" + constructorCounter);
#endif

        }

        //		private static int constructorCounter = 0;
        //		private int seq = 0;

        //		~TemplateSettingViewModel():~base()
        //		{
        //#if DEBUG
        //			Console.WriteLine("TemplateSettingViewModel Destructor" + seq);
        //#endif
        //		}

        public DelegateCommand ChangeMethodButton_Click { get; }
        private void ChangeMethodButton_ClickExecute()
        {
            SelectedParameter.InputMethod = InputMethod.ChangeMethod(SelectedParameter.InputMethod);
        }

        public ParameterDTO SelectedParameter { get; set; }

        public DelegateCommand<string> ChangeOrderButton_Click { get; }
        private void ChangeOrderButton_ClickExecute(string param)
        {
            if (SelectedParameter == null) return;
            var pos = SelectedParameter.Sequence;

            int indexp;
            if (param == "UP") indexp = ParamDataGridSource.IndexOf(SelectedParameter) - 1;
            else indexp = ParamDataGridSource.IndexOf(SelectedParameter) + 1;

            if (indexp < 0 || ParamDataGridSource.Count <= indexp) return;
            var p = ParamDataGridSource[indexp];
            (SelectedParameter.Sequence, p.Sequence) = (p.Sequence, SelectedParameter.Sequence);

            var list = ParamDataGridSource.OrderBy(x => x.Sequence).ToList();
            ParamDataGridSource.Clear();
            ParamDataGridSource.AddRange(list);

        }

        public DelegateCommand DeleteParameterButton_Click { get; }
        private void DeleteParameterButton_ClickExecute()
        {
            if (SelectedParameter == null) return;
            ParamDataGridSource.Remove(SelectedParameter);
        }

        public DelegateCommand AddParameterButton_Click { get; }
        private void AddParameterButton_ClickExecute()
        {
            int insertPoint;
            if (SelectedParameter == null) insertPoint = ParamDataGridSource.Count();
            else insertPoint = ParamDataGridSource.IndexOf(SelectedParameter);
            ParamDataGridSource.Where(x => x.Sequence >= insertPoint).ToList().ForEach(x => x.Sequence++);
            var newEntity = new ParameterDTO(TargetTemplate.Id, insertPoint);
            SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(new ParameterEntity(newEntity));
            ParamDataGridSource.Insert(insertPoint, newEntity);
        }


        private DBTreeViewDTO _selectedDBDTO;
        public DelegateCommand<DBTreeViewDTO> DBParamTreeView_SelectedItemChanged { get; set; }
        private void DBParamTreeView_SelectedItemChangedExecute(DBTreeViewDTO dbdto)
        {
            _selectedDBDTO = dbdto;
        }

        public DelegateCommand DBParamTreeView_MouseDoubleClick { get; }
        private void DBParamTreeView_MouseDoubleClickExecute()
        {
            if (ParamDataGridSource.Where(x => x.TableId == _selectedDBDTO.DBId && x.SelectDBParamId == _selectedDBDTO.Id).Any()) return;
            ParameterDTO filter = null;
            if (_selectedDBDTO.IsPrimary == false)
            {
                var fs = new List<SQLFilter>();
                fs.Add(new SQLFilter(nameof(DBParameterEntity.DBId), _selectedDBDTO.DBId));
                fs.Add(new SQLFilter(nameof(DBParameterEntity.IsPrimaryKey), true));
                var primaryKey = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<DBParameterEntity>(fs).First().Id;
                filter = ParamDataGridSource.SingleOrDefault(x => x.TableId == _selectedDBDTO.DBId && x.SelectDBParamId == primaryKey);
                if (filter == null)
                {
                    MessageBox.Show("先に主キーを割りつけてください。");
                    return;
                }
            }

            var newEntity = new ParameterEntity()
            {
                ParentId = TargetTemplate.Id,
                NameJP = new ParamName(_selectedDBDTO.Name),
                FilterParamId = filter == null ? 0 : filter.Id,
                SelectDBParamId = _selectedDBDTO.Id,
                TableId = _selectedDBDTO.DBId,
                Sequence = ParamDataGridSource.Count > 0 ? ParamDataGridSource.Max(x => x.Sequence) + 1 : 0,
            };
            if (_selectedDBDTO.IsPrimary) newEntity.InputMethod = InputMethod.Select;
            else newEntity.InputMethod = InputMethod.Reference;

            ParamDataGridSource.Add(new ParameterDTO(newEntity));

            if (_selectedDBDTO.IsPrimary == true)
            {
                EntityDataHelper.ParameterEntityListSave(ParamDataGridSource.ToList(), TargetTemplate.Id);
                ParamTreeViewRefresh();
            }
        }

        public DelegateCommand<ParamTreeDTO> ParamTreeSelectedItemChanged { get; }
        private void ParamTreeSelectedItemChangedExecute(ParamTreeDTO pe)
        {
            ParamSelectedItem = pe;
        }

        public DelegateCommand ParamTreeView_DoubleClick { get; }
        private void ParamTreeView_DoubleClickExecute()
        {
            if (ParamSelectedItem == null || ParamSelectedItem.Id == 0) return;

            var view = _regionManager.Regions["CompositionRegion"].ActiveViews.FirstOrDefault() as UserControl;
            var ta = view?.DataContext as ICompositionRegionElements;
            ta?.AddParameter(ParamSelectedItem);
        }

        public DelegateCommand<CompositionDTO> SelectedItemChanged { get; }
        private void SelectedItemChangedExecute(CompositionDTO dto)
        {
            if (dto == null) return;
            SelectedItem = dto;
            var p = new NavigationParameters();

            if (SelectedItem.AreaType == AreaType.TextArea)
            {
                p.Add(nameof(TextAreaViewModel.TargetEntity),
                    SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TextAreaEntity>(
                    new SQLFilter(nameof(TextAreaEntity.Id), SelectedItem.AreaDataId)).Single());
                _regionManager.RequestNavigate("CompositionRegion", nameof(TextAreaView), p);
            }
            else if (SelectedItem.AreaType == AreaType.TableArea)
            {
                p.Add(nameof(TableAreaViewModel.TargetEntity),
                    SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TableAreaEntity>(
                    new SQLFilter(nameof(TableAreaEntity.Id), SelectedItem.AreaDataId)).Single());
                _regionManager.RequestNavigate("CompositionRegion", nameof(TableAreaView), p);
            }
            else if (SelectedItem.AreaType == AreaType.FigureArea)
            {
                p.Add(nameof(FigureAreaViewModel.TargetEntity),
                    SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<FigureAreaEntity>(
                    new SQLFilter(nameof(FigureAreaEntity.Id), SelectedItem.AreaDataId)).Single());
                _regionManager.RequestNavigate("CompositionRegion", nameof(FigureAreaView), p);
            }
            else _regionManager.Regions["CompositionRegion"].RemoveAll();
        }

        public DelegateCommand<string> MenuItem_Click { get; }
        private void MenuItem_ClickExecute(string parameter)
        {
            if (!(SelectedItem is CompositionDTO pt)) pt = CompositionTreeViewSource.First();

            if (parameter.Contains("エリア"))
            {
                pt.Children.Add(CompositionDTOHelper.CreateNewComposition(pt, parameter));
                AttentionIsVisibility = false;
            }
            else if (pt.Level > 0)
            {
                switch (parameter)
                {
                    case "上に移動":
                        CompositionDTOHelper.MoveInTree(pt, 1);
                        break;
                    case "下に移動":
                        CompositionDTOHelper.MoveInTree(pt, -1);
                        break;
                    case "削除":
                        CompositionDTOHelper.DeleteInTree(pt);
                        if (CompositionTreeViewSource[0].Children.Count == 0) AttentionIsVisibility = true;
                        break;
                    case "総数可変":
                        pt.IsVariable = !pt.IsVariable;
                        break;
                    case "所内向け":
                        pt.IsHide = !pt.IsHide;
                        break;
                }
            }
        }

        private void ParamTreeViewRefresh()
        {

            ParamTreeViewSource.Clear();

            var p1 = (from p in SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<ParameterEntity>(
                new SQLFilter(nameof(ParameterEntity.ParentId), -1))
                      orderby p.Id
                      select new ParamTreeDTO(p.Id, p.NameJP.Value, p.Value)).ToList();

            var p2 = (from p in ParamDataGridSource
                      orderby p.Sequence, p.Id
                      select new ParamTreeDTO(p.Id, p.NameJP, p.Value)).ToList();

            ParamTreeViewSource.AddRange(new List<ParamTreeDTO>
            {
                new ParamTreeDTO() { Name = "共通変数", Children = p1 },
                new ParamTreeDTO() { Name = "ID固有変数", Children = p2 }
            });
        }

        public DelegateCommand UserControl_Unloaded { get; }
        private void UserControl_UnloadedExecute()
        {
            foreach (var window in PreviewWindowViewModel.PreviewWindows)
            {
                window.Close();
            }
            if (isDeleted) return;
            if (isSaved) return;
            SaveTemplate();
        }

        private void SaveTemplate()
        {
            EntityDataHelper.CompositionListSave(CompositionTreeViewSource.ToList());
            EntityDataHelper.ParameterEntityListSave
            (
                ParamDataGridSource.ToList(),
                TargetTemplate.Id
            );

            TargetTemplate.SaveTemplate(Description, new Date(DateTime.Now), AuthorComment);
            SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(TargetTemplate);
        }

        public DelegateCommand RequestCheckButton_Click { get; }
        private void RequestCheckButton_ClickExecute()
        {
            if (TargetTemplate == null) return;
            AuthorComment += Status.StatusAdvance(TargetTemplate, AuthorCommentNew);

            SaveTemplate();
            isSaved = true;
            TargetTemplate = null;
        }

        public DelegateCommand<ContextMenuEventArgs> ContextMenu_ContextMenuOpening { get; }
        private void ContextMenu_ContextMenuOpeningExecute(ContextMenuEventArgs e)
        {
            if (!(SelectedItem is CompositionDTO pt)) return;

            var tv = e.Source as TreeView;

            foreach (var d in tv.ContextMenu.Items.SourceCollection)
            {
                if (d is MenuItem)
                {
                    var dd = d as MenuItem;
                    if (dd.Header.ToString().Contains("エリア追加"))
                    {
                        if (pt.Level == 0) dd.IsEnabled = true;
                        else if (pt.Level == 1 && pt.IsVariable) dd.IsEnabled = true;
                        else dd.IsEnabled = false;
                    }
                    else if (dd.Header.ToString().Contains("移動"))
                    {
                        if (pt.Level > 0) dd.IsEnabled = true;
                        else dd.IsEnabled = false;
                    }
                    else if (dd.Header.ToString() == "削除")
                    {
                        if (pt.Level > 0) dd.IsEnabled = true;
                        else dd.IsEnabled = false;
                    }
                    else if (dd.Header.ToString().Contains("設定"))
                    {
                        if (pt.Level == 1)
                        {
                            if (dd.Header.ToString().Contains("可変") && pt.IsHide)
                            {
                                dd.IsEnabled = false;
                            }
                            else if (dd.Header.ToString().Contains("所内") && pt.IsVariable)
                            {
                                dd.IsEnabled = false;
                            }
                            else if (pt.IsVariable && pt.Children.Count > 0)
                            {
                                dd.IsEnabled = false;
                            }
                            else dd.IsEnabled = true;
                        }
                        else if (pt.Level == 2 && dd.Header.ToString().Contains("所内"))
                        {
                            dd.IsEnabled = true;
                        }
                        else
                        {
                            dd.IsEnabled = false;
                        }
                    }
                }
            }
        }

        public DelegateCommand<SelectionChangedEventArgs> TabSelectionChanged { get; }
        private void TabSelectionChangedExecute(SelectionChangedEventArgs e)
        {
            if (!(e.Source is TabControl tab)) return;
            if (((TabItem)tab.SelectedItem).Header.ToString() == "パラメータ設定")
            {

            }
            else if (((TabItem)tab.SelectedItem).Header.ToString() == "文書構成設定")
            {
                EntityDataHelper.ParameterEntityListSave
                (
                    ParamDataGridSource.ToList(),
                    TargetTemplate.Id
                );
                ParamTreeViewRefresh();
            }
        }
        public DelegateCommand DeleteTemplateButton_Click { get; }
        private void DeleteTemplateButton_ClickExecute()
        {
            var dialogResult = MessageBox.Show("このテンプレートを削除します。" +
                "\nこの処理は元に戻せません。よろしいですか？",
                "テンプレートの削除", MessageBoxButton.OKCancel);
            if (dialogResult == MessageBoxResult.Cancel) return;
            EntityDataHelper.DeleteTemplate(TargetTemplate);
            isDeleted = true;
            TargetTemplate = null;
        }

        public DelegateCommand PreviewButton_Click { get; }
        private void PreviewButton_ClickExecute()
        {
            if (PreviewWindowViewModel.PreviewWindows.Count > 1) return;

            var obj = _regionManager.Regions["CompositionRegion"].ActiveViews.SingleOrDefault() as UserControl;
            var vm = obj?.DataContext as ICompositionRegionElements;
            vm?.SaveAreaData();

            SaveTemplate();

            var fm = new PreviewWindowView(TargetTemplate);
            fm.Show();

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            isSaved = false;

            DBParamTreeViewSource.Clear();
            DBParamTreeViewSource.AddRange(DBTreeViewDTO.GetDTO(
                SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<DBListEntity>(),
                SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<DBParameterEntity>()
                ));

            TargetTemplate = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TemplateEntity>(
                new SQLFilter(nameof(TemplateEntity.Id),
                navigationContext.Parameters.GetValue<int>(nameof(TargetTemplate.Id)))).Single();

            var inspectionEntity = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<InspectionEntity>(
                new SQLFilter(nameof(InspectionEntity.Id), _targetTemplate.ParentId)).Single();

            TemplateNameLabel = inspectionEntity.NameJP.Value;
            TemplateNameLabelEN = inspectionEntity.NameEN.Value;
            RevisionLabel = "Rev." + TargetTemplate.Revision.DisplayValue;

            var list = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<ParameterEntity>(
                new SQLFilter(nameof(ParameterEntity.ParentId), TargetTemplate.Id))
                .OrderBy(x => x.Sequence).ThenBy(x => x.Id);

            int seq = 0;
            ParamDataGridSource.Clear();
            foreach (var l in list)
            {
                l.Sequence = seq++;
                ParamDataGridSource.Add(new ParameterDTO(l));
            }

            AuthorCommentNew = "";
            AuthorComment = TargetTemplate.AuthorComment;
            CheckerComment = TargetTemplate.CheckerComment;
            ApproverComment = TargetTemplate.ApproverComment;
            Description = TargetTemplate.Description;

            CompositionTreeViewSource.Clear();
            CompositionTreeViewSource.AddRange(
                CompositionDTOHelper.ConvertCompositionToDto(TargetTemplate));

            if (CompositionTreeViewSource[0].Children.Count > 0)
                AttentionIsVisibility = false;

            ParamTreeViewRefresh();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            if (!isDeleted)
            {
                if (!isSaved)
                {
                    SaveTemplate();
                    isSaved = true;
                }
            }

            // KeepAlive=false の場合の各プロパティ初期化
            //ParamDataGridSource = null;
            //ParamTreeViewSource = null;
            //DBParamTreeViewSource = null;
            //_selectedDBDTO = null;

            //DBParamTreeView_SelectedItemChanged = null;
            //CompositionTreeViewSource = null;
            //TargetTemplate = null;
            //ParamSelectedItem = null;
            //SelectedItem=null;
            //this.AuthorCommentNew = null;
            //this.ApproverComment = null;
            //this.CheckerComment = null;
            //this.Description = null;
            //this.TemplateNameLabel = null;
            //this.TemplateNameLabelEN = null;
            //this.RevisionLabel = null;
            //SelectedParameter = null;
            //_regionManager.Regions["TemplateRegion"].RemoveAll();
            //_regionManager.Regions.Remove("CompositionRegion");
            //GC.Collect();
        }
    }
}
