using GiveOrTake.Database;
using GiveOrTake.FrontEnd.Shared.Views;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using Plugin.Toasts;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace GiveOrTake.FrontEnd.Shared
{
	public partial class App : Application
	{
		public static string DatabasePath { get; private set; }
		public static NavigationPage NavigationPage { get; private set; }
		public static RootPage RootPage { get; private set; }

		public App(string databasePath)
		{
			InitializeComponent();

			DatabasePath = databasePath;
			NavigationPage = new NavigationPage(new OverviewPage());
			RootPage = new RootPage
			{
				Master = new MenuPage(),
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
