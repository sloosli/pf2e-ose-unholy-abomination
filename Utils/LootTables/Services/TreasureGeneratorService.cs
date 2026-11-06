using Utils.LootTables.Interfaces;
using Utils.LootTables.TreasureModels;

namespace Utils.LootTables.Services;

internal class TreasureGeneratorService : ITreasureGeneratorService
{
    private readonly ITreasureTableRepository _repository;
    private readonly IDiceRoller _diceRoller;
    private readonly Random _random;

    public TreasureGeneratorService(
        ITreasureTableRepository repository,
        IDiceRoller diceRoller,
        Random? random = null)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _diceRoller = diceRoller ?? throw new ArgumentNullException(nameof(diceRoller));
        _random = random ?? new Random();
    }

    public void Initialize(IEnumerable<string> filePaths, bool suppressErrors = true)
    {
        _repository.LoadFiles(filePaths, suppressErrors);
    }

    public IEnumerable<TreasureTableInfo> GetAvailableTables()
    {
        return _repository.GetAllTables().Select(kvp => new TreasureTableInfo(
            Name: kvp.Key,
            AverageValue: kvp.Value.AverageValue ?? string.Empty
        ));
    }

    public bool TryGenerate(string tableName, out GeneratedTreasure? treasure)
    {
        treasure = null;

        if (string.IsNullOrWhiteSpace(tableName))
            return false;

        if (!_repository.TryGetTable(tableName, out var tableDto))
            return false;

        if (tableDto?.Rows == null)
            return false;

        var triggeredGroups = new List<LootGroup>();
        foreach (var row in tableDto.Rows)
        {
            var percentileRoll = _random.Next(1, 101);

            if (percentileRoll > row.ChancePercent)
                continue;

            var groupItems = row.ContentBundle
                .Select(x => new GeneratedLootItem(x.LootType, _diceRoller.Roll(x.DiceFormula)))
                .Where(x => x.RolledQuantity > 0)
                .ToList();

            if (groupItems.Count > 0)
            {
                triggeredGroups.Add(new LootGroup(groupItems, row.ChancePercent, percentileRoll));
            }
        }

        treasure = new GeneratedTreasure(
            TableName: tableName,
            Groups: triggeredGroups
        );

        return true;
    }
}