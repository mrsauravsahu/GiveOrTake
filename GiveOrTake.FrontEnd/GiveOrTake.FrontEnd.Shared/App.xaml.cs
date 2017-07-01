using GiveOrTake.FrontEnd.Shared.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace GiveOrTake.FrontEnd.Shared
{
	public partial class App : Application
	{
		public NavigationPage NavigationPage { get; private set; }
		public App()
		{
			InitializeComponent();

			NavigationPage = new NavigationPage(new OverviewPage());
			MainPage = new RootPage
			{
				Master = new MenuPage(),
				Detail = NavigationPage
			};
		}
	}
}
