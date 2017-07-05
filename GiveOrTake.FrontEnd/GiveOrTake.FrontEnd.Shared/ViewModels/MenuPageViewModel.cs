using GiveOrTake.FrontEnd.Shared.Helpers;
using GiveOrTake.FrontEnd.Shared.Services;
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

			if (!Xamarin.Forms.DependencyService.Get<DataStore>().IsLoggedIn())
				menuItems.Add(new MenuItem
				{
					Label = "Login",
					Icon = "\uE8FA",
					Page = new LoginPage()
				});

			menuItems.AddRange(new List<MenuItem> {
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
					//TODO
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
