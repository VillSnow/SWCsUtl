using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

#nullable enable

namespace SWCsUtl.Debug
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}


		private void Click_CollectionSubjectAsyncTest(object sender, RoutedEventArgs e)
		{
			var w = new CollectionSubjectAsyncTest.TestWindow
			{
				DataContext = new CollectionSubjectAsyncTest.TestContext(),
				Owner = this
			};
			w.Show();
		}
	}
}
