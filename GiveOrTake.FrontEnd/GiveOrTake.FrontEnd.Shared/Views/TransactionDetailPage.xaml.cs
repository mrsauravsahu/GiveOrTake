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
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			if (this.transaction.IsComplete)
			{
				var deleteToolbarItem = new ToolbarItem
				{ Text = "Delete" };

				deleteToolbarItem.Clicked += async (s, e) =>
				{
					await DependencyService.Get<DataStore>().DeleteTransactionAsync(this.transaction.TransactionId);
					await Navigation.PopToRootAsync(true);
				};
				this.ToolbarItems.Add(deleteToolbarItem);
			}
			else
			{
				var completeToolbarItem = new ToolbarItem { Text = "Complete" };
				completeToolbarItem.Clicked += async (s, e) =>
				{
					await DependencyService.Get<DataStore>().SetTransactionCompleteAsync(this.transaction.TransactionId);
					await Navigation.PopToRootAsync(true);
				};
				this.ToolbarItems.Add(completeToolbarItem);
			}
		}
	}
}