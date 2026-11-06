using Utils.LootTables.FileDtos;

namespace Utils.LootTables.Interfaces;

public interface ITreasureTableRepository
{
    /// <summary>
    /// Загружает и парсит JSON-файлы, добавляя или обновляя таблицы в памяти.
    /// </summary>
    /// <param name="filePaths">Список путей до файлов с таблицами <see cref="TreasureFileDto"/></param>
    /// <param name="suppressErrors">Флаг заглушения ошибок, false - включает выброс эксепшенов при чтении файлов</param>
    void LoadFiles(IEnumerable<string> filePaths, bool suppressErrors = true);

    /// <summary>
    /// Возвращает все загруженные таблицы.
    /// </summary>
    /// <returns>Список ключ - имя таблицы, значение DTO таблицы</returns>
    IEnumerable<KeyValuePair<string, TreasureTableDto>> GetAllTables();

    /// <summary>
    /// Поиск таблицы по её строгому имени (например, "Type F").
    /// </summary>
    /// <param name="name">Имя таблицы</param>
    /// <param name="table">Результат, если таблица найдена; иначе null.</param>
    /// <returns>Найдена ли таблица</returns>
    bool TryGetTable(string name, out TreasureTableDto? table);
}