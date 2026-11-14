using Utils.LootTables.Interfaces;
using Utils.LootTables.TreasureModels;

namespace Utils.LootTables.Services.Processors;

public class CoinLootProcessor : ILootProcessor
{
    public bool CanProcess(LootType lootType)
    {
        return lootType is
            LootType.Cp or
            LootType.Sp or
            LootType.Ep or
            LootType.Gp or
            LootType.Pp;
    }

    public LootGroup Process(LootType lootType, int rolledQuantity, int chancePercentile, int percentileRow)
    {
        return new CoinLootGroup(
            Count: rolledQuantity,
            LootType: lootType,
            Chance: chancePercentile,
            PercentileRoll: percentileRow
        );
    }
}