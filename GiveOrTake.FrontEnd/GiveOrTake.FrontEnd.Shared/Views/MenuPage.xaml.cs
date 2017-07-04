using GiveOrTake.FrontEnd.Shared.Helpers;
using GiveOrTake.FrontEnd.Shared.ViewModels;
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
	public partial class MenuPage : ContentPage
	{
		private MenuPageViewModel viewModel;

		public MenuPage()
		{
			Title = Constants.AppTitle;
			BindingContext = viewModel = new MenuPageViewModel();
			InitializeComponent();
		}

		private void OnMenuItemSelect(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem is Helpers.MenuItem item && !(item is null))
			{
				App.RootPage.IsPresented = false;
				App.NavigationPage.PushAsync(item.Page);
				if (sender is ListView listView) { listView.SelectedItem = null; }
			}
		}
	}
}