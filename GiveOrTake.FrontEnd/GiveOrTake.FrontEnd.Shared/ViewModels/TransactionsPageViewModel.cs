using GiveOrTake.Database;
using GiveOrTake.FrontEnd.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GiveOrTake.FrontEnd.Shared.ViewModels
{
	class TransactionsPageViewModel : BaseViewModel
	{
		public ObservableRangeCollection<Transaction> Transactions { get; set; }
		public Command LoadTransactionsCommand { get; set; }

		public TransactionsPageViewModel()
		{
			Title = "All Transactions";
			Transactions = new ObservableRangeCollection<Transaction>();
			LoadTransactionsCommand = new Command(async () => await ExecuteLoadTransactionsCommand());

		}
			public async Task ExecuteLoadTransactionsCommand()
		{
			if (IsBusy)
				return;

			IsBusy = true;

			try
			{
				Transactions.Clear();
				var transactions = await DataStore.GetTransactionsAsync();
				Transactions.ReplaceRange(transactions);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				MessagingCenter.Send(new MessagingCenterAlert
				{
					Title = "Error",
					Message = "Unable to load items.",
					Cancel = "OK"
				}, "message");
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}
