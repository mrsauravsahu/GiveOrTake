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
		public static string LocalFolderPath { get; set; }
		public static NavigationPage NavigationPage { get; set; }
		public static RootPage RootPage { get; set; }
		public static MenuPage MenuPage { get; set; }

		public App(string localFolderPath)
		{
			InitializeComponent();

			LocalFolderPath = localFolderPath;

			var dataStore = DependencyService.Get<DataStore>();
			var isLoggedIn = dataStore.IsLoggedIn();

			NavigationPage = new NavigationPage(new OverviewPage());

			MenuPage = new MenuPage();
			RootPage = new RootPage
			{
				Master = MenuPage,
				Detail = NavigationPage
			};
			MainPage = RootPage;
		}
	}
}
