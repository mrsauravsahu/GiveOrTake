using Syncfusion.SfChart.XForms.UWP;

namespace GiveOrTake.FrontEnd.UWP
{
	public sealed partial class MainPage
	{
		public MainPage()
		{
			this.InitializeComponent();
			new SfChartRenderer();
			LoadApplication(new GiveOrTake.FrontEnd.Shared.App(App.DatabasePath));
		}
	}
}