using System.Text.Json;
using Utils.LootTables.FileDtos;
using Utils.LootTables.Interfaces;

namespace Utils.LootTables.Services;

internal class TreasureTableRepository : ITreasureTableRepository
{
    private readonly Dictionary<string, TreasureTableDto> _tables = new(StringComparer.OrdinalIgnoreCase);

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        AllowTrailingCommas = true,
        ReadCommentHandling = JsonCommentHandling.Skip
    };

    public void LoadFiles(IEnumerable<string> filePaths, bool suppressErrors = true)
    {
        ArgumentNullException.ThrowIfNull(filePaths);

        foreach (var path in filePaths)
        {
            try
            {
                if (!File.Exists(path))
                    throw new FileNotFoundException($"Файл таблицы сокровищ не найден по пути: {path}");

                var jsonContent = File.ReadAllText(path);
                var fileDto = JsonSerializer.Deserialize(jsonContent, TreasureJsonContext.Default.TreasureFileDto);

                if (fileDto?.Tables == null)
                    continue;

                foreach (var (tableName, tableDto) in fileDto.Tables)
                {
                    _tables[tableName] = tableDto;
                }
            }
            catch (Exception)
            {
                if (!suppressErrors)
                    throw;
            }
        }
    }

    public IEnumerable<KeyValuePair<string, TreasureTableDto>> GetAllTables()
    {
        return _tables;
    }

    public bool TryGetTable(string name, out TreasureTableDto? table)
    {
        table = null;

        if (string.IsNullOrWhiteSpace(name))
            return false;

        return _tables.TryGetValue(name, out table);
    }
}