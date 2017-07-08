using GiveOrTake.Database;
using GiveOrTake.FrontEnd.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GiveOrTake.FrontEnd.Shared.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TransactionDetailPage : ContentPage
	{
		private Transaction transaction;

		public TransactionDetailPage(Transaction transaction)
		{
			InitializeComponent();
			this.transaction = transaction;
			BindingContext = this.transaction;

			DeleteToolbarItem.Clicked += async (s, e) =>
			{
				await DependencyService.Get<DataStore>().DeleteTransactionAsync(this.transaction.TransactionId);
				await Navigation.PopAsync();
			};
			
			CompleteToolbarItem.Clicked += async (s, e) =>
			{
				await DependencyService.Get<DataStore>().SetTransactionCompleteAsync(this.transaction.TransactionId);
				await Navigation.PopAsync();
			};
		}
	}
}