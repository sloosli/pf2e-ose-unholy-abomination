using System.Collections.Generic;
using Utils.LootTables.Interfaces;
using Utils.LootTables.TreasureModels;

namespace DesktopTools.DesignServices;

internal class DesignTreasureGeneratorService : ITreasureGeneratorService
{
    public void Initialize(IEnumerable<string> filePaths, bool suppressErrors = true)
    {
    }

    public IEnumerable<TreasureTableInfo> GetAvailableTables() => [];

    public bool TryGenerate(string tableName, out GeneratedTreasure? treasure)
    {
        treasure = null;
        return false;
    }
}