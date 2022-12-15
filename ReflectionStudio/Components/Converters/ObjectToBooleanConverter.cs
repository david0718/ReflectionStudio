using System;
using System.Windows.Data;

namespace ReflectionStudio.Components.Converters
{
	/// <summary>
	/// Convert a non null object to a boolean, allow to enable context menu elements
	/// </summary>
	[ValueConversion(typeof(object), typeof(bool))]
	public class ObjectToBooleanConverter : IValueConverter
	{
		/// <summary>
		/// IValueConverter.Convert
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value != null)
				return true;
			else
				return false;
		}

		/// <summary>
		/// IValueConverter.ConvertBack
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
