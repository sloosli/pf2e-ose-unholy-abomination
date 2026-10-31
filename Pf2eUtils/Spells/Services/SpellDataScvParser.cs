using System.Text;
using System.Text.RegularExpressions;
using Pf2eUtils.Common.Enums;
using Pf2eUtils.Spells.Models;

namespace Pf2eUtils.Spells.Services;

internal static partial class SpellDataCsvParser
{
    [GeneratedRegex("\r\n|\r|\n")]
    private static partial Regex EndLineRegex();

    private const int ColumnCount = 18;

    public static List<SpellData> ParseFile(string filePath, bool hasHeader = true, Encoding? encoding = null)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Путь к файлу не может быть пустым.", nameof(filePath));

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Файл не найден по пути: {filePath}");

        encoding ??= Encoding.UTF8;

        using var reader = new StreamReader(filePath, encoding);
        var csvContent = reader.ReadToEnd();

        return ParseCsv(csvContent, hasHeader);
    }

    private static List<SpellData> ParseCsv(string csvContent, bool hasHeader = true)
    {
        var spells = new List<SpellData>();

        var lines = EndLineRegex().Split(csvContent)
            .Where(line => !string.IsNullOrWhiteSpace(line));

        if (hasHeader)
        {
            lines = lines.Skip(1);
        }

        foreach (var line in lines)
        {
            var columns = ParseCsvLine(line);

            var spell = new SpellData
            {
                Name = columns[0],
                PfsAvailability = ParseSimpleEnum<PfsAvailability>(columns[1]),
                Source = columns[2],
                Tradition = ParseSimpleEnumArray<MagicTradition>(columns[3]),
                Rarity = ParseSimpleEnum<Rarity>(columns[4]),
                Trait = ParseStringArray(columns[5]),
                Type = ParseSimpleEnum<SpellType>(columns[6]),
                Rank = GetIntOrNegative(columns[7]),
                Heighten = ParseIntArray(columns[8]),
                Summary = columns[9],
                Actions = columns[10],
                Trigger = columns[11],
                Target = columns[12],
                Range = GetIntOrNegative(columns[13]),
                Area = ParseIntArray(columns[14]),
                AreaType = ParseSimpleEnumArray<AreaType>(columns[15]),
                Duration = columns[16],
                Defense = ParseSimpleEnumArray<DefenceType>(columns[17])
            };

            spells.Add(spell);
        }

        return spells;
    }

    private static List<string> ParseCsvLine(string line)
    {
        var result = new List<string>();
        var inQuotes = false;
        var currentColumn = new StringBuilder();

        foreach (var c in line)
        {
            switch (c)
            {
                case '"':
                    inQuotes = !inQuotes;
                    break;
                case ',' when !inQuotes:
                    result.Add(currentColumn.ToString().Trim(' ', '"'));
                    currentColumn.Clear();
                    break;
                default:
                    currentColumn.Append(c);
                    break;
            }
        }

        result.Add(currentColumn.ToString().Trim(' ', '"'));

        for (var i = result.Count; i < ColumnCount; ++i)
        {
            result.Add(string.Empty);
        }

        return result;
    }

    private static T ParseSimpleEnum<T>(string value) where T : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(value))
            return default;

        var sanitized = value.Replace(" ", "");
        return Enum.TryParse<T>(sanitized, true, out var result) ? result : default;
    }

    private static T[] ParseSimpleEnumArray<T>(string value) where T : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(value))
            return [];

        return value.Split(',')
            .Select(x => x.Trim().Replace(" ", ""))
            .Select(ParseSimpleEnum<T>)
            .ToArray();
    }

    private static string[] ParseStringArray(string value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? []
            : value
                .Split(',')
                .Select(x => x.Trim())
                .ToArray();
    }

    private static int[] ParseIntArray(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return [];

        return value.Split(',')
            .Select(x => x.Trim())
            .Select(GetIntOrNegative)
            .ToArray();
    }

    private static int GetIntOrNegative(string value)
    {
        return int.TryParse(value, out var result) ? result : -1;
    }
}