using Utils.LootTables.FileDtos;
using Utils.LootTables.TreasureModels;

namespace Utils.LootTables.Interfaces;

public interface ITreasureGeneratorService
{
    /// <summary>
    /// Обновить список таблиц в памяти
    /// </summary>
    /// <param name="filePaths">Список путей до файлов с таблицами лута <see cref="TreasureFileDto"/></param>
    /// <param name="suppressErrors">Флаг заглушения ошибок, false - включает выброс эксепшенов при чтении файлов</param>
    void Initialize(IEnumerable<string> filePaths, bool suppressErrors = true);

    /// <summary>
    /// Получить список загруженных таблиц
    /// </summary>
    IEnumerable<TreasureTableInfo> GetAvailableTables();

    /// <summary>
    /// Попытаться сгенерировать сокровище по имени таблицы
    /// </summary>
    /// <param name="tableName">Имя таблицы (прим. "Type F").</param>
    /// <param name="treasure">Результат генерации, если таблица найдена; иначе null.</param>
    /// <returns>true, если генерация успешна; false, если таблица не найдена.</returns>
    bool TryGenerate(string tableName, out GeneratedTreasure? treasure);
}