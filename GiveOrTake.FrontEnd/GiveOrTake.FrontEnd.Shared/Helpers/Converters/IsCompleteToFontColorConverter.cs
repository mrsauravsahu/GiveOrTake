using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace GiveOrTake.FrontEnd.Shared.Helpers.Converters
{
	class IsCompleteToFontColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var isComplete = (bool)value;
			return isComplete ? new Color(.75, .75, .75) : new Color(.3, .3, .3);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
