using GiveOrTake.FrontEnd.Shared.Helpers;
using GiveOrTake.FrontEnd.Shared.Views;
using System.Collections.Generic;

namespace GiveOrTake.FrontEnd.Shared.ViewModels
{
	public class MenuPageViewModel : BaseViewModel
	{
		private ObservableRangeCollection<MenuItem> menuItems;
		public ObservableRangeCollection<MenuItem> MenuItems => menuItems;

		public MenuPageViewModel()
		{
			menuItems = new ObservableRangeCollection<MenuItem>();

			menuItems.AddRange(new List<MenuItem> {
				new MenuItem {
					Label = "Sync",
					Icon = "\uE895",
					Page = new SyncPage()
				},
				new MenuItem {
					Label = "New Transaction",
					Icon = "\uE710",
					Page = new AddOrEditTransactionPage(null)
				},
				new MenuItem {
					Label = "All Transactions",
					Icon = "\uE8AB",
					Page = new TransactionsPage()
				},
				new MenuItem {
					Label = "My Items",
					Icon = "\uE8BC",
					Page = new ItemsPage()
				},
				new MenuItem {
					Label = "About",
					Icon = "\uE716",
					Page = new AboutPage()
				}
			});
		}
	}
}
