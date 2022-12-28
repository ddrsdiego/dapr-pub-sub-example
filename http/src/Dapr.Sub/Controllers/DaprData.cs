namespace Dapr.Sub.Controllers
{
    using System.Text.Json.Serialization;

    public record DaprData<T>([property: JsonPropertyName("data")] T Data);
}