using Utils.LootTables.FileDtos;
using Utils.LootTables.Services;
using Utils.Tests.Mocks;

namespace Utils.Tests;

[TestFixture]
public class TreasureGeneratorServiceTests
{
    private FakeTableRepository _repository = null!;
    private DiceRoller _diceRoller = null!;

    [SetUp]
    public void Setup()
    {
        _repository = new FakeTableRepository();
        _diceRoller = new DiceRoller(new Random(42));
    }

    [Test]
    public void TryGenerate_TableDoesNotExist_ReturnsFalseAndNullTreasure()
    {
        var service = new TreasureGeneratorService(_repository, _diceRoller, new Random(42));

        var result = service.TryGenerate("NonExistentTable", out var treasure);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(treasure, Is.Null);
        });
    }

    [Test]
    public void TryGenerate_RowWith100PercentChance_AlwaysTriggersAndReturnsLoot()
    {
        var tableDto = new TreasureTableDto
        {
            AverageValue = "50gp",
            Rows =
            [
                new TreasureRowDto
                {
                    ChancePercent = 100,
                    ContentBundle =
                    [
                        new LootComponentDto { LootType = "gp", DiceFormula = "10" },
                        new LootComponentDto { LootType = "sp", DiceFormula = "1d6" }
                    ]
                }
            ]
        };
        _repository.AddTable("Type Guaranteed", tableDto);

        var service = new TreasureGeneratorService(_repository, _diceRoller, new Random(42));

        var success = service.TryGenerate("Type Guaranteed", out var treasure);

        Assert.Multiple(() =>
        {
            Assert.That(success, Is.True);
            Assert.That(treasure, Is.Not.Null);
            Assert.That(treasure!.TableName, Is.EqualTo("Type Guaranteed"));
            Assert.That(treasure.Groups, Has.Count.EqualTo(1));

            var lootItems = treasure.Groups[0].ExtractedLoot;
            Assert.That(lootItems, Has.Count.EqualTo(2));

            Assert.That(lootItems[0].LootType, Is.EqualTo("gp"));
            Assert.That(lootItems[0].RolledQuantity, Is.EqualTo(10));

            Assert.That(lootItems[1].LootType, Is.EqualTo("sp"));
            Assert.That(lootItems[1].RolledQuantity, Is.GreaterThan(0));
            Assert.That(lootItems[1].RolledQuantity, Is.LessThan(7));
        });
    }

    [Test]
    public void TryGenerate_RowWith0PercentChance_NeverTriggers()
    {
        var tableDto = new TreasureTableDto
        {
            Rows =
            [
                new TreasureRowDto
                {
                    ChancePercent = 0,
                    ContentBundle =
                    [
                        new LootComponentDto { LootType = "Gems", DiceFormula = "1d4" }
                    ]
                }
            ]
        };
        _repository.AddTable("Type Empty", tableDto);

        var service = new TreasureGeneratorService(_repository, _diceRoller, new Random(42));

        var success = service.TryGenerate("Type Empty", out var treasure);

        Assert.Multiple(() =>
        {
            Assert.That(success, Is.True);
            Assert.That(treasure!.Groups, Is.Empty);
        });
    }
}