using Utils.LootTables.Services;

namespace Utils.Tests;

[TestFixture]
public class TreasureTableRepositoryTests
{
    private string _tempFilePath = null!;

    [SetUp]
    public void Setup()
    {
        _tempFilePath = Path.GetTempFileName();
        const string sampleJson = """
                                  {
                                    "Tables": {
                                      "Test Type A": {
                                        "AverageValue": "100gp average",
                                        "Rows": [
                                          {
                                            "ChancePercent": 50,
                                            "ContentBundle": [
                                              { "LootType": "gp", "DiceFormula": "1d6" },
                                              { "LootType": "sp", "DiceFormula": "1d8" }
                                            ]
                                          }
                                        ]
                                      }
                                    }
                                  }
                                  """;
        File.WriteAllText(_tempFilePath, sampleJson);
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(_tempFilePath))
        {
            File.Delete(_tempFilePath);
        }
    }

    [Test]
    public void LoadFiles_ValidFile_LoadsTablesSuccessfully()
    {
        var repository = new TreasureTableRepository();

        repository.LoadFiles([_tempFilePath], suppressErrors: false);

        var found = repository.TryGetTable("Test Type A", out var table);

        Assert.Multiple(() =>
        {
            Assert.That(found, Is.True);
            Assert.That(table, Is.Not.Null);
            Assert.That(table!.AverageValue, Is.EqualTo("100gp average"));
            Assert.That(table.Rows, Has.Count.EqualTo(1));
            Assert.That(table.Rows[0].ChancePercent, Is.EqualTo(50));
            Assert.That(table.Rows[0].ContentBundle, Has.Count.EqualTo(2));
            Assert.That(table.Rows[0].ContentBundle[0], Is.Not.Null);
            Assert.That(table.Rows[0].ContentBundle[1], Is.Not.Null);
        });
    }

    [Test]
    public void LoadFiles_NonExistentFileAndSuppressTrue_DoesNotThrow()
    {
        var repository = new TreasureTableRepository();

        Assert.DoesNotThrow(() =>
            repository.LoadFiles(["invalid_path.json"], suppressErrors: true));
    }

    [Test]
    public void LoadFiles_NonExistentFileAndSuppressFalse_ThrowsException()
    {
        var repository = new TreasureTableRepository();

        Assert.Throws<FileNotFoundException>(() =>
            repository.LoadFiles(["invalid_path.json"], suppressErrors: false));
    }
}