using Tonrich.Shared.Util;

namespace Tonrich.Shared.Dtos.TonApi;

public class Event
{
    [JsonPropertyName("account")]
    public Account? Account { get; set; }

    [JsonPropertyName("actions")]
    public List<EventAction>? Actions { get; set; }

    [JsonPropertyName("event_id")]
    public string? EventId { get; set; }

    [JsonPropertyName("fee")]
    public EventFee? Fee { get; set; }

    [JsonPropertyName("in_progress")]
    public bool InProgress { get; set; }

    [JsonPropertyName("is_scam")]
    public bool IsScam { get; set; }

    [JsonPropertyName("lt")]
    public long Lt { get; set; }

    [JsonPropertyName("timestamp")]
    public long TimeStamp { get; set; }

    public DateTimeOffset EventDateTime => DateTimeOffset.FromUnixTimeSeconds(TimeStamp);

    public List<TransactionDto>? GetDate(string walletId)
    {
        if (Actions is null)
            return null;

        bool isInActions = false;
        var transactions = new List<TransactionDto>();

        foreach (var action in Actions)
        {
            if (action.ActionStatus != Enum.EventStatus.Ok)
                continue;

            var transaction = new TransactionDto
            {
                TransactionDateTime = EventDateTime
            };
            decimal amount = 0;
            if (action.ActionType == Enum.EventActionType.TonTransfer && action.TonTransfer != null)
            {
                amount = Convert.ToDecimal(action.TonTransfer.Amount);

                if (action.TonTransfer?.Sender?.Address == walletId)
                {
                    transaction.Address = action.TonTransfer?.Sender?.Address;
                    transaction.IsSpent = true;
                    isInActions = true;
                }
                else if (action.TonTransfer?.Recipient?.Address == walletId)
                {
                    transaction.Address = action.TonTransfer?.Recipient?.Address;
                    transaction.IsSpent = false;
                    isInActions = true;
                }
            }
            else if (action.ActionType == Enum.EventActionType.AuctionBid && action.AuctionBid != null)
            {
                _ = decimal.TryParse(action.AuctionBid?.Price?.Value, out amount);

                if (action.AuctionBid?.Bidder?.Address == walletId)
                {
                    transaction.Address = action.AuctionBid?.Bidder?.Address;
                    transaction.IsSpent = true;
                    isInActions = true;
                }
                else if (action.AuctionBid?.Beneficiary?.Address == walletId)
                {
                    transaction.Address = action.AuctionBid?.Beneficiary?.Address;
                    transaction.IsSpent = false;
                    isInActions = true;
                }

                if (action.AuctionBid!.NFT != null)
                {
                    transaction.NFTAddress = action.AuctionBid!.NFT.Address;
                    isInActions = true;
                }
            }
            else if (action.ActionType == Enum.EventActionType.JettonTransfer && action.JettonTransfer != null)
            {
                amount = Convert.ToDecimal(action.JettonTransfer?.Amount);

                if (action.JettonTransfer?.Sender?.Address == walletId)
                {
                    transaction.Address = action.JettonTransfer?.Sender?.Address;
                    transaction.IsSpent = true;
                    isInActions = true;
                }
                else if (action.JettonTransfer?.Recipient?.Address == walletId)
                {
                    transaction.Address = action.JettonTransfer?.Recipient?.Address;
                    transaction.IsSpent = false;
                    isInActions = true;
                }
            }
            else if (action.ActionType == Enum.EventActionType.NftPurchase && action.NFTPurchase != null)
            {
                _ = decimal.TryParse(action.NFTPurchase?.Amount?.Value, out amount);

                if (action.NFTPurchase?.Seller?.Address == walletId)
                {
                    transaction.Address = action.NFTPurchase?.Seller?.Address;
                    transaction.IsSpent = true;
                    isInActions = true;
                }
                else if (action.NFTPurchase?.Buyer?.Address == walletId)
                {
                    transaction.Address = action.NFTPurchase?.Buyer?.Address;
                    transaction.IsSpent = false;
                    isInActions = true;
                }
            }
            else if (action.ActionType == Enum.EventActionType.Subscribe && action.Subscribe != null)
            {
                amount = Convert.ToDecimal(action.Subscribe.Amount);

                if (action.Subscribe?.Subscriber?.Address == walletId)
                {
                    transaction.Address = action.Subscribe?.Subscriber?.Address;
                    transaction.IsSpent = true;
                    isInActions = true;
                }
                else if (action.Subscribe?.Beneficiary?.Address == walletId)
                {
                    transaction.Address = action.Subscribe?.Beneficiary?.Address;
                    transaction.IsSpent = false;
                    isInActions = true;
                }
            }
            else if (action.ActionType == Enum.EventActionType.UnSubscribe && action.UnSubscribe != null)
            {
                amount = Convert.ToDecimal(action.UnSubscribe.Amount);

                if (action.UnSubscribe?.Subscriber?.Address == walletId)
                {
                    transaction.Address = action.UnSubscribe?.Subscriber?.Address;
                    transaction.IsSpent = true;
                    isInActions = true;
                }
                else if (action.UnSubscribe?.Beneficiary?.Address == walletId)
                {
                    transaction.Address = action.Subscribe?.Beneficiary?.Address;
                    transaction.IsSpent = false;
                    isInActions = true;
                }
            }

            if (isInActions)
            {
                transaction.Amount = amount / AppSetting.TONDenominator;
                transaction.Type = action.ActionType;
                transactions.Add(transaction);
            }
        }

        if (isInActions == false)
            return null;

        return transactions;
    }

}
