using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using Kakeibo.Model;
using Kakeibo.View;
using Prism.Commands;
using System.Windows;

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

		private readonly string ColumnName = "日付,内容,金額,収支";

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

		public ICommand OpenSelectMonthCommand { get; set; }

		public ICommand OutputMonthlyCsvCommand { get; set; }

		public ObservableCollection<KakeiboModel> KakeiboList { get; set; }

		public ObservableCollection<string> ShushiList { get; set; }

		public KakeiboModel SelectedKakeiboModel { get; set; }

		public DateTime SelectedMonth { get; set; }

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
			this.OpenSelectMonthCommand = new DelegateCommand(() => this.OpenSelectMonthDialog());
			this.OutputMonthlyCsvCommand = new DelegateCommand<object>(this.OutputMonthlyCsv);
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
				sw.WriteLine(this.ColumnName);
				sw.Close();
			}
		}

		private void OpenSelectMonthDialog()
		{
			this.SelectedMonth = DateTime.Now;
			var dialog = new SelectMonthlyDialog(this);
			dialog.Show();
		}

		private void OutputMonthlyCsv(object x)
		{

			if(x == null)
			{
				return;
			}
			var newOutputList = new List<KakeiboModel>();

			var sw = new StreamWriter(@".\Kakeibo_" + this.SelectedMonth.Year.ToString() + "年" + this.SelectedMonth.Month.ToString() + "月.csv", true);
			sw.WriteLine(this.ColumnName, Encoding.UTF8);
			foreach (var list in KakeiboList)
			{
				if (!list.Date.HasValue)
				{
					continue;
				}
				if (list.Date?.Year == this.SelectedMonth.Year && list.Date?.Month == this.SelectedMonth.Month)
				{
					newOutputList.Add(list);
					sw.WriteLine(list.Date.ToString() + "," + list.Meimoku + "," + list.Kingaku.ToString() + "," + list.Shushi);
				}
			}

			sw.WriteLine();
			sw.WriteLine(",合計," + this.Calc(newOutputList).ToString(), Encoding.GetEncoding("utf-8"));

			sw.Close();

			var window = (Window)x;
			window.Close();
		}

		private int Calc(IReadOnlyCollection<KakeiboModel> kakeiboModel)
		{
			var result = 0;
			foreach(var kakeibo in kakeiboModel)
			{
				if (kakeibo.Shushi == "収入")	result += kakeibo.Kingaku.Value;
				else result -= kakeibo.Kingaku.Value;
			}
			return result;
		}


		private void RefreshKakeiboList()
		{

		}
	}
}
