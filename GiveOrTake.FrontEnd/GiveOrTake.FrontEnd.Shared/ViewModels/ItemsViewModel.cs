using System;
using System.Diagnostics;
using System.Threading.Tasks;

using GiveOrTake.FrontEnd.Shared.Helpers;
using GiveOrTake.FrontEnd.Shared.Views;

using Acr.UserDialogs;
using Xamarin.Forms;
using GiveOrTake.Database;
using GiveOrTake.FrontEnd.Shared.Services;

namespace GiveOrTake.FrontEnd.Shared.ViewModels
{
	public class ItemsViewModel : BaseViewModel
	{
		public ObservableRangeCollection<Item> Items { get; set; }
		public Command LoadItemsCommand { get; set; }
		public Command InfoItemCommand { get; set; }
		private IUserDialog UserDialog => DependencyService.Get<IUserDialog>();

		public ItemsViewModel()
		{
			Title = "My Items";
			Items = new ObservableRangeCollection<Item>();
			LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
			InfoItemCommand = new Command(async (parameter) => await ExecuteInfoItemCommand((parameter as Item)));
		}

		public async Task ExecuteInfoItemCommand(Item item)
		{
			await UserDialogs.Instance.AlertAsync(new AlertConfig
			{
				Message = item.Transaction.Count == 0 ?
					"This item is not referenced by any transactions." :
					(item.Transaction.Count == 1) ? "This item is referenced by 1 transaction." : $"This item is referenced by {item.Transaction.Count} transactions.",
				OkText = "Close",
				Title = "Info"
			});
		}

		public async Task ExecuteLoadItemsCommand()
		{
			if (IsBusy)
				return;

			IsBusy = true;

			try
			{
				Items.Clear();
				var items = await DataStore.GetItemsAsync();
				Items.ReplaceRange(items);
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