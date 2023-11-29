using Tonrich.Shared.Dtos.TonApi.Enum;

namespace Tonrich.Shared.Dtos.TonApi;

public class EventAction
{
    [JsonPropertyName("AuctionBid")]
    public AuctionBidAction? AuctionBid { get; set; }

    [JsonPropertyName("ContractDeploy")]
    public ContractDeployAction? ContractDeploy { get; set; }

    [JsonPropertyName("JettonTransfer")]
    public JettonTransferAction? JettonTransfer { get; set; }

    [JsonPropertyName("NftItemTransfer")]
    public NftItemTransfer? NftItemTransfer { get; set; }

    [JsonPropertyName("NftPurchase")]
    public NFTPurchase? NFTPurchase { get; set; }

    [JsonPropertyName("Subscribe")]
    public SubscribeAction? Subscribe { get; set; }

    [JsonPropertyName("TonTransfer")]
    public TonTransfer? TonTransfer { get; set; }

    [JsonPropertyName("UnSubscribe")]
    public SubscribeAction? UnSubscribe { get; set; }

    [JsonPropertyName("simple_preview")]
    public EventActionSimplePreview? SimplePreview { get; set; }

    [JsonPropertyName("status")]
    public string? StatusStr { get; set; }
    public EventStatus ActionStatus
    {
        get
        {
            _ = System.Enum.TryParse<EventStatus>(StatusStr, out var result);
            return result;
        }
    }

    [JsonPropertyName("type")]
    public string? TypeStr { get; set; }
    public EventActionType ActionType
    {
        get
        {
            _ = System.Enum.TryParse<EventActionType>(TypeStr, out var result);
            return result;
        }
    }
}
