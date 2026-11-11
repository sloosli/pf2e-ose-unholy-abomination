namespace Utils.LootTables.TreasureModels;

public record CoinLootGroup(
    int Count,
    LootType LootType,
    int Chance,
    int PercentileRoll) : LootGroup([], LootType, Chance, PercentileRoll)
{
    public override int GetCount()
    {
        return Count;
    }

    public override int GetTotalGoldValue()
    {
        return LootType switch
        {
            LootType.Pp => Count * 5,
            LootType.Gp => Count,
            LootType.Ep => Count / 2,
            LootType.Sp => Count / 10,
            LootType.Cp => Count / 100,
            _ => 0
        };
    }
}