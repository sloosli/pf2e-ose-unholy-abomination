using Microsoft.Extensions.DependencyInjection;
using Utils.LootTables.Interfaces;
using Utils.LootTables.Services;
using Utils.LootTables.Services.Processors;
using Utils.MiniTools.Reincarnation;
using Utils.Spells.Interfaces;
using Utils.Spells.Services;

namespace Utils;

public static class Services
{
    public static void RegisterDependencies(ServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ILootProcessor, GemLootProcessor>();
        serviceCollection.AddSingleton<ILootProcessor, JewelleryLootProcessor>();
        serviceCollection.AddSingleton<ILootProcessor, CoinLootProcessor>();
        serviceCollection.AddSingleton<ILootProcessor, ScrollLootProcessor>();
        serviceCollection.AddSingleton<ISpellFilterContext, SpellFilterContext>();

        serviceCollection.AddTransient<ISpellSelectorService, SpellSelectorService>();
        serviceCollection.AddTransient<ITreasureTableRepository, TreasureTableRepository>();
        serviceCollection.AddTransient<IDiceRoller, DiceRoller>();
        serviceCollection.AddTransient<ITreasureGeneratorService, TreasureGeneratorService>();
        serviceCollection.AddTransient<IReincarnationService, ReincarnationService>();
    }
}