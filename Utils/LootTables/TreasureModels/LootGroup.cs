namespace Utils.LootTables.TreasureModels;

public record LootGroup(
    List<GeneratedLootItem> ExtractedLoot,
    int Chance,
    int PecentileRoll
);