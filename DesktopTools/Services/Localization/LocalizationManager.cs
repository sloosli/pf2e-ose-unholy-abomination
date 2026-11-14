using System;
using System.ComponentModel;
using System.Globalization;
using DesktopTools.Resources;

namespace DesktopTools.Services.Localization;

internal sealed class LocalizationManager : INotifyPropertyChanged
{
    public static LocalizationManager Instance { get; } = new();

    public string this[string key] => LocalizationStrings.ResourceManager.GetString(key, LocalizationStrings.Culture) ?? $"%%%{key}%%%";

    public CultureInfo CurrentCulture
    {
        get => LocalizationStrings.Culture;
        set
        {
            if (Equals(LocalizationStrings.Culture, value))
                return;

            LocalizationStrings.Culture = value;

            OnPropertyChanged("Item");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private LocalizationManager()
    {
        LocalizationStrings.Culture = new CultureInfo("ru-RU");
    }

    public string GetByEnum<T>(T key) where T : Enum
    {
        return this[key.ToString()];
    }

    private void OnPropertyChanged(string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}