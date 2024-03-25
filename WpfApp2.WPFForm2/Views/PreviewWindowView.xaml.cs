using Prism.Services.Dialogs;
using System.Windows;
using System.Windows.Controls;
using WpfApp2.Domain.Entities;
using WpfApp2.WPFForm2.ViewModels;

namespace WpfApp2.WPFForm2.Views
{
	/// <summary>
	/// Interaction logic for PreviewWindowView.xaml
	/// </summary>
	public partial class PreviewWindowView : Window, IDialogWindow
	{
		public PreviewWindowView(TemplateEntity te)
		{
			InitializeComponent();
			((PreviewWindowViewModel)this.DataContext).TargetTemplate = te;
		}

		public IDialogResult Result { get; set; }
	}
}
