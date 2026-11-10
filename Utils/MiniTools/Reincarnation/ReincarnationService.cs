using Utils.Common.Enums;

namespace Utils.MiniTools.Reincarnation;

public class ReincarnationService : IReincarnationService
{
    private readonly Random _random;

    // Списки рас из файла
    private static readonly List<string> CommonRaces =
    [
        "Dwarf", "Elf", "Gnome", "Goblin", "Halfling", "Human", "Orc"
    ];

    private static readonly List<string> UncommonRareRaces =
    [
        "Anadi", "Azarketi", "Bugbear", "Catfolk", "Centaur", "Fetchling",
        "Fleshwarp", "Hobgoblin", "Jotunborn", "Kitsune", "Kobold",
        "Lizardfolk", "Minotaur", "Nagaji", "Ratfolk", "Samsaran",
        "Shisk", "Skeleton", "Strix", "Vishkanya", "Yaksha", "Yaoguai"
    ];

    public ReincarnationService(Random? random = null)
    {
        _random = random ?? new Random();
    }

    public string RollNewRace()
    {
        var d20Roll = _random.Next(1, 21);

        var raceList = d20Roll <= 14 ? CommonRaces : UncommonRareRaces;
        return raceList[_random.Next(CommonRaces.Count)];
    }
}