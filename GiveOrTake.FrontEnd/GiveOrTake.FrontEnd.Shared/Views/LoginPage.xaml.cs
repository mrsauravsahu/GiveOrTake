using GiveOrTake.FrontEnd.Shared.Services;
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
	public partial class LoginPage : ContentPage
	{
		private LoginPageViewModel viewModel;

		public LoginPage()
		{
			InitializeComponent();
			BindingContext = viewModel = new LoginPageViewModel();
			NavigationPage.SetHasNavigationBar(this, false);
		}

		public async void OnSignInLaterButtonClick(object sender, EventArgs e)
		{
			this.viewModel.IsBusy = true;

			await DependencyService.Get<DataStore>().CreateUserAsync();
			var otherPage = new OverviewPage();
			var homePage = App.NavigationPage.Navigation.NavigationStack.First();
			App.NavigationPage.Navigation.InsertPageBefore(otherPage, homePage);
			await App.NavigationPage.PopToRootAsync(true);

			this.viewModel.IsBusy = false;
		}
	}
}