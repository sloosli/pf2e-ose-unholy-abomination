using System.Diagnostics.CodeAnalysis;
using Utils.LootTables.FileDtos;
using Utils.LootTables.Interfaces;

namespace Utils.Tests.Mocks;

internal class FakeTableRepository : ITreasureTableRepository
{
    private readonly Dictionary<string, TreasureTableDto> _tables = new(StringComparer.OrdinalIgnoreCase);

    public void AddTable(string name, TreasureTableDto table) => _tables[name] = table;

    public void LoadFiles(IEnumerable<string> filePaths, bool suppressErrors = true) { }

    public IEnumerable<KeyValuePair<string, TreasureTableDto>> GetAllTables() => _tables;

    public bool TryGetTable(string name, [NotNullWhen(true)] out TreasureTableDto? table)
    {
        return _tables.TryGetValue(name, out table);
    }
}