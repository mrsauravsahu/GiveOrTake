using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace GiveOrTake.FrontEnd.Shared.ViewModels
{
	public class DataPoint
	{
		public string Label { get; set; }
		public double Value { get; set; }
	}
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
