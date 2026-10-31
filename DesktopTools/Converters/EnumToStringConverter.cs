using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace DesktopTools.Converters;

public class EnumToStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value?.ToString();
    }

    public object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string stringValue || targetType is not { IsEnum: true })
            return BindingOperations.DoNothing;

        return Enum.TryParse(targetType, stringValue, out var result)
            ? result
            : BindingOperations.DoNothing;
        ;
    }
}