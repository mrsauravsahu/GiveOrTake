using GiveOrTake.Database;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GiveOrTake.FrontEnd.Shared.ViewModels
{
	class AddOrEditTransactionPageViewModel : BaseViewModel
	{
		private Transaction transaction;
		public Transaction Transaction
		{
			get { return transaction; }
			set { SetProperty(ref transaction, value); }
		}

		public AddOrEditTransactionPageViewModel(Transaction t)
		{
			Transaction = (t is null) ? new Transaction { } : t;
		}
	}
}
