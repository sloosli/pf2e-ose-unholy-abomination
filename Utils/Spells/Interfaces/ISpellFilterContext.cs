using Utils.Spells.Models;

namespace Utils.Spells.Interfaces;

public interface ISpellFilterContext
{
    SpellFilterDto CurrentFilter { get; set; }
}