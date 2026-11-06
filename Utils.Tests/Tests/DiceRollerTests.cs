using Utils.LootTables.Services;

namespace Utils.Tests;

[TestFixture]
public class DiceRollerTests
{
    private readonly DiceRoller _diceRoller = new(new Random(42));

    [TestCase("3", ExpectedResult = 3)]
    [TestCase("0", ExpectedResult = 0)]
    [TestCase("  15  ", ExpectedResult = 15)]
    public int Roll_StaticNumbers_ReturnsExpectedValue(string formula)
    {
        return _diceRoller.Roll(formula);
    }

    [TestCase("-5", ExpectedResult = 0)]
    [TestCase("1d6 - 10", ExpectedResult = 0)]
    public int Roll_NegativeResults_TruncatesToZero(string formula)
    {
        return _diceRoller.Roll(formula);
    }

    [TestCase("999999999999999999", ExpectedResult = int.MaxValue)]
    [TestCase("1d6 * 20000000000", ExpectedResult = int.MaxValue)]
    public int Roll_OverflowNumbers_ReturnsIntMaxValue(string formula)
    {
        return _diceRoller.Roll(formula);
    }

    [TestCase("2 * 10 + 5", ExpectedResult = 25)]
    [TestCase("5 + 3 * 2", ExpectedResult = 11)]
    [TestCase("10 - 2 * 2 - 3", ExpectedResult = 3)] 
    public int Roll_MathExpressionsWithoutDice_RespectsPrecedence(string formula)
    {
        return _diceRoller.Roll(formula);
    }

    [TestCase("1d6 + abc", ExpectedResult = 0)] 
    [TestCase("1dd6 * 100", ExpectedResult = 0)] 
    public int Roll_InvalidFormula_ReturnsZero(string formula)
    {
        return _diceRoller.Roll(formula);
    }
}