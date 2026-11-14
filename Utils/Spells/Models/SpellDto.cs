using Utils.Common.Enums;

namespace Utils.Spells.Models;

public class SpellDto
{
    public IReadOnlyList<int> FilteredHeighten { get; init; } = [];

    public string Name { get; init; } = string.Empty;

    public string Summary { get; init; } = string.Empty;

    public Rarity Rarity { get; init; }

    public SpellType Type { get; init; }

    public string TraditionsDisplay { get; init; } = string.Empty;

    public string DefenseDisplay { get; init; } = string.Empty;
}