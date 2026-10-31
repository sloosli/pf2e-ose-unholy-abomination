using System;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace DesktopTools.Converters;

public class ComparisonConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        if (value == null || parameter == null)
            return false;

        return value.Equals(parameter);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter,
        System.Globalization.CultureInfo culture)
    {
        if (value is true && parameter != null)
        {
            return parameter;
        }

        return BindingOperations.DoNothing;
    }
}