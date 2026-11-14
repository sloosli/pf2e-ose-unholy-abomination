using Utils.LootTables.TreasureModels;

namespace Utils.LootTables.Services.Processors;

internal class JewelleryLootProcessor : BaseLootProcessor
{
    public JewelleryLootProcessor(Random? random = null) : base(random)
    {
    }

    protected override LootType LootType => LootType.Jewellery;

    protected override List<IndividualLootPiece> GetIndividualPieces(LootType lootType, int rolledQuantity)
    {
        var results = new List<IndividualLootPiece>();

        for (var i = 0; i < rolledQuantity; i++)
        {
            var diceSum = _random.Next(1, 7) + _random.Next(1, 7) + _random.Next(1, 7);

            var value = diceSum * 100;
            results.Add(new IndividualLootPiece($"{value}gp", value));
        }

        return results;
    }
}