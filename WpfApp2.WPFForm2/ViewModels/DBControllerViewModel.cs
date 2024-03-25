using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using SQLWriter;
using System;
using System.Collections.ObjectModel;
using System.Data;
using WpfApp2.Domain.DTO;
using WpfApp2.Domain.Entities;

using WpfApp2.WPFForm2.Views;

namespace WpfApp2.WPFForm2.ViewModels
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class DBControllerViewModel : BindableBase, INavigationAware
    {
        private ObservableCollection<DBListEntity> _dbListDataGridSource = new ObservableCollection<DBListEntity>();
        public ObservableCollection<DBListEntity> DBListDataGridSource
        {
            get => _dbListDataGridSource;
            set { SetProperty(ref _dbListDataGridSource, value); }
        }

        private DBListEntity _selectedDB = null;
        public DBListEntity SelectedDB
        {
            get => _selectedDB;
            set { SetProperty(ref _selectedDB, value); }
        }

        private ObservableCollection<DBParameterEntity> _dbParameterDataGridSource = new ObservableCollection<DBParameterEntity>();
        public ObservableCollection<DBParameterEntity> DBParameterDataGridSource
        {
            get => _dbParameterDataGridSource;
            set { SetProperty(ref _dbParameterDataGridSource, value); }
        }

        private DataTable _dbValueDataGridSource = new DataTable();
        public DataTable DBValueGridGridSource
        {
            get => _dbValueDataGridSource;
            set { SetProperty(ref _dbValueDataGridSource, value); }
        }

        public DelegateCommand DBListDataGrid_SelectedCellsChanged { get; }
        public DelegateCommand AddNewTableButton_Click { get; }
        public DelegateCommand UpDate_Click { get; }
        private IDialogService _dialogService;

        public DBControllerViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            DBListDataGrid_SelectedCellsChanged = new DelegateCommand(DBListDataGrid_SelectedCellsChangedExecute);
            AddNewTableButton_Click = new DelegateCommand(AddNewTableButton_ClickExecute);
            UpDate_Click = new DelegateCommand(UpDate_ClickExecute);

            DBListDataGridSource.AddRange(SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<DBListEntity>());

#if DEBUG
            constructorCounter++;
            Console.WriteLine("DBControllerViewModel Constructor" + constructorCounter);
#endif

        }

        private static int constructorCounter = 0;
        private static int destructorCounter = 0;

        ~DBControllerViewModel()
        {
#if DEBUG
            destructorCounter++;
            Console.WriteLine("DBControllerViewModel Destructor" + destructorCounter);
#endif
        }

        private void DBListDataGrid_SelectedCellsChangedExecute()
        {
            if (SelectedDB == null) return;

            var p = new SQLFilter(nameof(DBParameterEntity.DBId), SelectedDB.Id);
            var p1 = new SQLFilter(nameof(DBValueEntity.DBId), SelectedDB.Id);

            var dto = new DBTableDTO
                (
                    SelectedDB.Id,
                    SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<DBParameterEntity>(p),
                    SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<DBValueEntity>(p1)
                );

            DBParameterDataGridSource.Clear();
            DBParameterDataGridSource.AddRange(dto.ColumnElements);
            DBValueGridGridSource = dto.Source;
        }

        private void AddNewTableButton_ClickExecute()
        {
            _dialogService.ShowDialog(nameof(DBAddNewDBTableView), DBAddNewDBTableViewClose);
        }

        private void DBAddNewDBTableViewClose(IDialogResult result)
        {
            DBListDataGridSource.Clear();
            DBListDataGridSource.AddRange(SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<DBListEntity>());
            DBParameterDataGridSource.Clear();
            DBValueGridGridSource.Clear();
        }

        private void UpDate_ClickExecute()
        {
            foreach (var dbp in DBParameterDataGridSource)
            {
                SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(dbp);
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => false;

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}

