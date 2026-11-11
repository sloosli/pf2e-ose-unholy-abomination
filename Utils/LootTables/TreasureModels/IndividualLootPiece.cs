namespace Utils.LootTables.TreasureModels;

public record IndividualLootPiece(
    string LootString,
    int GoldValue)
{
    public override string ToString() => LootString;
}