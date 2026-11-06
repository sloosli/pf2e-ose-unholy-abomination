namespace Utils.LootTables.FileDtos;

public class TreasureRowDto
{
    public int ChancePercent { get; set; }
    public List<LootComponentDto> ContentBundle { get; set; } = [];
}