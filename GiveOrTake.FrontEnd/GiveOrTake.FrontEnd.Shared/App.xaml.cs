using GiveOrTake.FrontEnd.Shared.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
