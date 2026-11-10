using Microsoft.Extensions.DependencyInjection;
using Utils.LootTables.Interfaces;
using Utils.LootTables.Services;
using Utils.MiniTools.Reincarnation;

namespace Utils;

public static class Services
{
    public static void RegisterDependencies(ServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<ITreasureTableRepository, TreasureTableRepository>();
        serviceCollection.AddTransient<IDiceRoller, DiceRoller>();
        serviceCollection.AddTransient<ITreasureGeneratorService, TreasureGeneratorService>();
        serviceCollection.AddTransient<IReincarnationService, ReincarnationService>();
    }
}