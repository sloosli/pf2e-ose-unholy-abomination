using System.Text.Json;
using System.Text.Json.Serialization;

namespace Utils.LootTables.FileDtos;

[JsonSourceGenerationOptions(
    PropertyNameCaseInsensitive = true,
    AllowTrailingCommas = true,
    ReadCommentHandling = JsonCommentHandling.Skip)]
[JsonSerializable(typeof(TreasureFileDto))]
internal partial class TreasureJsonContext : JsonSerializerContext
{
}