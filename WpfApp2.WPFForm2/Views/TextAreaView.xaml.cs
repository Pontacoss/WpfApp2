using System.Windows.Controls;

using WpfApp2.WPFForm2.ViewModels;

namespace WpfApp2.WPFForm2.Views
{
	/// <summary>
	/// Interaction logic for TextAreaView
	/// </summary>
	public partial class TextAreaView : UserControl
	{
		public TextAreaView()
		{
			InitializeComponent();
			//((TextAreaViewModel)this.DataContext).TargetEntity = target as TextAreaEntity;
		}

		private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
		{

		}
	}
}
