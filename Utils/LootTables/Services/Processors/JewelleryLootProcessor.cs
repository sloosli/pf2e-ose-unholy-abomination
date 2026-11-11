using Utils.LootTables.TreasureModels;

namespace Utils.LootTables.Services.Processors;

public class JewelleryLootProcessor : ILootProcessor
{
    private readonly Random _random;

    public JewelleryLootProcessor(Random? random = null)
    {
        _random = random ?? new Random();
    }

    public bool CanProcess(LootType lootType)
    {
        return lootType == LootType.Jewellery;
    }

    public LootGroup Process(LootType lootType, int rolledQuantity, int chancePercentile, int percentileRow)
    {
        var results = new List<IndividualLootPiece>();

        for (var i = 0; i < rolledQuantity; i++)
        {
            var diceSum = _random.Next(1, 7) + _random.Next(1, 7) + _random.Next(1, 7);

            var value = diceSum * 100;
            results.Add(new IndividualLootPiece($"{value}gp", value));
        }

        return new LootGroup(
            IndividualLootPieces: results,
            LootType: lootType,
            Chance: chancePercentile,
            PercentileRoll: percentileRow
        );
    }
}