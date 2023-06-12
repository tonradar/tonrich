namespace Tonrich.Shared.Dtos.TonApi;

public class AccountEvents
{
    [JsonPropertyName("events")]
    public List<Event>? Events { get; set; }

    [JsonPropertyName("next_from")]
    public long NextFrom { get; set; }
}
