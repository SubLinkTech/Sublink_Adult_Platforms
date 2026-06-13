using System.Text.Json.Serialization;

namespace tech.SubLink.Joystick.Client.Data.Response;

public sealed class Ping : IBaseResponse {
    [JsonPropertyName("message")]
    public long Message { get; set; } = 0;
}
