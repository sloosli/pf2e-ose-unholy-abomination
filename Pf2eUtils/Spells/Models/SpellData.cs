using Pf2eUtils.Common.Enums;

namespace Pf2eUtils.Spells.Models;

public class SpellData
{
    // Основная информация о заклинании
    public required string Name { get; set; }
    public required PfsAvailability PfsAvailability { get; set; }
    public required string Source { get; set; }
    public required Rarity Rarity { get; set; }
    public required MagicTradition[] Tradition { get; set; }
    public required string[] Trait { get; set; }
    public required SpellType Type { get; set; }
    public required string Summary { get; set; }
    
    // Ранг заклинания
    public required int Rank { get; set; }
    public required int[] Heighten { get; set; }
    
    public required string Actions { get; set; }
    public required string Trigger { get; set; }
    public required string Target { get; set; }
    public required int Range { get; set; }
    public required int[] Area { get; set; }
    public required AreaType[] AreaType { get; set; }
    public required string Duration { get; set; }
    public required DefenceType[] Defense { get; set; }

    public override string ToString()
    {
        var tradition = string.Join(", ", Tradition);
        return $"{Name} \n{Rarity} Lvl {Rank}; {tradition} \n\n{Summary}";
    }
}
