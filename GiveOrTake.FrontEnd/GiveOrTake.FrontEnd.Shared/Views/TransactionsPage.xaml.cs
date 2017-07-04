using GiveOrTake.FrontEnd.Shared.ViewModels;
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
	public partial class TransactionsPage : ContentPage
	{
		private TransactionsPageViewModel viewModel;

		public TransactionsPage()
		{
			InitializeComponent();
			BindingContext = viewModel = new TransactionsPageViewModel();
		}

		async void AddItem_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new NewItemPage());
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();
			await viewModel.ExecuteLoadTransactionsCommand();
		}
	}
}