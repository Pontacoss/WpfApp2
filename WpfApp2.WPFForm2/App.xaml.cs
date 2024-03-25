using Prism.Ioc;
using Prism.Unity;
using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using WpfApp2.Domain.Exceptions;
using WpfApp2.WPFForm2.ViewModels;
using WpfApp2.WPFForm2.Views;
using SQLWriter;

namespace WpfApp2.WPFForm2
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : PrismApplication
	{
		public App()
		{
			SQLWriterFacade.SetReferenceToDomain
				(@"C:\Users\EY28754\source\repos\WpfApp2\WpfApp2.WPFForm2\bin\Debug\"
				, "WpfApp2.Domain.dll"
				, "WpfApp2.Domain.Entities");

			this.DispatcherUnhandledException += App_DispatcherUnhandledException;
			AppDomain.CurrentDomain.FirstChanceException += (sender, args) =>
			{
				ReportException("FirstChanceException.log", sender, args.Exception);
			};
		}

		private static void ReportException(string fileName, object sender, Exception exception)
		{
			const string reportFormat =
			"===========================================================\r\n" +
			"ERROR Date = {0}, Sender = {1}, \r\n" +
			"{2}\r\n\r\n";

			var reportText = string.Format(reportFormat, DateTimeOffset.Now, sender, exception);
			File.AppendAllText(fileName, reportText);
		}

		private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			MessageBoxImage icon = MessageBoxImage.Error;
			string caption = "エラー";
			var exceptionBase = e.Exception as ExceptionBase;
			if (exceptionBase != null)
			{
				if (exceptionBase.Kind == ExceptionBase.ExceptionKind.Info)
				{
					icon = MessageBoxImage.Information;
					caption = "情報";
				}
				else if (exceptionBase.Kind == ExceptionBase.ExceptionKind.Warning)
				{
					icon = MessageBoxImage.Warning;
					caption = "警告";
				}
			}
			MessageBox.Show(e.Exception.Message, caption, MessageBoxButton.OK, icon);
			e.Handled = true;
		}

		protected override Window CreateShell()
		{
			return Container.Resolve<MainView>();
		}

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<MainControlView, MainControlViewModel>();
			containerRegistry.RegisterForNavigation<TemplateSelectView, TemplateSelectViewModel>();
			containerRegistry.RegisterForNavigation<TemplateSettingView, TemplateSettingViewModel>();
			containerRegistry.RegisterForNavigation<TextAreaView, TextAreaViewModel>();
			containerRegistry.RegisterForNavigation<TableAreaView, TableAreaViewModel>();
			containerRegistry.RegisterForNavigation<FigureAreaView, FigureAreaViewModel>();
			containerRegistry.RegisterDialog<ChangeInspectionInfoView, ChangeInspectionInfoViewModel>();
			containerRegistry.RegisterDialog<RevisionSelectorView, RevisionSelectorViewModel>();
			containerRegistry.RegisterDialog<DBAddNewDBTableView, DBAddNewDBTableViewModel>();
			containerRegistry.RegisterForNavigation<ReviewAndApproveView, ReviewAndApproveViewModel>();
			//containerRegistry.RegisterForNavigation<PreviewControlView, PreviewControlViewModel>();
			containerRegistry.RegisterForNavigation<SearchDevelopmentOrderView, SearchDevelopmentOrderViewModel>();
			//containerRegistry.RegisterDialogWindow<PreviewWindowView>(nameof(PreviewWindowView));
			containerRegistry.RegisterDialog<ModelSelectorView, ModelSelectorViewModel>();
			containerRegistry.RegisterForNavigation<TemplateApplicationView, TemplateApplicationViewModel>();
			containerRegistry.RegisterForNavigation<SQLWriterTableSettingView, SQLWriterTableSettingViewModel>();
			containerRegistry.RegisterForNavigation<DBControllerView, DBControllerViewModel>();

			containerRegistry.RegisterForNavigation<ParameterInputView, ParameterInputViewModel>();
			containerRegistry.RegisterForNavigation<SpecSheetCreatorView, SpecSheetCreatorViewModel>();

		}
	}
}
