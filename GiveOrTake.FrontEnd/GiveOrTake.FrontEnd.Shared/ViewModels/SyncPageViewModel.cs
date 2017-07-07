using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GiveOrTake.FrontEnd.Shared.ViewModels
{
	class SyncPageViewModel : BaseViewModel
	{
		private string busyMessage;
		private bool canSync = true;

		public string BusyMessage
		{
			get { return busyMessage; }
			set { SetProperty(ref busyMessage, value); }
		}

		public Command SyncDataCommand { get; private set; }

		async Task SyncDataAsync()
		{
			if (IsBusy) return;

			CanSyncData(false);
			IsBusy = true;

			try
			{
				//Get transactions from ApplicationDbContext
				var transactions = await DataStore.GetTransactionsAsync();

				var count = transactions.Count;
				//TODO: Send our transactions by looping over each transaction.
				for (int i = 0; i < count; ++i)
				{
					await Server.SendTransactionAsync(transactions[i]);
					BusyMessage = $"Sent {i} of {count} transactions...";
				}

				//TODO: Get transactions from server
				BusyMessage = "Resolving conflicts and retrieving data...";
				var serverData = Server.GetTransactionsAsync();

				BusyMessage = "Saving transactions to local database";
				await DataStore.ResolveConflictsAndSaveTransactionsAsync(serverData);
			}
			catch (Exception e)
			{
				BusyMessage = "An error occured while syncing data...";
				await Task.Delay(5000);
			}
			finally
			{
				CanSyncData(true);
				IsBusy = false;
			}
		}

		void CanSyncData(bool value)
		{
			canSync = value;
			SyncDataCommand.ChangeCanExecute();
		}

		public SyncPageViewModel()
		{
			SyncDataCommand = new Command(async () => await SyncDataAsync(), () => canSync);
			BusyMessage = "Syncing data...";
		}
	}
}
