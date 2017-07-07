using GiveOrTake.FrontEnd.Shared.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GiveOrTake.FrontEnd.Shared.ViewModels
{
	class LoginPageViewModel : BaseViewModel
	{
		private string busyMessage;

		public string BusyMessage
		{
			get { return busyMessage; }
			set { SetProperty(ref busyMessage, value); }
		}

		public Command LoginWithCredentialsCommand { get; private set; }
		public Command LoginWithFacebookCommand { get; private set; }
		public Command LoginLaterCommand { get; private set; }

		async Task LoginWithCredentialsAsync()
		{
			if (IsBusy) return;

			IsBusy = true;
			busyMessage = "Logging you in...";
			try
			{
				//TODO: Login using server
				await Task.Delay(3);
			}
			catch (Exception e)
			{
				BusyMessage = "An error occured while logging you in...";
			}
			finally
			{
				IsBusy = false;
			}
		}

		public LoginPageViewModel()
		{
			LoginWithCredentialsCommand = new Command(async () => await LoginWithCredentialsAsync());
			LoginWithFacebookCommand = new Command(async () => await LoginWithFacebookAsync());
			LoginLaterCommand = new Command(async () => await LoginLaterAsync());
			BusyMessage = "Logging you in...";
		}

		async Task LoginLaterAsync()
		{
			if (IsBusy) return;
			await App.Current.MainPage.Navigation.PopModalAsync();
		}

		async Task LoginWithFacebookAsync()
		{
			if (IsBusy) return;
			await App.Current.MainPage.Navigation.PushModalAsync(new FacebookLoginPage());
		}
	}
}
