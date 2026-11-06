namespace Utils.LootTables.TreasureModels;

public record GeneratedLootItem(
    string LootType,
    int RolledQuantity,
    List<string>? SubItems = null // TODO генерация конкретных единиц лута
);