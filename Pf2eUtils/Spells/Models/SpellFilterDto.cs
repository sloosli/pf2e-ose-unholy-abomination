using Pf2eUtils.Common.Enums;

namespace Pf2eUtils.Spells.Models;

public class SpellFilterDto
{
    public Rarity[] Rarity { get; init; } = [];
    public MagicTradition[] Tradition { get; init; } = [];
    public SpellType Type { get; init; } = Constants.DefaultSpellType;
    public int MinRank { get; init; } = Constants.MinRank;
    public int MaxRank { get; init; } = Constants.MaxRank;
}
