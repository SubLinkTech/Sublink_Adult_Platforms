using System.Text.Json.Serialization;

namespace tech.SubLink.Fansly.FanslyClient.APIDataTypes;

internal abstract class BaseApiResponse {
    [JsonPropertyName("success")]
    public bool Success { get; set; } = false;
}
