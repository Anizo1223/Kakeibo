using Kakeibo.ViewModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Kakeibo.View
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainView : Window
	{
		public MainView()
		{
			InitializeComponent();
			this.DataContext = new KakeiboViewModel();
		}

		private void ListHeader_Click(object sender, RoutedEventArgs e)
		{
			var header = (GridViewColumnHeader)e.OriginalSource;

			if(header.Column == null)
			{
				return;
			}

			var binding = (Binding)header.Column.DisplayMemberBinding;
			var path = binding.Path.Path;

			var listview = (ListView)sender;
			var pre_sort_items = listview.ItemsSource.Cast<object>();
			var sorted_items = pre_sort_items.OrderBy(row =>
			{
				return row.GetType().GetProperty(path).GetValue(row);
			}).ToList();

			// すでに昇順なら降順にする
			if (sorted_items.SequenceEqual(pre_sort_items))
			{
				sorted_items.Reverse();
			}

			listview.ItemsSource = sorted_items;
		}
	}
}
