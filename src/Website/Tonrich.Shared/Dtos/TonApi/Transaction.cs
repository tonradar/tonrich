namespace Tonrich.Shared.Dtos.TonApi;

public class Transaction
{
    [JsonPropertyName("account")]
    public Account Account { get; set; } = default!;

    [JsonPropertyName("fee")]
    public decimal Fee { get; set; }

    [JsonPropertyName("other_fee")]
    public decimal OtherFee { get; set; }

    [JsonPropertyName("storage_fee")]
    public decimal StorageFee { get; set; }

    [JsonPropertyName("in_msg")]
    public Message InMessage { get; set; } = default!;

    [JsonPropertyName("out_msgs")]
    public List<Message> OutMessages { get; set; } = default!;

    [JsonPropertyName("utime")]
    public long UTime { get; set; }

    [JsonPropertyName("lt")]
    public long Lt { get; set; }

    public DateTimeOffset CreateDateTime => DateTimeOffset.FromUnixTimeSeconds(UTime);

    public decimal SpentTon(string raw)
    {
        if (OutMessages != null && OutMessages.Where(c => c.Source.Address == raw).Any())
        {
            var spentPrice = OutMessages.Where(c => c.Source.Address == raw).Sum(a => a.Value);
            if (spentPrice > 0)
            {
                return spentPrice + Fee;
            }
        }

        return 0;
    }


    public decimal DepositTon(string raw)
    {
        if (InMessage != null && InMessage.Destination.Address == raw)
        {
            if (InMessage.Value > 0)
            {
                return InMessage.Value - Fee;
            }
        }

        return 0;
    }


    public decimal Price(string raw)
    {
        return DepositTon(raw) + SpentTon(raw);
    }
}
