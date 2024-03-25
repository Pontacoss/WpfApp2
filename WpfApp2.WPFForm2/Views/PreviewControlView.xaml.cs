using System.Windows.Controls;
using WpfApp2.Domain.Entities;
using WpfApp2.WPFForm2.ViewModels;

namespace WpfApp2.WPFForm2.Views
{
	/// <summary>
	/// Interaction logic for PreviewControlView
	/// </summary>
	public partial class PreviewControlView : UserControl
	{
		public PreviewControlView(TemplateEntity te, bool isHtml = false) 
		{
			InitializeComponent();
			((PreviewControlViewModel)this.DataContext).IsHtml = isHtml;
			((PreviewControlViewModel)this.DataContext).TargetTemplate = te;
		}
	}
}
