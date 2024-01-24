using System.Text.Json.Serialization;

namespace Domain.Model;

[Serializable]
public class User
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("username")]
    public string? UserName { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("address")]
    public Address? Adress { get; set; }

    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("website")]
    public string? WebSite { get; set; }

    [JsonPropertyName("company")]
    public Company? Company { get; set; }
}