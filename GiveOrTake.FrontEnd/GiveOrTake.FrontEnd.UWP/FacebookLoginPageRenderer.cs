using GiveOrTake.FrontEnd.Shared.Views;
using GiveOrTake.FrontEnd.UWP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.UWP;
using Xamarin.Forms;
using Xamarin.Auth;
using GiveOrTake.FrontEnd.Shared.Helpers;

[assembly: ExportRenderer(typeof(FacebookLoginPage), typeof(FacebookLoginPageRenderer))]
namespace GiveOrTake.FrontEnd.UWP
{
	public class FacebookLoginPageRenderer : PageRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
		{
			base.OnElementChanged(e);

			var auth = new OAuth2Authenticator(
				clientId: "207552723093297", // your OAuth2 client id
				scope: "public_profile,email", // the scopes for the particular API you're accessing, delimited by "+" symbols
				authorizeUrl: new Uri("https://graph.facebook.com/oauth/authorize"), // the auth URL for the service
				redirectUrl: new Uri("https://giveortakebackend.azurewebsites.net")); // the redirect URL for the service

			auth.Completed += async (sender, eventArgs) =>
			{
				if (eventArgs.IsAuthenticated)
				{
					//App.SuccessfulLoginAction.Invoke();
					// Use eventArgs.Account to do wonderful things
					Settings.FacebookAccessToken = eventArgs.Account.Properties["access_token"];
					await GiveOrTake.FrontEnd.Shared.App.Current.MainPage.Navigation.PopModalAsync();
				}
				else
				{
					// The user cancelled
				}
			};

			WindowsPage windowsPage = new WindowsPage();
			var _frame = windowsPage.Frame;
			if (_frame == null)
			{
				_frame = new Windows.UI.Xaml.Controls.Frame
				{
					Language = global::Windows.Globalization.ApplicationLanguages.Languages[0]
				};
				windowsPage.Content = _frame;
				SetNativeControl(windowsPage);
			}

			Type pageType = auth.GetUI();
			_frame.Navigate(pageType, auth);
		}
	}
}
