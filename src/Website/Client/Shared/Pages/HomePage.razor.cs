namespace Tonrich.Client.Shared.Pages;

public partial class HomePage
{
    public string WalletAddress { get; set; }
    void ChangeWalletAddress(string address)
    {
        WalletAddress = address;
    }
}
