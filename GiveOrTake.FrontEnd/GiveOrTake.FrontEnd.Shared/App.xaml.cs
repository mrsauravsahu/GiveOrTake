using GiveOrTake.FrontEnd.Shared.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using Plugin.Toasts;
using GiveOrTake.FrontEnd.Shared.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace GiveOrTake.FrontEnd.Shared
{
	public partial class App : Application
	{
		public static string DatabasePath { get; set; }
		public static NavigationPage NavigationPage { get; set; }
		public static RootPage RootPage { get; set; }
		public static MenuPage MenuPage { get; set; }

		public App(string databasePath)
		{
			InitializeComponent();

			DatabasePath = databasePath;

			var dataStore = DependencyService.Get<DataStore>();
			var isLoggedIn = dataStore.IsLoggedIn();

			if (isLoggedIn == false)
				NavigationPage = new NavigationPage(new LoginPage());
			else
				NavigationPage = new NavigationPage(new OverviewPage());

			MenuPage = new MenuPage();
			RootPage = new RootPage
			{
				Master = MenuPage,
				Detail = NavigationPage
			};
			MainPage = RootPage;
		}

		protected override async void OnSleep()
		{
			var notificator = DependencyService.Get<IToastNotificator>();

			var result = await notificator.Notify(new NotificationOptions()
			{
				Title = "Hello!",
				Description = "Check back in!",
				DelayUntil = DateTime.UtcNow + TimeSpan.FromSeconds(10)
			});
		}
	}
}
