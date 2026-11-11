using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using DesktopTools.Services.Localization;
using ReactiveUI;
using Utils.LootTables.TreasureModels;

namespace DesktopTools.ViewModels.LootTables;

public class LootGroupViewModel : ViewModelBase
{
    private readonly LootGroup _model;

    public string ShortDescription => $"{LocalizationManager.Instance.GetByEnum(_model.LootType)}: x{_model.GetCount()} ({_model.GetTotalGoldValue()} {LocalizationManager.Instance["Gp"]})";

    public string FullDescription
    {
        get
        {
            if (_model.IndividualLootPieces.Count == 0)
                return string.Empty;

            var joinedItems = string.Join(", ", _model.IndividualLootPieces.Select(i => i.LootString));
            return joinedItems;
        }
    }

    public string RollInfo => string.Format(
        LocalizationManager.Instance["RollInfoFormat"],
        _model.Chance,
        _model.PercentileRoll);

    public bool IsExpanded
    {
        get => NotifyGetValue(() => IsExpanded);
        set => NotifySetValue(() => IsExpanded, value);
    }

    public ICommand ToggleExpandCommand { get; }

    public LootGroupViewModel(LootGroup model)
    {
        _model = model;
        IsExpanded = false;
        ToggleExpandCommand = ReactiveCommand.Create(() => IsExpanded = !IsExpanded);
        LocalizationManager.Instance.PropertyChanged += OnLocalizationChange;
    }

    private void OnLocalizationChange(object? sender, PropertyChangedEventArgs e)
    {
        NotifySetValue(() => FullDescription, string.Empty);
        NotifySetValue(() => ShortDescription, string.Empty);
        NotifySetValue(() => RollInfo, string.Empty);
    }
}