using System.Text.Json.Serialization;

namespace Descartes.Api.Models
{
    /// <summary>
    /// Response payload describing the diff result.
    /// </summary>
    public class DiffResponse
    {
        public string DiffResultType { get; init; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IReadOnlyList<DiffSegmentResponse>? Diffs { get; init; }
    }

    public class DiffSegmentResponse
    {
        public int Offset { get; init; }
        public int Length { get; init; }
    }
}
