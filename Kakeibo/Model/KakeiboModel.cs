using Kakeibo.ViewModel;
using System;

namespace Kakeibo.Model
{
	public class KakeiboModel : ViewModelBase
	{
		private string m_meimoku;

		public string Meimoku
		{
			get { return this.m_meimoku; }
			set
			{
				this.m_meimoku = value;
				this.OnPropertyChanged("Meimoku");
			}
		}

		private DateTime? m_date;

		public DateTime? Date
		{ get { return this.m_date; }
			set
			{
				this.m_date = value;
				this.OnPropertyChanged("Date");
			}
		}

		private int? m_kingaku;

		public int? Kingaku
		{
			get { return this.m_kingaku; }
			set
			{
				this.m_kingaku = value;
				this.OnPropertyChanged("Kingaku");
			}
		}

		private string m_shushi;

		public string Shushi
		{
			get { return this.m_shushi; }
			set
			{
				this.m_shushi = value;
				this.OnPropertyChanged("Shushi");
			}
		}

	}
}
