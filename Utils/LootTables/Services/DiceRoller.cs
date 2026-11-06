using System.Text.RegularExpressions;
using Utils.LootTables.Interfaces;

namespace Utils.LootTables.Services;

internal partial class DiceRoller : IDiceRoller
{
    private readonly Random _random;

    // "1d6", "3D8"
    private static readonly Regex DiceRegex = GetDiceRegex();

    // (0-9, +, -, *, любой пробел)
    private static readonly Regex ValidExpressionRegex = GetValidExpressionRegex();

    public DiceRoller(Random? random = null)
    {
        _random = random ?? new Random();
    }

    public int Roll(string formula)
    {
        if (string.IsNullOrWhiteSpace(formula))
            return 0;

        try
        {
            var expression = DiceRegex.Replace(formula, RollDice);

            return ValidExpressionRegex.IsMatch(expression) ? EvaluateSimpleExpression(expression) : 0;
        }
        catch (Exception)
        {
            return 0;
        }
    }

    [GeneratedRegex(@"(?<count>\d+)[dD](?<sides>\d+)", RegexOptions.Compiled)]
    private static partial Regex GetDiceRegex();

    [GeneratedRegex(@"^[0-9\s\+\-\*]+$", RegexOptions.Compiled)]
    private static partial Regex GetValidExpressionRegex();

    private static int EvaluateSimpleExpression(string expr)
    {
        expr = expr.Replace(" ", "");

        return ParseSum(expr);
    }

    private static int ParseSum(string expr)
    {
        var index = -1;
        var type = 0;

        for (var i = expr.Length - 1; i >= 0; i--)
        {
            if (expr[i] == '+')
            {
                index = i;
                type = '+';
                break;
            }

            if (expr[i] == '-')
            {
                index = i;
                type = '-';
                break;
            }
        }

        if (index == -1)
            return ParseProduct(expr);

        var leftStr = expr[..index];
        var rightStr = expr[(index + 1)..];

        var left = ParseSum(leftStr);
        var right = ParseProduct(rightStr);

        checked
        {
            var result = type == '+' ? (long)left + right : (long)left - right;

            return result switch
            {
                > int.MaxValue => int.MaxValue,
                < 0 => 0,
                _ => (int)result
            };
        }
    }

    private static int ParseProduct(string expr)
    {
        var index = expr.LastIndexOf('*');
        if (index == -1)
            return ParsePrimary(expr);

        var leftStr = expr[..index];
        var rightStr = expr[(index + 1)..];

        var left = ParseProduct(leftStr);
        var right = ParsePrimary(rightStr);

        checked
        {
            var result = (long)left * right;
            return result switch
            {
                > int.MaxValue => int.MaxValue,
                < 0 => 0,
                _ => (int)result
            };
        }
    }

    private static int ParsePrimary(string expr)
    {
        if (string.IsNullOrEmpty(expr))
            return 0;

        if (!long.TryParse(expr, out var value))
            return expr.StartsWith('-') ? 0 : int.MaxValue;

        return value switch
        {
            < 0 => 0,
            > int.MaxValue => int.MaxValue,
            _ => (int)value
        };
    }

    private string RollDice(Match match)
    {
        if (!int.TryParse(match.Groups["count"].Value, out var count) || count < 0)
            count = 0;

        if (!int.TryParse(match.Groups["sides"].Value, out var sides) || sides < 0)
            sides = 0;

        long diceSubTotal = 0;
        for (var i = 0; i < count; i++)
        {
            if (diceSubTotal >= int.MaxValue)
                return int.MaxValue.ToString();

            diceSubTotal += _random.Next(1, sides == int.MaxValue ? int.MaxValue : sides + 1);
        }

        return diceSubTotal.ToString();
    }
}