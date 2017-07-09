using GiveOrTake.Database;
using GiveOrTake.FrontEnd.Shared.Models;
using GiveOrTake.FrontEnd.Shared.Services;
using GiveOrTake.FrontEnd.Shared.ViewModels;
using Plugin.Toasts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		private IUserDialog UserDialog => DependencyService.Get<IUserDialog>();
		bool count = false;
		IToastNotificator notificator = DependencyService.Get<IToastNotificator>();

		public AddOrEditTransactionPage(Transaction t)
		{
			InitializeComponent();
			EmptyList = new List<Database.Item>();
			Items = new List<Database.Item>();

			if (t is null)
			{
				Title = "Add Transaction";
				viewModel = new AddOrEditTransactionPageViewModel(new Transaction
				{
					Name = string.Empty,
					OccurrenceDate = DateTime.Now,
					ExpectedCompletionDate = DateTime.Now,
					Description = string.Empty
				});
			}
			else
			{
				Title = "Edit Transaction";
				viewModel = new AddOrEditTransactionPageViewModel(t);
				setupPage();
			}
			BindingContext = viewModel;
		}

		private void setupPage()
		{
			this.ItemSearchBar.Text = viewModel.Transaction.Item.Name;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			this.Items = await DependencyService.Get<DataStore>().GetItemsAsync();
			FilteredItemsListView.ItemsSource = EmptyList;
			setTransactionTypeLabel(false);
			updateListView(0);
		}

		public void UpdateFilteredItemsCollectionCommand(object sender, TextChangedEventArgs e)
		{
			if (Items is null) return;
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
		{ setTransactionTypeLabel(e.Value); }

		private void setTransactionTypeLabel(bool value)
		{
			TransactionTypeTextLabel.Text = value ? "Borrowed" : "Lent";
			viewModel.Transaction.TransactionType = value ? Take : Give;
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
			count = false;
		}

		public async void Save_Clicked(object sender, EventArgs e)
		{
			if (validate())
			{
				viewModel.IsBusy = true;
				await DependencyService.Get<DataStore>().AddTransactionAsync(viewModel.Transaction, ItemSearchBar.Text.Trim());
				viewModel.IsBusy = false;

				if (ExpectedCompletionDateDatePicker.Date + NotificationTimePicker.Time > DateTime.Now)
				{
					await notificator.Notify(new NotificationOptions
					{
						DelayUntil = ExpectedCompletionDateDatePicker.Date + NotificationTimePicker.Time,
						Title = "Ping",
						Description = string.Format("Don't forget, you had {0} a {1}. Time to {2} it back.",
						   TransactionTypeSwitch.IsToggled ? ("borrowed") : ("lent"),
						   ItemSearchBar.Text,
						   TransactionTypeSwitch.IsToggled ? ("give") : ("get")),
						IsClickable = true
					});

					Debug.WriteLine($"Notification set for {ExpectedCompletionDateDatePicker.Date + NotificationTimePicker.Time}");
				}

				cleanupPage();
				await Navigation.PopToRootAsync(true);
			}
		}

		private bool validate()
		{
			bool showAlert(string message)
			{
				UserDialog.Show(message, "Alert", TimeSpan.FromSeconds(5));
				return false;
			}

			if (String.IsNullOrWhiteSpace(NameEntry.Text))
				return showAlert("Transaction Name cannot be empty.");
			if (String.IsNullOrWhiteSpace(ItemSearchBar.Text))
				return showAlert("Item cannot be empty.");
			if (String.IsNullOrWhiteSpace(DescriptionEditor.Text))
				return showAlert("Description cannot be empty");
			if (ExpectedCompletionDateDatePicker.Date + NotificationTimePicker.Time < DateTime.Now)
				if (count) return true;
				else return showAlert("Notifications won't be provided if expected completion date is in the past.");
			return true;
		}
	}
}