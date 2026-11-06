using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text.Json;
using Avalonia.Platform;

namespace DesktopTools.Services.Localization;

internal sealed class LocalizationManager : INotifyPropertyChanged
{
    public static LocalizationManager Instance { get; } = new();

    private Dictionary<string, string> _currentTranslations = new();
    private CultureInfo _currentCulture = new("ru-RU");

    public string this[string key] => _currentTranslations.TryGetValue(key, out var translation)
        ? translation
        : $"%%%{key}%%%";

    public CultureInfo CurrentCulture
    {
        get => _currentCulture;
        set
        {
            if (Equals(_currentCulture, value))
                return;

            _currentCulture = value;

            LoadCulture(_currentCulture);

            OnPropertyChanged("Item");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private LocalizationManager()
    {
        LoadCulture(_currentCulture);
    }

    private void OnPropertyChanged(string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void LoadCulture(CultureInfo culture)
    {
        try
        {
            var assetUri = new Uri($"avares://DesktopTools/Assets/Lang/{culture.Name}.json");

            using var stream = AssetLoader.Open(assetUri);
            using var reader = new StreamReader(stream);
            var jsonContent = reader.ReadToEnd();

            var deserialized = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonContent);

            if (deserialized != null)
            {
                _currentTranslations = deserialized;
            }
        }
        catch (Exception)
        {
            _currentTranslations = new Dictionary<string, string>();
        }
    }
}