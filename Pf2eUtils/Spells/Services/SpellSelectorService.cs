using Pf2eUtils.Spells.Models;

namespace Pf2eUtils.Spells.Services;

public sealed class SpellSelectorService
{
    private readonly IReadOnlyList<SpellData> _spells;
    private readonly Random _rnd = new();

    private SpellSelectorService()
    {
        // TODO SpellSelectorService refactor
        _spells = SpellDataCsvParser.ParseFile("Data/pathfinder_2e_spells_cleaned.csv");
    }

    // TODO SpellSelectorService refactor
    public static SpellSelectorService Instance { get; } = new();

    public List<SpellData> GetSpells(SpellFilterDto filter)
    {
        if (filter.MaxRank < filter.MinRank)
            return [];

        var filteredSpells = _spells
            .Where(s => s.Type == filter.Type &&
                        ((s.Rank >= filter.MinRank && s.Rank <= filter.MaxRank) || 
                         s.Heighten.Any(h => h >= filter.MinRank && h <= filter.MaxRank)) &&
                        (filter.Rarity.Length == 0 || filter.Rarity.Contains(s.Rarity)) &&
                        (filter.Tradition.Length == 0 || s.Tradition.Intersect(filter.Tradition).Any()))
            .ToList();

        return filteredSpells;
    }
}