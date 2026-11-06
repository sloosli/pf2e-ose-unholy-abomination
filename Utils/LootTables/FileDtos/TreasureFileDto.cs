namespace Utils.LootTables.FileDtos;

public class TreasureFileDto
{
    // Ключ — название таблицы
    public Dictionary<string, TreasureTableDto> Tables { get; set; } = new();
}