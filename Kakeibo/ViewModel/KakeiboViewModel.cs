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

			this.RegColumnCommand = new DelegateCommand(() => this.RegColumn());
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

		public ObservableCollection<KakeiboModel> KakeiboList { get; set; }

		public ObservableCollection<string> ShushiList { get; set; }

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

		private void InitializeKakeiboList()
		{
			this.KakeiboList = new ObservableCollection<KakeiboModel>();
			this.ShushiList = new ObservableCollection<string> { "収入", "支出", };
		}

		private void WriteCSV()
		{
			var lines = this.Kakeibo.Date.ToString() + "," + this.Kakeibo.Meimoku + "," + this.Kakeibo.Kingaku.ToString();
			var sw = new StreamWriter(@".\Kakeibo.csv", true);
			sw.WriteLine(lines);
			sw.Close();
		}

		private void ReadCSV()
		{
			try
			{
				var lines = File.ReadAllLines(@".\Kakeibo.csv", Encoding.GetEncoding("utf-8"));
				foreach (var line in lines)
				{
					var item = line.Split(',');
					var KakeiboItem = new KakeiboModel
					{
						Date = DateTime.Parse(item[0]),
						Meimoku = item[1],
						Kingaku = int.Parse(item[2]),
					};

					this.KakeiboList.Add(KakeiboItem);
					this.OnPropertyChanged("KakeiboList");
				}
			}
			catch (Exception)
			{
				var sw = new StreamWriter(@".\Kakeibo.csv");
				sw.Close();
			}
		}
	}
}
