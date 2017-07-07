using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using GiveOrTake.FrontEnd.Shared.Views;
using GiveOrTake.FrontEnd.Droid;
using Xamarin.Forms.Platform.Android;
using Xamarin.Auth;
using GiveOrTake.FrontEnd.Shared.Helpers;

[assembly: ExportRenderer(typeof(FacebookLoginPage), typeof(FacebookLoginPageRenderer))]
namespace GiveOrTake.FrontEnd.Droid
{
	public class FacebookLoginPageRenderer : PageRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
		{
			base.OnElementChanged(e);

			// this is a ViewGroup - so should be able to load an AXML file and FindView<>
			var activity = this.Context as Activity;

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
						//App.SaveToken(eventArgs.Account.Properties["access_token"]);
						Settings.FacebookAccessToken = eventArgs.Account.Properties["access_token"];
						await Xamarin.Forms.Application.Current.MainPage.Navigation.PopModalAsync();
					}
					else
					{
						// The user cancelled
					}
				};

			activity.StartActivity(auth.GetUI(activity));
		}
	}
}