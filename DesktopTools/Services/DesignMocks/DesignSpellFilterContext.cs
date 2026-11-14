using Utils.Spells.Interfaces;
using Utils.Spells.Models;

namespace DesktopTools.Services.DesignMocks;

public class DesignSpellFilterContext : ISpellFilterContext
{
    public SpellFilterDto CurrentFilter { get; set; } = new SpellFilterDto();
}