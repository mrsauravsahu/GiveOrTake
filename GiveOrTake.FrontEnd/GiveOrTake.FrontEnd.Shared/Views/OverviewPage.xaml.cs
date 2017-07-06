using GiveOrTake.FrontEnd.Shared.Helpers;
using GiveOrTake.FrontEnd.Shared.ViewModels;
using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GiveOrTake.FrontEnd.Shared.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OverviewPage : ContentPage
	{
		private OverviewPageViewModel viewModel;

		public OverviewPage()
		{
			InitializeComponent();
			BindingContext = viewModel = new OverviewPageViewModel();

			Binding binding = new Binding("Statistics");
			doughnutSeries.SetBinding(ChartSeries.ItemsSourceProperty, binding);
		}
		protected override async void OnAppearing()
		{
			base.OnAppearing();
			await viewModel.LoadUserDetailsAsync();
		}
	}
}