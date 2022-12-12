using System.Numerics;

namespace NFTApplication.Models.MyWallet
{
    /// <summary>
    /// Send Coins Request
    /// </summary>
    public class SendCoinsRequest
    {
        /// <summary>To Address</summary>
        public string? ToAddress { get; set; }

        /// <summary>Amount Eth exluding gas fee</summary>
        public decimal AmountEth { get; set; }

        /// <summary>Gas Eth</summary>
        public decimal GasWei { get; set; }
    }
}
