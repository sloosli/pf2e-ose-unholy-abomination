using Utils.LootTables.TreasureModels;

namespace Utils.LootTables.Interfaces;

public interface ILootProcessor
{
    /// <summary>
    /// Проверяет, может ли этот процессор детализировать данный тип лута.
    /// </summary>
    bool CanProcess(LootType lootType);

    /// <summary>
    /// Конкретизирует выпавший предмет и возвращает список текстовых строк описания.
    /// </summary>
    /// <param name="lootType">Тип предметов</param>
    /// <param name="rolledQuantity">Количество предметов, выкинутых на кубах.</param>
    /// <param name="chancePercentile">Шанс выпадения</param>
    /// <param name="percentileRow">Выпавший шанс</param>
    LootGroup Process(LootType lootType,
        int rolledQuantity,
        int chancePercentile,
        int percentileRow);
}