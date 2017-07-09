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
		(loadUserDetailsCommand ?? (loadUserDetailsCommand = new Command(async () => await LoadUserDetailsAsync())));

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

		private ObservableCollection<Transaction> upcomingTransactions;

		public ObservableCollection<Transaction> UpcomingTransactions
		{
			get { return upcomingTransactions; }
			set { SetProperty(ref upcomingTransactions, value); }
		}


		public async Task LoadUserDetailsAsync()
		{
			if (IsBusy == true) return;

			IsBusy = true;
			BusyMessage = "Loading your transactions...";

			ShowNoTransactions = false;
			ShowTransactions = false;

			try
			{
				this.User = await DataStore.GetUserDetails();

				var givenCount = (from u in User.Transaction
								  where u.TransactionType == TransactionType.Give
								  select u).Count();
				var takenCount = (from u in User.Transaction
								  where u.TransactionType == TransactionType.Take
								  select u).Count();

				this.Statistics.Clear();
				this.Statistics.Add(
					new DataPoint
					{
						Label = "Lent",
						Value = givenCount
					});
				this.Statistics.Add(
					new DataPoint
					{
						Label = "Borrowed",
						Value = takenCount
					});

				UpcomingTransactions.Clear();

				User.Transaction
					.Where(t => t.IsComplete == false)
					.OrderBy(p => p.ExpectedCompletionDate)
					.Take(3)
					.ToList()
					.ForEach(p => UpcomingTransactions.Add(p));

				ShowNoTransactions = (UpcomingTransactions.Count == 0);
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
			upcomingTransactions = new ObservableCollection<Transaction>();
			Statistics = new ObservableCollection<DataPoint>()
			{
				new DataPoint { Label = "Lent", Value = 0 },
				new DataPoint { Label = "Borrowed", Value = 0 }
			};
			ShowNoTransactions = false;
			ShowTransactions = false;
		}
	}
}
