using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using ReactiveUI;
using Utils.LootTables.Interfaces;
using Utils.LootTables.TreasureModels;

namespace DesktopTools.ViewModels.LootTables;

public class TreasureGeneratorViewModel : ViewModelBase
{
    private readonly ITreasureGeneratorService _treasureService;

    public ObservableCollection<TreasureTableInfo> AvailableTables { get; } = [];

    public bool HasTables
    {
        get => NotifyGetValue(() => HasTables);
        set => NotifySetValue(() => HasTables, value);
    }

    public TreasureTableInfo? SelectedTable
    {
        get => NotifyGetValue(() => SelectedTable);
        set => NotifySetValue(() => SelectedTable, value);
    }

    public GeneratedTreasure? LastGenerationResult
    {
        get => NotifyGetValue(() => LastGenerationResult);
        set => NotifySetValue(() => LastGenerationResult, value);
    }

    public ICommand RollLootCommand { get; }

    public TreasureGeneratorViewModel(ITreasureGeneratorService treasureService)
    {
        _treasureService = treasureService;

        RollLootCommand = ReactiveCommand.Create(ExecuteRollLoot);
    }

    public void RefreshTables()
    {
        AvailableTables.Clear();

        var tables = _treasureService.GetAvailableTables().ToList();

        AvailableTables.AddRange(tables);

        HasTables = AvailableTables.Count > 0;
    }

    private void ExecuteRollLoot()
    {
        if (SelectedTable == null)
            return;

        if (_treasureService.TryGenerate(SelectedTable.Name, out var result))
            LastGenerationResult = result;
    }
}