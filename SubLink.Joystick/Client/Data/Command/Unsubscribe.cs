using System.Text.Json.Serialization;

namespace tech.SubLink.Joystick.Client.Data.Command;

public sealed class Unsubscribe : IBaseCommand {
    [JsonPropertyName("command")]
    public string Command { get; } = "unsubscribe";

    [JsonPropertyName("identifier")]
    public string Identifier { get; } = "{\"channel\":\"GatewayChannel\"}";
}
