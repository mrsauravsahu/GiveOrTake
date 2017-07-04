using GiveOrTake.Database;
using GiveOrTake.FrontEnd.Shared.Data;
using GiveOrTake.FrontEnd.Shared.Helpers;
using GiveOrTake.FrontEnd.Shared.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GiveOrTake.FrontEnd.Shared.ViewModels
{
	class OverviewPageViewModel : BaseViewModel
	{
		public ObservableRangeCollection<DataPoint> Statistics { get; set; }

		public User user;
		public User User
		{
			get { return user; }
			set { SetProperty(ref user, value); }
		}

		private Command loadUserDetailsCommand;
		public Command LoadUserDetailsCommand =>
		(loadUserDetailsCommand ?? (loadUserDetailsCommand = new Command(async () => await LoadUserDetails())));

		private bool showNoTransactions;

		public bool ShowNoTransactions
		{
			get { return showNoTransactions; }
			set { SetProperty(ref showNoTransactions, value); }
		}
		private bool showTransactions;

		public bool ShowTransactions
		{
			get { return showTransactions; }
			set { SetProperty(ref showTransactions, value); }
		}


		public async Task LoadUserDetails()
		{
			if (IsBusy == true) return;

			IsBusy = true;
			BusyMessage = "Loading your transactions...";

			ShowNoTransactions = false;
			ShowTransactions = false;

			try
			{
				this.User = await DataStore.GetUserDetails();

				this.Statistics.ReplaceRange(new List<DataPoint>
				{
					new DataPoint
					{
						Label = "Lent",
						Value = (from u in User.Transaction
								 where u.TransactionType == TransactionType.Give
								 select u).Count()
					},
					new DataPoint
					{
						Label = "Borrowed",
						Value = (from u in User.Transaction
								 where u.TransactionType == TransactionType.Take
								 select u).Count()
					}
				});

				ShowNoTransactions = (user.Transaction.Count == 0);
				ShowTransactions = !ShowNoTransactions;
			}
			catch (Exception e) { }
			finally
			{
				IsBusy = false;
				BusyMessage = string.Empty;
			}
		}

		private string busyMessage;

		public string BusyMessage
		{
			get { return busyMessage; }
			set { SetProperty(ref busyMessage, value); }
		}


		public OverviewPageViewModel()
		{
			User = new User();
			Statistics = new ObservableRangeCollection<DataPoint>();
			ShowNoTransactions = false;
			ShowTransactions = false;
		}
	}
}
