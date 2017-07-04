using GiveOrTake.Database;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace GiveOrTake.FrontEnd.Shared.Helpers.Converters
{
	class UserTransactionsCountToVisibilityReverseConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is ObservableRangeCollection<Transaction> t)
				return (t.Count == 0);
			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
