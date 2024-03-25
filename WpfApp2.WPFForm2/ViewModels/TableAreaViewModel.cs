using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows;

using System.Windows.Controls;
using WpfApp2.Infrastructure;

using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Text;
using Prism.Regions;
using MaterialDesignThemes.MahApps;
using SQLWriter;
using WpfApp2.Domain.Entities;
using WpfApp2.WPFForm2.Interfaces;
using WpfApp2.Domain.DTO;

namespace WpfApp2.WPFForm2.ViewModels
{
	[RegionMemberLifetime(KeepAlive = false)]
	public class TableAreaViewModel : BindableBase, INavigationAware, ICompositionRegionElements
	{
		private Point? _startPos;
		private object _dragItem;
		private object _dragBase;

		private List<ObservableCollection<UIElement>> Stockers = new List<ObservableCollection<UIElement>>();

		private ObservableCollection<UIElement> _valueLabelStocker = new ObservableCollection<UIElement>();
		public ObservableCollection<UIElement> ValueLabelStocker
		{
			get { return _valueLabelStocker; }
			set { SetProperty(ref _valueLabelStocker, value); }
		}

		private bool _columnDirectionIsEnabled;
		public bool ColumnDirectionIsEnabled
		{
			get { return _columnDirectionIsEnabled; }
			set { SetProperty(ref _columnDirectionIsEnabled, value); }
		}

		private ObservableCollection<UIElement> _rowLabelStocker = new ObservableCollection<UIElement>();
		public ObservableCollection<UIElement> RowLabelStocker
		{
			get { return _rowLabelStocker; }
			set { SetProperty(ref _rowLabelStocker, value); }
		}

		private ObservableCollection<UIElement> _columnLabelStocker = new ObservableCollection<UIElement>();
		public ObservableCollection<UIElement> ColumnLabelStocker
		{
			get { return _columnLabelStocker; }
			set { SetProperty(ref _columnLabelStocker, value); }
		}

		private string _tableNameJP = string.Empty;
		public string TableNameJP
		{
			get { return _tableNameJP; }
			set { SetProperty(ref _tableNameJP, value); }
		}
		private string _tableNameEN = string.Empty;
		public string TableNameEN
		{
			get { return _tableNameEN; }
			set { SetProperty(ref _tableNameEN, value); }
		}
		private bool _direc_RowIsChecked;
		public bool Direc_RowIsChecked
		{
			get { return _direc_RowIsChecked; }
			set { SetProperty(ref _direc_RowIsChecked, value); }
		}

		private TableAreaEntity _targetEntity;
		public TableAreaEntity TargetEntity
		{
			get { return _targetEntity; }
			set { SetProperty(ref _targetEntity, value); }
		}

		public TableAreaViewModel()
		{
			Stockers.Add(ValueLabelStocker);
			Stockers.Add(RowLabelStocker);
			Stockers.Add(ColumnLabelStocker);
			UserControl_Unloaded = new DelegateCommand(UserControl_UnloadedExecute);
			DropLabel = new DelegateCommand<DragEventArgs>(DropLabelExecute);

#if DEBUG
			constructorCounter++;
			Console.WriteLine("TableAreaViewModel Constructor" + constructorCounter);
#endif

		}

		private static int constructorCounter = 0;
		private static int destructorCounter = 0;

		~TableAreaViewModel()
		{
#if DEBUG
			destructorCounter++;
			Console.WriteLine("TableAreaViewModel Destructor" + destructorCounter);
#endif
		}

		public DelegateCommand<DragEventArgs> DropLabel { get; }
		private void DropLabelExecute(DragEventArgs e)
		{
			int lb = Convert.ToInt32(e.Data.GetData(typeof(int)));

			var param = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<ParameterEntity>(
						new SQLFilter(nameof(ParameterEntity.Id), lb)).Single();
			DeleteLabel(lb);

			if (param == null) return;

			var target = ((FrameworkElement)e.Source).Name;

			if (target == "ColumnLabelStocker")  ColumnLabelStocker.Add(CreateLabel(lb));
			else if (target == "RowLabelStocker")  RowLabelStocker.Add(CreateLabel(lb));
			else if (target == "ValueLabelStocker")  ValueLabelStocker.Add(CreateLabel(lb));

		}

		public DelegateCommand UserControl_Unloaded { get; }
		private void UserControl_UnloadedExecute()
		{
			// 削除されていた場合、保存しない。
			var e = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TableAreaEntity>(
						new SQLFilter(nameof(ParameterEntity.Id), TargetEntity.Id)).SingleOrDefault(); 
			if (e == null) return;

			SaveAreaData();
		}

		public void SaveAreaData()
		{
			var sb = new StringBuilder();
			foreach (Label lb in RowLabelStocker)
			{
				sb.Append(lb.Tag + ",");
			}
			if (sb.Length > 0)
				sb.Length--;

			var sb1 = new StringBuilder();
			foreach (Label lb in ColumnLabelStocker)
			{
				sb1.Append(lb.Tag + ",");
			}
			if (sb1.Length > 0)
				sb1.Length--;

			var sb2 = new StringBuilder();
			foreach (Label lb in ValueLabelStocker)
			{
				sb2.Append(lb.Tag + ",");
			}
			if (sb2.Length > 0)
				sb2.Length--;

			SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(
				new TableAreaEntity(
				TargetEntity.Id,
				_tableNameJP,
				_tableNameEN,
				Direc_RowIsChecked,
				sb.ToString(),
				sb1.ToString(),
				sb2.ToString()
				));
		}

		private void DeleteLabel(int target)
		{
			foreach (ObservableCollection<UIElement> stocker in Stockers)
			{
				foreach (Label s in stocker)
				{
					if (s!=null && (int)s.Tag == target)
					{
						stocker.Remove(s);
						return;
					}
				}
			}
		}

		public void AddParameter(ParamTreeDTO pt)
		{
			DeleteLabel(pt.Id);
			ValueLabelStocker.Add(CreateLabel(pt.Id));

			if (ValueLabelStocker.Count < 2)
			{
				ColumnDirectionIsEnabled = false;
			}
			else
			{
				ColumnDirectionIsEnabled = true;
			}
		}

		private Label CreateLabel(int labelId)
		{
			var pp = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<ParameterEntity>(
						new SQLFilter(nameof(ParameterEntity.Id), labelId)).Single();

			if (pp == null) return null;
			Label lb = new Label()
			{
				Content = pp.NameJP.Value,
				Margin = new Thickness(5, 10, 5, 0),
				HorizontalAlignment = HorizontalAlignment.Center,
				HorizontalContentAlignment = HorizontalAlignment.Center,
				BorderThickness = new Thickness(2),
				BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("Black")),
				Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("Gray")),
				Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("White")),
				Width = 120,
				ToolTip = "左ドラッグ：移動、ダブルクリック：削除。",
				Tag = labelId,
				AllowDrop = false,
			};

			lb.MouseDoubleClick += (sender1, e1) => Lb_DoubleClick(sender1, e1);
			lb.MouseLeftButtonDown += (sender1, e1) => Lb_LeftButtonDown(sender1, e1);
			lb.MouseMove += (sender1, e1) => Lb_MouseMove(sender1, e1);
			lb.MouseLeftButtonUp += (sender1, e1) => Lb_LeftButtonUp(sender1, e1);
			lb.MouseRightButtonDown += (sender1, e1) => Lb_RightButtonDown(sender1, e1);

			return lb;
		}



		private void Lb_DoubleClick(object sender, MouseButtonEventArgs e)
		{
			var lb = sender as Label;

			foreach (var s in Stockers)
			{
				foreach (Label ss in s.Cast<Label>())
				{
					if (lb.Tag == ss.Tag) { s.Remove(lb); break; }
				}
			}

			if (ValueLabelStocker.Count < 2)
			{
				ColumnDirectionIsEnabled = false;
			}
			else
			{
				ColumnDirectionIsEnabled = true;
			}
		}

		private void Lb_LeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			var lb = sender as Label;

			foreach (var s in Stockers)
			{
				foreach (Label ss in s.Cast<Label>())
				{
					if (lb.Tag == ss.Tag)
					{
						_dragBase = sender as Label;
						_dragItem = lb.Tag;
						_startPos = e.GetPosition(lb);
						return;
					}
				}
			}
		}

		private void Lb_LeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			ClearDragData();
		}

		private static bool IsDragStartable(Vector delta)
		{
			return (SystemParameters.MinimumHorizontalDragDistance < Math.Abs(delta.X)) ||
				   (SystemParameters.MinimumVerticalDragDistance < Math.Abs(delta.Y));
		}

		private void Lb_MouseMove(object sender, MouseEventArgs e)
		{
			if (_startPos != null)
			{
				var curPos = e.GetPosition(sender as Label);
				var p = _dragBase as Label;
				Vector diff = curPos - (Point)_startPos;
				if (IsDragStartable(diff))
				{
					DragDrop.DoDragDrop(p, _dragItem, DragDropEffects.Move);
					ClearDragData();
				}
			}
		}

		private void ClearDragData()
		{
			_startPos = null;
			_dragItem = null;
			_dragBase = null;
		}


		private void Lb_RightButtonDown(object sender, MouseButtonEventArgs e)
		{
			//var targetLabel = sender as Label;
			//ModulateLabelValue(targetLabel.Content.ToString());
		}

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			TargetEntity = navigationContext.Parameters.GetValue<TableAreaEntity>(nameof(TargetEntity));
			_tableNameJP = _targetEntity.TitleString;
			_tableNameEN = _targetEntity.TitleStringEN;

			var str = _targetEntity.RowLabels.Split(',');
			foreach (var lb in str)
			{
				if (int.TryParse(lb, out var val))
				{
					var lbb = CreateLabel(val);
					if (lbb != null) RowLabelStocker.Add(lbb);
				}
			}
			str = _targetEntity.ColumnLabels.Split(',');
			foreach (var lb in str)
			{
				if (int.TryParse(lb, out var val))
				{
					var lbb = CreateLabel(val);
					if (lbb != null) ColumnLabelStocker.Add(lbb);
				}
			}
			str = _targetEntity.ValueLabels.Split(',');
			foreach (var lb in str)
			{
				if (int.TryParse(lb, out var val))
				{
					var lbb = CreateLabel(val);
					if (lbb != null) ValueLabelStocker.Add(lbb);
				}
			}

			Direc_RowIsChecked = _targetEntity.Direction;

			if (ValueLabelStocker.Count < 2)
			{
				ColumnDirectionIsEnabled = false;
			}
			else
			{
				ColumnDirectionIsEnabled = true;
			}
		}


		public bool IsNavigationTarget(NavigationContext navigationContext) => false;

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{
			
		}
	}
}
