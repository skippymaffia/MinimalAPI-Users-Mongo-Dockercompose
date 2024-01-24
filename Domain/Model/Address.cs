using System.Text.Json.Serialization;

namespace Domain.Model;

[Serializable]
public class Address
{
    [JsonPropertyName("street")]
    public string? Street { get; set; }

    [JsonPropertyName("suite")]
    public string? Suite { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("zipcode")]
    public string? ZipCode { get; set; }

    [JsonPropertyName("geo")]
    public Geo? Geo { get; set; }


}