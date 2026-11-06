namespace Utils.LootTables.FileDtos;

public class TreasureTableDto
{
    public string? AverageValue { get; set; }
    public List<TreasureRowDto> Rows { get; set; } = [];
}