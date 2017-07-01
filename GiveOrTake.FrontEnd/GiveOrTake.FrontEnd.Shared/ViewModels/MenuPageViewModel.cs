using System.Collections.Generic;

namespace GiveOrTake.FrontEnd.Shared.ViewModels
{
	class MenuPageViewModel
	{
		private List<string> menuItems = new List<string>
		{
			"New Transcation",
			"All Transactions",
			"My Items",
			"About"
		};

		public List<string> MenuItems => menuItems;
	}
}
