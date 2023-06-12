using System.Runtime.Serialization;

namespace Tonrich.Shared.Dtos.TonApi.Enum;

public enum EventActionType
{
    [EnumMember(Value = "TonTransfer")]
    TonTransfer,

    [EnumMember(Value = "JettonTransfer")]
    JettonTransfer,

    [EnumMember(Value = "NftItemTransfer")]
    NftItemTransfer,

    [EnumMember(Value = "ContractDeploy")]
    ContractDeploy,

    [EnumMember(Value = "Subscribe")]
    Subscribe,

    [EnumMember(Value = "UnSubscribe")]
    UnSubscribe,

    [EnumMember(Value = "AuctionBid")]
    AuctionBid,

    [EnumMember(Value = "NftPurchase")]
    NftPurchase,

    [EnumMember(Value = "Unknown")]
    Unknown
}
