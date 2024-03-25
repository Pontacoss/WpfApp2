using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using WpfApp2.Domain.Entities;
using WpfApp2.Domain.DTO;
using SQLWriter;
using WpfApp2.WPFForm2.Helpers;
using WpfApp2.WPFForm2.Interfaces;

namespace WpfApp2.WPFForm2.ViewModels
{
	[RegionMemberLifetime(KeepAlive = false)]
	public class TextAreaViewModel : BindableBase, INavigationAware, ICompositionRegionElements
	{
		private TextAreaEntity _targetEntity;
		public TextAreaEntity TargetEntity
		{
			get { return _targetEntity; }
			set { SetProperty(ref _targetEntity, value); }
		}

		private bool _isVariable = true;
		public bool IsVariable
		{
			get { return _isVariable; }
			set { SetProperty(ref _isVariable, value); }
		}

		private bool _isHide = true;
		public bool IsHide
		{
			get { return _isHide; }
			set { SetProperty(ref _isHide, value); }
		}

		private bool _jpChecked = true;
		public bool JPChecked
		{
			get { return _jpChecked; }
			set { SetProperty(ref _jpChecked, value); }
		}

		private string _mainText = string.Empty;
		public string MainText
		{
			get { return _mainText; }
			set { SetProperty(ref _mainText, value); }
		}

		private string _subText = string.Empty;
		public string SubText
		{
			get { return _subText; }
			set { SetProperty(ref _subText, value); }
		}

		private ObservableCollection<TextParamDTO> _textParamGridSource 
			= new ObservableCollection<TextParamDTO>();
		public ObservableCollection<TextParamDTO> TextParamGridSource
		{
			get { return _textParamGridSource; }
			set { SetProperty(ref _textParamGridSource, value); }
		}

		private string _charCount = string.Empty;
		public string CharCount
		{
			get { return _charCount; }
			set { SetProperty(ref _charCount, value); }
		}

		private TextParamDTO _textParamGridSelectedItem = null;
		public TextParamDTO TextParamGridSelectedItem
		{
			get { return _textParamGridSelectedItem; }
			set { SetProperty(ref _textParamGridSelectedItem, value); }
		}

		private IRegionManager _regionManager;
		public TextAreaViewModel(IRegionManager regionManager)
		{
			_regionManager = regionManager;
			UserControl_Unloaded = new DelegateCommand(UserControl_UnloadedExecute);
			LanguageSelectChange = new DelegateCommand<string>(LanguageSelectChangeExecute);
			SentenceParamDataGrid_Drop = new DelegateCommand<IDataObject>(SentenceParamDataGrid_DropExecute);
			TextBox_Changed = new DelegateCommand(TextBox_ChangedExecute);
			MenuItem_Click = new DelegateCommand<string>(MenuItem_ClickExecute);
			DeleteRow=new DelegateCommand<object>(DeleteRowExecute);

			JPChecked = true;

#if DEBUG
			constructorCounter++;
			Console.WriteLine("TextAreaViewModel Constructor" + constructorCounter);
#endif

		}

		private static int constructorCounter = 0;
		private static int destructorCounter = 0;

		~TextAreaViewModel()
		{
			this.TextParamGridSource = null;
			this.TextParamGridSelectedItem=null;

#if DEBUG
			destructorCounter++;
			Console.WriteLine("TextAreaViewModel Destructor" + destructorCounter);
#endif
		}



		public DelegateCommand<object> DeleteRow { get; }
		private void DeleteRowExecute(object sender)
		{
			var bt = sender as Button;
			if (!(bt.DataContext is TextParamDTO tp)) return;

			tp.ParamId = 0;
			tp.Name = string.Empty;
			tp.Value = string.Empty;
		}

		public DelegateCommand<string> MenuItem_Click { get; }
		private void MenuItem_ClickExecute(string str)
		{
			if (str == "自動インクリメント")
			{
				TextParamGridSelectedItem.ParamId = 1;
				TextParamGridSelectedItem.Name = "自動インクリメント";
				TextParamGridSelectedItem.Value = string.Empty;

			}
			else if (str == "計算式")
			{
				TextParamGridSelectedItem.ParamId = 2;
				TextParamGridSelectedItem.Name = "計算式";
				TextParamGridSelectedItem.Value = string.Empty;
			}
			else
			{
				TextParamGridSelectedItem.ParamId = 0;
				TextParamGridSelectedItem.Name = string.Empty;
				TextParamGridSelectedItem.Value = string.Empty;
			}
			UserControl_UnloadedExecute();
		}

		public DelegateCommand UserControl_Unloaded { get; }
		private void UserControl_UnloadedExecute()
		{
			// 削除されていた場合、保存しない。
			var e = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TextAreaEntity>(
				new SQLFilter(nameof(TextAreaEntity.Id), TargetEntity.Id)).SingleOrDefault();
			if (e == null) return;

			SaveAreaData();
		}

		public void SaveAreaData()
		{
			var entity = new TextAreaEntity();

			entity.Id = TargetEntity.Id;
			if (JPChecked == true)
			{
				entity.Value = MainText;
				entity.ValueEN = SubText;
			}
			else
			{
				entity.Value = SubText;
				entity.ValueEN = MainText;
			}

			int[] args = new int[10] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
			foreach (var p in TextParamGridSource)
			{
				args[p.Id] = p.ParamId;
			}
			entity.Set_Args(args);

			SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(entity);
		}

		public DelegateCommand<string> LanguageSelectChange { get; }
		private void LanguageSelectChangeExecute(string language)
		{
			var dum = SubText;
			SubText = MainText;
			MainText = dum;
		}

		public void AddParameter(ParamTreeDTO pt)
		{
			var te = TextParamGridSource.SingleOrDefault(x => x.ParamId == pt.Id);
			if (te != null) return;
			te = TextParamGridSource.FirstOrDefault(x => x.ParamId == 0);
			if (te == null) return;

			te.ParamId = pt.Id;
			te.Name = pt.Name;
			te.Value = pt.Value;

		}

		public DelegateCommand TextBox_Changed { get; }
		private void TextBox_ChangedExecute()
		{
			CharCount = MainText.Length + "/1000";

			var sb = new StringBuilder();
			sb.Append(MainText + SubText);

			var temp = new List<TextParamDTO>();
			temp.AddRange(TextParamGridSource);

			TextParamGridSource.Clear();

			var list = new List<string>();
			MatchCollection matches = Regex.Matches(sb.ToString(), "{[0-9]}");
			foreach (Match m in matches)
			{
				list.Add(m.Value);
			}
			var disList = list.Select(x => new { Name = x, Index = int.Parse(Regex.Replace(x, @"[^0-9]", "")) })
				.OrderBy(x => x.Index).Select(x => x.Index).Distinct();

			if (!disList.Any()) return;

			foreach (int s in disList)
			{
				var p = temp.SingleOrDefault(x => x.Id == s);
				if (p == null) TextParamGridSource.Add(new TextParamDTO(s, "", 0, ""));
				else TextParamGridSource.Add(p);
			}

		}

		public DelegateCommand<IDataObject> SentenceParamDataGrid_Drop { get; }
		private void SentenceParamDataGrid_DropExecute(IDataObject data)
		{

		}

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			TargetEntity = navigationContext.Parameters.GetValue<TextAreaEntity>(nameof(TargetEntity));
			SubText = _targetEntity.ValueEN;
			TextParamGridSource.AddRange(TextParamDTOHelper.GetDataGridSource(_targetEntity));
			MainText = _targetEntity.Value;
		}

		public bool IsNavigationTarget(NavigationContext navigationContext) => false;

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{
		}
	}
}
