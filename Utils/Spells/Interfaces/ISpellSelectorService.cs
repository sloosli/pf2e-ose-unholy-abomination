using Utils.Spells.Models;

namespace Utils.Spells.Interfaces;

public interface ISpellSelectorService
{
    /// <summary>
    /// Получить несколько случайных заклинаний
    /// </summary>
    /// <param name="filter">Фильтр заклинаний</param>
    /// <param name="count">Количество заклинаний</param>
    public List<RankedSpellDto> GetRandomSpellsByFilter(SpellFilterDto filter, int count = 1);

    /// <summary>
    /// Получить все подходящие заклинания
    /// </summary>
    /// <param name="filter">Фильтр заклинаний</param>
    public List<SpellDto> GetSpells(SpellFilterDto filter);
}