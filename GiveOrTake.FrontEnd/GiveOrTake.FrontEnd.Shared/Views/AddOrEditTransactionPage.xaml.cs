using GiveOrTake.Database;
using GiveOrTake.FrontEnd.Shared.Models;
using GiveOrTake.FrontEnd.Shared.Services;
using GiveOrTake.FrontEnd.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using static GiveOrTake.Database.TransactionType;
namespace GiveOrTake.FrontEnd.Shared.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddOrEditTransactionPage : ContentPage
	{
		private AddOrEditTransactionPageViewModel viewModel;
		private List<Database.Item> Items;
		private List<Database.Item> EmptyList;

		public AddOrEditTransactionPage(Transaction t)
		{
			InitializeComponent();
			EmptyList = new List<Database.Item>();
			if (t is null)
			{
				Title = "Add Transaction";
				viewModel = new AddOrEditTransactionPageViewModel(new Transaction { });
			}
			else
			{
				Title = "Edit Transaction";
				viewModel = new AddOrEditTransactionPageViewModel(t);
			}
			BindingContext = viewModel;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			this.Items = await DependencyService.Get<DataStore>().GetItemsAsync();
			FilteredItemsListView.ItemsSource = EmptyList;
			updateListView(0);
		}

		public void UpdateFilteredItemsCollectionCommand(object sender, TextChangedEventArgs e)
		{
			IList<Database.Item> source;
			if (string.IsNullOrWhiteSpace(e.NewTextValue))
			{ source = Items; }
			else
			{ source = Items.Where(p => p.Name.ToUpper().Contains(e.NewTextValue.ToUpper())).ToList(); }

			FilteredItemsListView.ItemsSource = source;
			updateListView(source.Count);
		}

		private void updateListView(int count)
		{
			FilteredItemsListView.IsVisible = count > 0;
			FilteredItemsListView.HeightRequest = count * FilteredItemsListView.RowHeight;
		}

		public void OnItemSelect(object sender, SelectedItemChangedEventArgs e)
		{
			ItemSearchBar.Text = (e.SelectedItem as Database.Item).Name;
			updateListView(0);
		}

		public void OnTransactionTypeToggled(object sender, ToggledEventArgs e)
		{
			TransactionTypeTextLabel.Text = e.Value ? "Borrowed" : "Lent";
			viewModel.Transaction.TransactionType = e.Value ? Take : Give;
		}

		public async void Cancel_Clicked(object sender, EventArgs e)
		{
			cleanupPage();

			await Navigation.PopAsync();
		}

		private void cleanupPage()
		{
			NameEntry.Text = string.Empty;
			DescriptionEditor.Text = string.Empty;
			OccurrenceDataDatePicker.Date = DateTime.Now;
			ExpectedCompletionDateDatePicker.Date = DateTime.Now;
			TransactionTypeSwitch.IsToggled = false;
			ItemSearchBar.Text = string.Empty;
		}

		public async void Save_Clicked(object sender, EventArgs e)
		{
			viewModel.IsBusy = true;
			await DependencyService.Get<DataStore>().AddTransactionAsync(viewModel.Transaction, ItemSearchBar.Text.Trim());
			viewModel.IsBusy = false;

			cleanupPage();
			await Navigation.PopAsync();
		}
	}
}