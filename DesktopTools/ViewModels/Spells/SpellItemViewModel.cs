using System.Collections.Generic;
using Utils.Common.Enums;
using Utils.Spells.Models;

namespace DesktopTools.ViewModels.Spells;

public class SpellItemViewModel : ViewModelBase
{
    public IReadOnlyList<int> FilteredHeighten { get; }

    public string Name
    {
        get => NotifyGetValue(() => Name) ?? string.Empty;
        set => NotifySetValue(() => Name, value);
    }

    public string Summary
    {
        get => NotifyGetValue(() => Summary) ?? string.Empty;
        set => NotifySetValue(() => Summary, value);
    }

    public Rarity Rarity
    {
        get => NotifyGetValue(() => Rarity);
        set => NotifySetValue(() => Rarity, value);
    }

    public SpellType Type
    {
        get => NotifyGetValue(() => Type);
        set => NotifySetValue(() => Type, value);
    }

    public string TraditionsDisplay
    {
        get => NotifyGetValue(() => TraditionsDisplay) ?? string.Empty;
        set => NotifySetValue(() => TraditionsDisplay, value);
    }

    public string DefenseDisplay
    {
        get => NotifyGetValue(() => DefenseDisplay) ?? string.Empty;
        set => NotifySetValue(() => DefenseDisplay, value);
    }

    public string DisplayRank
    {
        get => NotifyGetValue(() => DisplayRank) ?? string.Empty;
        set => NotifySetValue(() => DisplayRank, value);
    }

    public SpellItemViewModel(SpellDto spell, SpellFilterDto filter)
    {
        Name = spell.Name;
        Summary = spell.Summary;
        Rarity = spell.Rarity;
        Type = spell.Type;
        TraditionsDisplay = spell.TraditionsDisplay;
        DefenseDisplay = spell.DefenseDisplay;
        FilteredHeighten = spell.FilteredHeighten;
        DisplayRank = string.Join(", ", FilteredHeighten);
    }

    public SpellItemViewModel(SpellItemViewModel baseModel, int rank)
    {
        Name = baseModel.Name;
        Summary = baseModel.Summary;
        Rarity = baseModel.Rarity;
        Type = baseModel.Type;
        TraditionsDisplay = baseModel.TraditionsDisplay;
        DefenseDisplay = baseModel.DefenseDisplay;
        DisplayRank = $"{rank}";
        FilteredHeighten = baseModel.FilteredHeighten;
    }
}