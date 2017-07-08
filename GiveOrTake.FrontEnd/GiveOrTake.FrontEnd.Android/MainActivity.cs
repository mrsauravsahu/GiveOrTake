using Android.App;
using Android.Content.PM;
using Android.OS;
using System.IO;
using Xamarin.Forms;
using Plugin.Toasts;

namespace GiveOrTake.FrontEnd.Droid
{
	[Activity(Label = "@string/app_name", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize, ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			var localFolderPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "gt2.db");

			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;
			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			DependencyService.Register<ToastNotification>();
			ToastNotification.Init(this, new PlatformOptions() { SmallIconDrawable = Android.Resource.Drawable.IcDialogInfo });

			LoadApplication(new GiveOrTake.FrontEnd.Shared.App(localFolderPath));
		}
	}
}