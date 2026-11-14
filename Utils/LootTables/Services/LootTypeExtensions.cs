using Utils.LootTables.TreasureModels;

namespace Utils.LootTables.Services;

public static class LootTypeExtensions
{
    public static LootType FromString(string lootTypeStr)
    {
        if (string.IsNullOrWhiteSpace(lootTypeStr))
            return LootType.Other;

        return lootTypeStr.Trim().ToLower() switch
        {
            "cp" => LootType.Cp,
            "sp" => LootType.Sp,
            "ep" => LootType.Ep,
            "gp" => LootType.Gp,
            "pp" => LootType.Pp,
            "gems" => LootType.Gems,
            "jewellery" or "pieces of jewellery" => LootType.Jewellery,
            "scroll" or "scrolls" or "magic scrolls" => LootType.Scrolls,
            _ => LootType.Other
        };
    }
}