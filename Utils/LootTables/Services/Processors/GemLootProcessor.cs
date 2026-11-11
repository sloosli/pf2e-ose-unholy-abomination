using Utils.LootTables.TreasureModels;

namespace Utils.LootTables.Services.Processors;

public class GemLootProcessor : ILootProcessor
{
    private readonly Random _random;

    public GemLootProcessor(Random? random = null)
    {
        _random = random ?? new Random();
    }

    public bool CanProcess(LootType lootType)
    {
        return lootType == LootType.Gems;
    }

    public LootGroup Process(LootType lootType, int rolledQuantity, int chancePercentile, int percentileRow)
    {
        var results = new List<IndividualLootPiece>();

        for (var i = 0; i < rolledQuantity; i++)
        {
            var d20Roll = _random.Next(1, 21);
            var value = d20Roll switch
            {
                <= 4 => 10,
                <= 9 => 50,
                <= 15 => 100,
                <= 19 => 500,
                _ => 1000
            };

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