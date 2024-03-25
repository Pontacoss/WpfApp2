using Prism;
using Prism.Commands;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using SQLWriter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp2.Domain.Entities;
using WpfApp2.Infrastructure;
using WpfApp2.WPFForm2.Views;

namespace WpfApp2.WPFForm2.ViewModels
{
	[RegionMemberLifetime(KeepAlive = false)]
	public class PreviewWindowViewModel : BindableBase
	{
		public static List<Window> PreviewWindows = new List<Window>();
		private Window _window;

		private TemplateEntity _targetTemplate;
		public TemplateEntity TargetTemplate
		{
			get => _targetTemplate;
			set
			{
				_targetTemplate = value;
				TemplateRevLabel = _targetTemplate.Revision.Value;
				TemplateNameLabel = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<InspectionEntity>(
						new SQLFilter(nameof(InspectionEntity.Id), _targetTemplate.ParentId)).Single().NameJP.Value;
				ControlDocker.Add(new PreviewControlView(_targetTemplate));
			}
		}

		private string _templateNameLabel;
		public string TemplateNameLabel
		{
			get => _templateNameLabel;
			set { SetProperty(ref _templateNameLabel, value); }
		}

		public string _templateRevLabel;
		public string TemplateRevLabel
		{
			get => _templateRevLabel;
			set { SetProperty(ref _templateRevLabel, value); }
		}

		private ObservableCollection<UserControl> _controlDocker = new ObservableCollection<UserControl>();
		public ObservableCollection<UserControl> ControlDocker
		{
			get => _controlDocker;
			set { SetProperty(ref _controlDocker, value); }
		}

		private SolidColorBrush _buttonBackGroundColor = Brushes.Gray;
		public SolidColorBrush ButtonBackGroundColor
		{
			get => _buttonBackGroundColor;
			set { SetProperty(ref _buttonBackGroundColor, value); }
		}

		private bool _isTopMost = false;
		public bool IsTopMost
		{
			get => _isTopMost;
			set { SetProperty(ref _isTopMost, value); }
		}

		public PreviewWindowViewModel()
		{
			CloseButton_Click = new DelegateCommand(CloseButton_ClickExecute);
			TopMostButton_Click=new DelegateCommand(TopMostButton_ClickExecute);
			LoadedCommand = new DelegateCommand<Window>(LoadedCommandExecute);
			UnloadedCommand = new DelegateCommand(UnloadedCommandExecute);

//#if DEBUG
//			constructorCounter++;
//			Console.WriteLine("PreviewWindowViewModel Constructor" + constructorCounter);
//#endif

		}

//		private static int constructorCounter = 0;
//		private static int destructorCounter = 0;

//		~PreviewWindowViewModel()
//		{
//#if DEBUG
//			destructorCounter++;
//			Console.WriteLine("PreviewWindowViewModel Destructor" + destructorCounter);
//#endif
//		}

		public DelegateCommand UnloadedCommand { get; }
		private void UnloadedCommandExecute()
		{
			PreviewWindows.Remove(_window);
		}

		public DelegateCommand<Window> LoadedCommand { get; }
		private void LoadedCommandExecute(Window window)
		{
			PreviewWindows.Add(window);
			_window = window;
		}

		public DelegateCommand CloseButton_Click { get; }
		private void CloseButton_ClickExecute()
		{
			_window.Close();
		}

		public DelegateCommand TopMostButton_Click { get; }

		public string Title => "Preview Window";

		private void TopMostButton_ClickExecute()
		{
			if (IsTopMost)
			{
				IsTopMost = false;
				ButtonBackGroundColor = Brushes.Gray;
			}
			else
			{
				IsTopMost = true;
				ButtonBackGroundColor = Brushes.Crimson;
			}
		}

	}
}
