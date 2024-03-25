using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using SQLWriter;
using System;
using WpfApp2.Domain.DTO;
using WpfApp2.Domain.Entities;
using WpfApp2.Infrastructure;
using WpfApp2.Infrastructure.ClosedXML;

using WpfApp2.WPFForm2.Helpers;

namespace WpfApp2.WPFForm2.ViewModels
{
	[RegionMemberLifetime(KeepAlive = false)]
	public class DBAddNewDBTableViewModel : BindableBase, IDialogAware
	{
		private string _tableNameTextBox = "";

		public event Action<IDialogResult> RequestClose;

		public string TableNameTextBox
		{
			get => _tableNameTextBox;
			set { SetProperty(ref _tableNameTextBox, value); }
		}

		public DBAddNewDBTableViewModel()
		{
			InportExcelButton_Click = new DelegateCommand(InportExcelButton_ClickExecute);
			AddNewDBTableButton_Click = new DelegateCommand(AddNewDBTableButton_ClickExecute);

#if DEBUG
			constructorCounter++;
			Console.WriteLine("DBAddNewDBTableViewModel Constructor" + constructorCounter);
#endif

		}

		private static int constructorCounter = 0;
		private static int destructorCounter = 0;

		~DBAddNewDBTableViewModel()
		{
#if DEBUG
			destructorCounter++;
			Console.WriteLine("DBAddNewDBTableViewModel Destructor" + destructorCounter);
#endif
		}

		public DelegateCommand InportExcelButton_Click { get; }
		public DelegateCommand AddNewDBTableButton_Click { get; }

		public string Title => "DB設定";

		private void InportExcelButton_ClickExecute()
		{
			var dto = new DBTableDTO(InportExcelHelper.ExtractData());

			if (dto.Source == null) return;

			if (TableNameTextBox != "")
				dto.Source.TableName = TableNameTextBox;

			var newEnt = new DBListEntity(dto.Source.TableName);
			SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(newEnt);
			DataTableHelper.SaveDataTable(dto.Source, newEnt.Id);

			RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
		}

		private void AddNewDBTableButton_ClickExecute()
		{

		}

		public bool CanCloseDialog()
		{
			return true;
		}

		public void OnDialogClosed() { }

		public void OnDialogOpened(IDialogParameters parameters) { }
	}
}
