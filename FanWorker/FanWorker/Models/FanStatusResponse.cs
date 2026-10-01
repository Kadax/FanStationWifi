using System.Text.Json.Serialization;

namespace FanWorker.Models;

public class FanStatusResponse
{
    [JsonPropertyName("power")]
    public int Power { get; set; }

    [JsonPropertyName("direction")]
    public int Direction { get; set; }

    [JsonPropertyName("speed")]
    public int Speed { get; set; }

    [JsonPropertyName("servoPosition")]
    public int ServoPosition { get; set; }
}