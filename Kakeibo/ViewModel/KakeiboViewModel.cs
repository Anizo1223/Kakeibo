using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using Kakeibo.Model;
using Prism.Commands;

namespace Kakeibo.ViewModel
{
	public class KakeiboViewModel : ViewModelBase
	{
		public KakeiboViewModel()
		{
			this.Kakeibo = new KakeiboModel
			{
				Kingaku = 0,
				Meimoku = string.Empty,
				Date = DateTime.Now
			};

			this.InitializeKakeiboList();

			this.ReadCSV();

			this.InitializeCommand();
		}

		private KakeiboModel m_kakeibo;

		public KakeiboModel Kakeibo
		{
			get { return this.m_kakeibo; }
			set
			{
				this.m_kakeibo = value;
				this.OnPropertyChanged("Kakeibo");
			}
		}

		public ICommand RegColumnCommand { get; set; }

		public ICommand DeleteColumnCommand { get; set; }

		public ObservableCollection<KakeiboModel> KakeiboList { get; set; }

		public ObservableCollection<string> ShushiList { get; set; }

		public KakeiboModel SelectedKakeiboModel { get; set; }

		private void RegColumn()
		{
			this.KakeiboList.Add(Kakeibo);
			this.OnPropertyChanged("KakeiboList");

			this.WriteCSV();

			this.Kakeibo = new KakeiboModel
			{
				Kingaku = 0,
				Meimoku = string.Empty,
				Date = DateTime.Now
			};
		}

		private void DeleteColumn()
		{
			this.KakeiboList.Remove(this.SelectedKakeiboModel);
			this.RefreshKakeiboList();
		}

		private void InitializeKakeiboList()
		{
			this.KakeiboList = new ObservableCollection<KakeiboModel>();
			this.ShushiList = new ObservableCollection<string> { "収入", "支出", };
		}

		private void InitializeCommand()
		{
			this.RegColumnCommand = new DelegateCommand(() => this.RegColumn());
			this.DeleteColumnCommand = new DelegateCommand(() => this.DeleteColumn());
		}

		private void WriteCSV()
		{
			var lines = this.Kakeibo.Date.ToString() + "," + this.Kakeibo.Meimoku + "," + this.Kakeibo.Kingaku.ToString() + "," + this.Kakeibo.Shushi;
			var sw = new StreamWriter(@".\Kakeibo.csv", true);
			sw.WriteLine(lines, Encoding.GetEncoding("utf-8"));
			sw.Close();
		}

		private void ReadCSV()
		{
			try
			{
				var lines = File.ReadAllLines(@".\Kakeibo.csv", Encoding.GetEncoding("utf-8"));

				// 項目名は取り除く
				var newLines = new List<string>();
				newLines.AddRange(lines);
				newLines.RemoveAt(0);

				foreach (var line in newLines.ToArray())
				{
					var item = line.Split(',');
					var KakeiboItem = new KakeiboModel
					{
						Date = DateTime.Parse(item[0]),
						Meimoku = item[1],
						Kingaku = int.Parse(item[2]),
						Shushi = item[3],
					};

					this.KakeiboList.Add(KakeiboItem);
					this.OnPropertyChanged("KakeiboList");
				}
			}
			catch (Exception)
			{
				var sw = new StreamWriter(@".\Kakeibo.csv");
				var columnName = "日付,内容,金額,収支";
				sw.WriteLine(columnName);
				sw.Close();
			}
		}

		private void RefreshKakeiboList()
		{

		}
	}
}
