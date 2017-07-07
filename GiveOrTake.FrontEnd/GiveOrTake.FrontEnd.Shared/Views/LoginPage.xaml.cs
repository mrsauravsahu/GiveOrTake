using GiveOrTake.FrontEnd.Shared.Helpers;
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
			await App.Current.MainPage.Navigation.PopModalAsync(true);
		}

		public async void OnFacebookClick(object sender, EventArgs e)
		{
			if (Settings.FacebookAccessToken == string.Empty)
				await App.Current.MainPage.Navigation.PushModalAsync(new FacebookLoginPage());
		}
	}
}