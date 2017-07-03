using GiveOrTake.Database;
using GiveOrTake.FrontEnd.Shared.Data;
using GiveOrTake.FrontEnd.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace GiveOrTake.FrontEnd.Shared.ViewModels
{
	class OverviewPageViewModel
	{
		public ObservableCollection<DataPoint> Data { get; set; }
		public OverviewPageViewModel()
		{
			Data = new ObservableCollection<DataPoint>
			{
				new DataPoint { Label = "Lent", Value = 3 },
				new DataPoint { Label = "Borrowed", Value = 4 }
			};
		}
	}
}
