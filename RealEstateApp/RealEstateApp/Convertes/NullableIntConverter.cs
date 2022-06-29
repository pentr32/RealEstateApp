using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealEstateApp.Converters
{
    public class NullableIntConverter : IValueConverter, IMarkupExtension
    {
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nullable = value as int?;
            var result = string.Empty;

            if (nullable.HasValue) result = nullable.Value.ToString();

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value as string;
            int? result = null;

            if (int.TryParse(stringValue, out var intValue)) result = intValue;

            return result;
        }
    }
}