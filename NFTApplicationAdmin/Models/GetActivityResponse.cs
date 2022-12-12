using NFTDatabaseEntities;

namespace NFTApplicationAdmin.Models
{
    /// <summary>
    /// Get Activity Response
    /// </summary>
    public class GetActivityResponse
    {
        /// <summary>Primary Key</summary>
        public int HistoryId { get; set; }

        /// <summary>Collection Image</summary>
        public string? Collection{ get; set; }

        /// <summary>Item Image</summary>
        public string? Item { get; set; }

        /// <summary>History Type : </summary>
        public string? HistoryType { get; set; }

        /// <summary>Price</summary>
        public decimal? Price { get; set; }

        /// <summary>From</summary>
        public string? From { get; set; }

        /// <summary>To</summary>
        public string? To { get; set; }

        /// <summary>Transaction Hask from Blockchain</summary>
        public string? TransactionHash { get; set; }

        /// <summary>Created</summary>
        public DateTime CreateDate { get; set; }
    }
}
