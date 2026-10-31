using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using DesktopTools.ViewModels.Spells;
using DynamicData;
using Pf2eUtils.Spells.Models;
using Pf2eUtils.Spells.Services;
using ReactiveUI;

namespace DesktopTools.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly Random _rnd = new();
    private readonly SpellSelectorService _spellSelectorService;

    public SpellFilterViewModel SpellFilter { get; }

    public ObservableCollection<SpellItemViewModel> Spells
    {
        get => NotifyGetValue(() => Spells) ?? [];
        set => NotifySetValue(() => Spells, value);
    }


// ТЕКУЩЕЕ СЛУЧАЙНОЕ ЗАКЛИНАНИЕ (содержит развернутое описание)
    public SpellItemViewModel? RandomSpell
    {
        get => NotifyGetValue(() => RandomSpell);
        set => NotifySetValue(() => RandomSpell, value);
    }

// Команда для получения случайного заклинания
    public ICommand GetRandomSpellCommand { get; }

    public MainWindowViewModel()
    {
        // TODO SpellSelectorService refactor
        _spellSelectorService = SpellSelectorService.Instance;
        SpellFilter = new SpellFilterViewModel(ApplySpellFilter);
        Spells = [];

        var canExecuteRandom = this
            .WhenAnyValue(x => x.Spells.Count)
            .Select(count => count > 0);

        GetRandomSpellCommand = ReactiveCommand.Create(ExecuteGetRandomSpell, canExecuteRandom);
    }

    private void ApplySpellFilter(SpellFilterDto filter)
    {
        var filteredList = _spellSelectorService.GetSpells(filter);

        var itemViewModels = filteredList
            .Select(spell => new SpellItemViewModel(spell, filter))
            .ToList();

        Spells.Clear();
        Spells.AddRange(itemViewModels);
    }

    private void ExecuteGetRandomSpell()
    {
        if (Spells.Count == 0)
            return;

        var randomSpell = Spells[_rnd.Next(Spells.Count)];
        var chosenRank = randomSpell.FilteredHeighten[_rnd.Next(randomSpell.FilteredHeighten.Count)];

        RandomSpell = new SpellItemViewModel(randomSpell, chosenRank);
    }
}