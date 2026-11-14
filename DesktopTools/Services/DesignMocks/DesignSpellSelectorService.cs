using System.Collections.Generic;
using Utils.Spells.Interfaces;
using Utils.Spells.Models;

namespace DesktopTools.Services.DesignMocks;

public class DesignSpellSelectorService : ISpellSelectorService
{
    public List<RankedSpellDto> GetRandomSpellsByFilter(SpellFilterDto filter, int count = 1)
    {
        return [];
    }

    public List<SpellDto> GetSpells(SpellFilterDto filter)
    {
        return [];
    }
}