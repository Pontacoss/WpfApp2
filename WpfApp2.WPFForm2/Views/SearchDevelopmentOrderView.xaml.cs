using System;
using System.Windows.Controls;

namespace WpfApp2.WPFForm2.Views
{
	/// <summary>
	/// Interaction logic for SearchOrderView
	/// </summary>
	public partial class SearchDevelopmentOrderView : UserControl
	{
		public SearchDevelopmentOrderView()
		{
			InitializeComponent();
#if DEBUG
			constructorCounter++;
			Console.WriteLine("SearchDevelopmentOrderView Constructor" + constructorCounter);
#endif

		}

		private static int constructorCounter = 0;
		private static int destructorCounter = 0;

		~SearchDevelopmentOrderView()
		{
#if DEBUG
			destructorCounter++;
			Console.WriteLine("SearchDevelopmentOrderView Destructor" + destructorCounter);
#endif
		}

	}
}
