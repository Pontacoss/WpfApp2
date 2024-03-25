using System;
using System.Windows.Controls;

namespace WpfApp2.WPFForm2.Views
{
	/// <summary>
	/// Interaction logic for TemplateApplicationView
	/// </summary>
	public partial class TemplateApplicationView : UserControl
	{
		public TemplateApplicationView()
		{
			InitializeComponent();

#if DEBUG
			constructorCounter++;
			Console.WriteLine("TemplateApplicationView Constructor" + constructorCounter);
#endif

		}

		private static int constructorCounter = 0;
		private static int destructorCounter = 0;

		~TemplateApplicationView()
		{
#if DEBUG
			destructorCounter++;
			Console.WriteLine("TemplateApplicationView Destructor" + destructorCounter);
#endif
		}
	}
}
