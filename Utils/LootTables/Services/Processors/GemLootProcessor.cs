using Utils.LootTables.TreasureModels;

namespace Utils.LootTables.Services.Processors;

internal class GemLootProcessor : BaseLootProcessor
{
    public GemLootProcessor(Random? random = null) : base(random)
    {
    }

    protected override LootType LootType => LootType.Gems;

    protected override List<IndividualLootPiece> GetIndividualPieces(LootType lootType, int rolledQuantity)
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

        return results;
    }
}