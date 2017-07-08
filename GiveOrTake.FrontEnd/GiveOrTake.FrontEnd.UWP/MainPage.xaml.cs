using Plugin.Toasts.UWP;
using Syncfusion.SfChart.XForms.UWP;
using Xamarin.Forms;

namespace GiveOrTake.FrontEnd.UWP
{
	public sealed partial class MainPage
	{
		public MainPage()
		{
			this.InitializeComponent();
			new SfChartRenderer();

			DependencyService.Register<ToastNotification>();
			ToastNotification.Init();
			
			LoadApplication(new GiveOrTake.FrontEnd.Shared.App(App.LocalFolderPath));
		}
	}
}