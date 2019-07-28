using System.Windows;
using Kakeibo.ViewModel;

namespace Kakeibo.View
{
	/// <summary>
	/// FixKakeiboItemDialog.xaml の相互作用ロジック
	/// </summary>
	public partial class FixKakeiboItemDialog : Window
	{
		public FixKakeiboItemDialog(KakeiboViewModel kakeiboVM)
		{
			InitializeComponent();

			this.DataContext = kakeiboVM;
		}
	}
}
