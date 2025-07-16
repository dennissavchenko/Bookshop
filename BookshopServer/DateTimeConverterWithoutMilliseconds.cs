using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookShopServer;

/// <summary>
/// A custom JSON converter for <c>DateTime</c> values that excludes milliseconds during serialization.
/// Uses the format <c>yyyy-MM-ddTHH:mm:ss</c>.
/// </summary>
public class DateTimeConverterWithoutMilliseconds : JsonConverter<DateTime>
{
    private const string Format = "yyyy-MM-ddTHH:mm:ss";

    /// <summary>
    /// Reads and parses a <c>DateTime</c> value from JSON using the specified format without milliseconds.
    /// </summary>
    /// <param name="reader">The UTF-8 JSON reader.</param>
    /// <param name="typeToConvert">The type to convert (ignored).</param>
    /// <param name="options">Serialization options (ignored).</param>
    /// <returns>A parsed <c>DateTime</c> object.</returns>
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => DateTime.ParseExact(reader.GetString()!, Format, null);

    /// <summary>
    /// Writes a <c>DateTime</c> value to JSON using the specified format without milliseconds.
    /// </summary>
    /// <param name="writer">The UTF-8 JSON writer.</param>
    /// <param name="value">The <c>DateTime</c> value to write.</param>
    /// <param name="options">Serialization options (ignored).</param>
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(Format));
}
