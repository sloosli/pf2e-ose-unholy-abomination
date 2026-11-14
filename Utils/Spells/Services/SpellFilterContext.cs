using Utils.Spells.Interfaces;
using Utils.Spells.Models;

namespace Utils.Spells.Services;

internal class SpellFilterContext : ISpellFilterContext
{
    // Инициализируем дефолтными значениями на случай, если пользователь ещё не открывал вкладку фильтров
    public SpellFilterDto CurrentFilter { get; set; } = new()
    {
        Rarity = [Constants.DefaultRarity],
        Tradition = [],
        Type = Constants.DefaultSpellType,
        MinRank = Constants.MinRank,
        MaxRank = Constants.MaxRank
    };
}