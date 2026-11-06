namespace Utils.LootTables.Interfaces;

public interface IDiceRoller
{
    /// <summary>
    /// Парсит формулу кубов и возвращает итоговый результат броска.
    /// Поддерживаемые операторы в порядке приоритета: "d"(прим. 1d6, генерация случайного числа с "куба"), "*", "+" и "-"
    /// </summary>
    /// <param name="formula"></param>
    /// <returns></returns>
    int Roll(string formula);
}