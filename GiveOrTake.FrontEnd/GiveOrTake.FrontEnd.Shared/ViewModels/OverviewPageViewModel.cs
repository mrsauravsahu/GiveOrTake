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
		public ObservableCollection<DataPoint> Statistics { get; set; }

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


		public async Task LoadUserDetails()
		{
			if (IsBusy == true) return;

			IsBusy = true;
			BusyMessage = "Loading your transactions...";

			await Task.Run(async () =>
			{
				this.User = await DataStore.GetUserDetails();

				this.Statistics[0].Value = (from u in User.Transaction
											where u.TransactionType == TransactionType.Give
											select u).Count();
				this.Statistics[1].Value = (from u in User.Transaction
											where u.TransactionType == TransactionType.Take
											select u).Count();
				ShowNoTransactions = (user.Transaction.Count == 0);
			});

			IsBusy = false;
			BusyMessage = string.Empty;
		}

		private string busyMessage;

		public string BusyMessage
		{
			get { return busyMessage; }
			set { SetProperty(ref busyMessage, value); }
		}


		public OverviewPageViewModel()
		{
			Statistics = new Helpers.ObservableRangeCollection<DataPoint>()
			{
				new DataPoint{Label="Lent", Value=2},
				new DataPoint{Label="Borrowed", Value=3}
			};
			User = new User();
			ShowNoTransactions = false;
		}
	}
}
