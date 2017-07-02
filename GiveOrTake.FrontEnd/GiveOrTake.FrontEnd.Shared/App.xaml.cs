using GiveOrTake.Database;
using GiveOrTake.FrontEnd.Shared.Views;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using GiveOrTake.FrontEnd.Shared.Data;
using System.Linq;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace GiveOrTake.FrontEnd.Shared
{
	public partial class App : Application
	{
		public static string DatabasePath { get; private set; }
		public static ObservableCollection<Person> People { get; private set; }
		public static NavigationPage NavigationPage { get; private set; }
		public static RootPage RootPage { get; private set; }

		public App(string databasePath)
		{
			InitializeComponent();

			DatabasePath = databasePath;
			getData();

			NavigationPage = new NavigationPage(new OverviewPage());
			RootPage = new RootPage
			{
				Master = new MenuPage(),
				Detail = NavigationPage
			};
			MainPage = RootPage;
		}

		private void getData()
		{
			using (var context = new ApplicationDbContext(App.DatabasePath))
			{
				context.Database.EnsureCreated();

				if (context.People.Count() == 0)
					context.People.AddRange(
						new Person[]
						{
						new Person{Name ="User 1"},
						new Person{Name ="User 2"},
						new Person{Name ="User 3"}
						});
				context.SaveChanges();
			}
			using (var context = new ApplicationDbContext(App.DatabasePath))
			{
				var items = context.People;
				App.People = new ObservableCollection<Person>();
				items.ToList().ForEach(p => App.People.Add(p));
			}
		}
	}
}
