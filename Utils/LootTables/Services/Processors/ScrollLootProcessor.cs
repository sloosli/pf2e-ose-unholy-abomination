using Utils.LootTables.TreasureModels;
using Utils.Spells.Interfaces;

namespace Utils.LootTables.Services.Processors;

internal class ScrollLootProcessor : BaseLootProcessor
{
    private readonly ISpellSelectorService _spellSelectorService;
    private readonly ISpellFilterContext _spellFilterContext;

    public ScrollLootProcessor(ISpellSelectorService spellSelectorService,
        ISpellFilterContext spellFilterContext,
        Random? random = null) : base(random)
    {
        _spellSelectorService = spellSelectorService;
        _spellFilterContext = spellFilterContext;
    }

    protected override LootType LootType => LootType.Scrolls;

    protected override List<IndividualLootPiece> GetIndividualPieces(LootType lootType, int rolledQuantity)
    {
        var filter = _spellFilterContext.CurrentFilter;
        var spells = _spellSelectorService.GetRandomSpellsByFilter(filter, rolledQuantity);

        return spells
            .Select(s =>
                new IndividualLootPiece($"{s.Name} - {s.RandomRank}", 0)) 
            .ToList();
    }
}