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

		private async void loginButtonClicked(object sender, EventArgs e)
		{
			App.RootPage.IsPresented = false;
			var loginPage = new LoginPage();
			var homePage = App.NavigationPage.Navigation.NavigationStack.First();
			App.NavigationPage.Navigation.InsertPageBefore(loginPage, homePage);
			await App.NavigationPage.PopToRootAsync(false);
		}
	}
}