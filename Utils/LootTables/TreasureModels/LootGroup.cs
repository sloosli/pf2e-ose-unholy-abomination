namespace Utils.LootTables.TreasureModels;

public record LootGroup(
    List<IndividualLootPiece> IndividualLootPieces,
    LootType LootType,
    int Chance,
    int PercentileRoll
)
{
    public virtual int GetCount()
    {
        return IndividualLootPieces.Count;
    }
    
    public virtual int GetTotalGoldValue()
    {
        return IndividualLootPieces.Sum(x => x.GoldValue);
    }
}