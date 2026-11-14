using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using ReactiveUI;
using Utils.Spells.Interfaces;
using Utils.Spells.Models;

namespace DesktopTools.ViewModels.Spells;

public class SpellsListViewModel : ViewModelBase
{
    private readonly Random _rnd = new();
    private readonly ISpellFilterContext _spellFilterContext;
    private readonly ISpellSelectorService _spellSelectorService;

    public SpellFilterViewModel SpellFilter { get; }

    public ObservableCollection<SpellItemViewModel> Spells
    {
        get => NotifyGetValue(() => Spells) ?? [];
        set => NotifySetValue(() => Spells, value);
    }

    public SpellItemViewModel? RandomSpell
    {
        get => NotifyGetValue(() => RandomSpell);
        set => NotifySetValue(() => RandomSpell, value);
    }

    public ICommand GetRandomSpellCommand { get; }

    public SpellsListViewModel(ISpellFilterContext spellFilterContext, ISpellSelectorService spellSelectorService)
    {
        _spellFilterContext = spellFilterContext;
        _spellSelectorService = spellSelectorService;
        SpellFilter = new SpellFilterViewModel(ApplySpellFilter);
        Spells = [];

        var canExecuteRandom = this
            .WhenAnyValue(x => x.Spells.Count)
            .Select(count => count > 0);

        GetRandomSpellCommand = ReactiveCommand.Create(ExecuteGetRandomSpell, canExecuteRandom);
    }

    private void ApplySpellFilter(SpellFilterDto filter)
    {
        _spellFilterContext.CurrentFilter = filter;

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