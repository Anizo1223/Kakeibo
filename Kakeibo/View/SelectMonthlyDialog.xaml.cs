using System.Windows;
using Kakeibo.ViewModel;

namespace Kakeibo.View
{
	/// <summary>
	/// SelectMonthlyDialog.xaml の相互作用ロジック
	/// </summary>
	public partial class SelectMonthlyDialog : Window
	{
		public SelectMonthlyDialog(KakeiboViewModel kakeiboVM)
		{
			InitializeComponent();

			this.DataContext = kakeiboVM;
		}
	}
}
