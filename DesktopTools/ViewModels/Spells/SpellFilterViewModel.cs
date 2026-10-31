using System;
using System.Windows.Input;
using Pf2eUtils.Common.Enums;
using Pf2eUtils.Spells;
using Pf2eUtils.Spells.Models;
using ReactiveUI;

namespace DesktopTools.ViewModels.Spells;

public class SpellFilterViewModel : ViewModelBase
{
    private readonly Action<SpellFilterDto>? _onApplyFilter;

    public Rarity[] Rarity
    {
        get => NotifyGetValue(() => Rarity) ?? [];
        set => NotifySetValue(() => Rarity, value);
    }

    public MagicTradition[] Tradition
    {
        get => NotifyGetValue(() => Tradition) ?? [];
        set => NotifySetValue(() => Tradition, value);
    }

    public SpellType Type
    {
        get => NotifyGetValue(() => Type);
        set => NotifySetValue(() => Type, value);
    }

    public int MinRank
    {
        get => NotifyGetValue(() => MinRank);
        set
        {
            if (value > MaxRank)
            {
                MaxRank = value;
            }

            NotifySetValue(() => MinRank, value);
        }
    }

    public int MaxRank
    {
        get => NotifyGetValue(() => MaxRank);
        set
        {
            if (value < MinRank)
            {
                value = MinRank;
            }

            NotifySetValue(() => MaxRank, value);
        }
    }

    public ICommand ApplyFilterCommand { get; }
    public ICommand ResetFilterCommand { get; }

    public SpellFilterViewModel(Action<SpellFilterDto>? onApplyFilter)
    {
        _onApplyFilter = onApplyFilter;
        ResetToDefaults();

        ApplyFilterCommand = ReactiveCommand.Create(ExecuteApplyFilter);
        ResetFilterCommand = ReactiveCommand.Create(ExecuteResetFilter);
    }

    private void ResetToDefaults()
    {
        Rarity = [Pf2eUtils.Common.Enums.Rarity.Common];
        Tradition = [];
        Type = Constants.DefaultSpellType;
        MinRank = Constants.MinRank;
        MaxRank = Constants.MaxRank;
    }

    private void ExecuteApplyFilter()
    {
        var filter = GetFilter();

        _onApplyFilter?.Invoke(filter);
    }

    private void ExecuteResetFilter()
    {
        ResetToDefaults();
    }

    private SpellFilterDto GetFilter()
    {
        return new SpellFilterDto
        {
            Rarity = Rarity,
            Tradition = Tradition,
            Type = Type,
            MinRank = MinRank,
            MaxRank = MaxRank
        };
    }
}