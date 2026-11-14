using Utils.LootTables.Interfaces;
using Utils.LootTables.TreasureModels;

namespace Utils.LootTables.Services.Processors;

internal abstract class BaseLootProcessor : ILootProcessor
{
    protected readonly Random _random;
    protected abstract LootType LootType { get; }

    protected BaseLootProcessor(Random? random = null)
    {
        _random = random ?? new Random();
    }

    public bool CanProcess(LootType lootType)
    {
        return lootType == LootType;
    }

    public LootGroup Process(LootType lootType, int rolledQuantity, int chancePercentile, int percentileRow)
    {
        var results = GetIndividualPieces(lootType, rolledQuantity);

        return new LootGroup(
            IndividualLootPieces: results,
            LootType: lootType,
            Chance: chancePercentile,
            PercentileRoll: percentileRow
        );
    }

    protected abstract List<IndividualLootPiece> GetIndividualPieces(LootType lootType, int rolledQuantity);
}