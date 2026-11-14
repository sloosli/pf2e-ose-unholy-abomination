using Utils.Spells.Interfaces;
using Utils.Spells.Models;

namespace Utils.Spells.Services;

internal sealed class SpellSelectorService : ISpellSelectorService
{
    private readonly IReadOnlyList<SpellData> _spells;
    private readonly Random _rnd = new();

    public SpellSelectorService()
    {
        // TODO SpellSelectorService refactor
        _spells = SpellDataCsvParser.ParseFile("Data/pathfinder_2e_spells_cleaned.csv");
    }

    public List<RankedSpellDto> GetRandomSpellsByFilter(SpellFilterDto filter, int count = 1)
    {
        if (count <= 0)
            return [];

        var matchingSpells = GetSpells(filter);

        if (matchingSpells.Count <= count)
            return [.. ChooseRandomRanks(matchingSpells)];

        for (var i = 0; i < count; i++)
        {
            var randomIndex = _rnd.Next(i, matchingSpells.Count);
            (matchingSpells[i], matchingSpells[randomIndex]) = (matchingSpells[randomIndex], matchingSpells[i]);
        }

        return [.. ChooseRandomRanks(matchingSpells.GetRange(0, count))];
    }

    public List<SpellDto> GetSpells(SpellFilterDto filter)
    {
        if (filter.MaxRank < filter.MinRank)
            return [];

        var filteredSpells = _spells
            .Select(x => new
            {
                FilteredRanks = x.Heighten
                    .Where(h => filter.MinRank <= h && h <= filter.MaxRank)
                    .ToList(),
                Spell = x
            })
            .Where(x => x.Spell.Type == filter.Type &&
                        x.FilteredRanks.Count != 0 &&
                        (filter.Rarity.Length == 0 || filter.Rarity.Contains(x.Spell.Rarity)) &&
                        (filter.Tradition.Length == 0 || x.Spell.Tradition.Intersect(filter.Tradition).Any()))
            .Select(x => new SpellDto
            {
                FilteredHeighten = x.FilteredRanks,
                Name = x.Spell.Name,
                Summary = x.Spell.Summary,
                Rarity = x.Spell.Rarity,
                Type = x.Spell.Type,
                TraditionsDisplay = string.Join(", ", x.Spell.Tradition),
                DefenseDisplay = string.Join(", ", x.Spell.Defense)
            })
            .ToList();

        return filteredSpells;
    }

    private IEnumerable<RankedSpellDto> ChooseRandomRanks(IEnumerable<SpellDto> spells)
    {
        return spells.Select(x => new RankedSpellDto
        {
            FilteredHeighten = x.FilteredHeighten,
            RandomRank = x.FilteredHeighten[_rnd.Next(x.FilteredHeighten.Count)],
            Name = x.Name,
            Summary = x.Summary,
            Rarity = x.Rarity,
            Type = x.Type,
            TraditionsDisplay = x.TraditionsDisplay,
            DefenseDisplay = x.DefenseDisplay
        });
    }
}