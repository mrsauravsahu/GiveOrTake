using GiveOrTake.FrontEnd.Shared.Helpers;
using GiveOrTake.FrontEnd.Shared.Models;
using GiveOrTake.FrontEnd.Shared.Services;

using Xamarin.Forms;

namespace GiveOrTake.FrontEnd.Shared.ViewModels
{
	public class BaseViewModel : ObservableObject
	{
		public DataStore DataStore => DependencyService.Get<DataStore>();

		bool isBusy = false;
		public bool IsBusy
		{
			get { return isBusy; }
			set
			{
				SetProperty(ref isBusy, value);
				SetProperty(ref isFree, !(bool)value, nameof(IsFree));
			}
		}

		bool isFree = true;
		public bool IsFree => isFree;
		/// <summary>
		/// Private backing field to hold the title
		/// </summary>
		string title = string.Empty;
		/// <summary>
		/// Public property to set and get the title of the item
		/// </summary>
		public string Title
		{
			get { return title; }
			set { SetProperty(ref title, value); }
		}
	}
}

