using Utils.LootTables.Interfaces;
using Utils.LootTables.Services.Processors;
using Utils.LootTables.TreasureModels;

namespace Utils.LootTables.Services;

internal class TreasureGeneratorService : ITreasureGeneratorService
{
    private readonly ITreasureTableRepository _repository;
    private readonly IDiceRoller _diceRoller;
    private readonly List<ILootProcessor> _lootProcessors;
    private readonly Random _random;

    public TreasureGeneratorService(
        ITreasureTableRepository repository,
        IDiceRoller diceRoller,
        IEnumerable<ILootProcessor> lootProcessors,
        Random? random = null)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _diceRoller = diceRoller ?? throw new ArgumentNullException(nameof(diceRoller));
        _lootProcessors = lootProcessors.ToList();
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

        var triggeredGroups = tableDto.Rows
            .Select(x => new
            {
                PercentileRoll = _random.Next(1, 101),
                Row = x
            })
            .Where(x => x.PercentileRoll <= x.Row.ChancePercent)
            .SelectMany(x => x.Row.ContentBundle
                .Select(y => new
                {
                    RawLootType = y.LootType,
                    LootTypeEnum = LootTypeExtensions.FromString(y.LootType),
                    RolledQuantity = _diceRoller.Roll(y.DiceFormula)
                })
                .Where(y => y.RolledQuantity > 0)
                .Select(y =>
                    GetLootSpecificLootListByType(y.LootTypeEnum, y.RawLootType, y.RolledQuantity, x.Row.ChancePercent,
                        x.PercentileRoll)))
            .ToList();

        treasure = new GeneratedTreasure(
            TableName: tableName,
            Groups: triggeredGroups
        );

        return true;
    }

    private LootGroup GetLootSpecificLootListByType(LootType lootType,
        string rawLootType,
        int rolledQuantity,
        int chancePercentile,
        int percentileRow)
    {
        var matchingProcessor = _lootProcessors.FirstOrDefault(p => p.CanProcess(lootType));

        if (matchingProcessor != null)
            return matchingProcessor.Process(lootType, rolledQuantity, chancePercentile, percentileRow);

        var displayString = lootType == LootType.Other ? rawLootType : lootType.ToString();

        return new LootGroup(
            IndividualLootPieces: [new IndividualLootPiece($"{rolledQuantity} {displayString}", 0)],
            LootType: lootType,
            Chance: chancePercentile,
            PercentileRoll: percentileRow);
    }
}