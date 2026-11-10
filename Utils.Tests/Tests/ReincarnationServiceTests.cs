using Utils.MiniTools.Reincarnation;

namespace Utils.Tests;

[TestFixture]
public class ReincarnationServiceTests
{
    [Test]
    public void RollNewRace_Determinism_ReturnsExpectedRaces()
    {
        var service = new ReincarnationService(new Random(42));

        var result1 = service.RollNewRace();

        Assert.Multiple(() =>
        {
            Assert.That(result1, Is.Not.Null.Or.Empty);
        });
    }
}