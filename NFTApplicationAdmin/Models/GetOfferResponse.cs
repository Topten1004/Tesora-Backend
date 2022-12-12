namespace NFTApplicationAdmin.Models
{
    /// <summary>
    /// Get Offer Response
    /// </summary>
    public class GetOfferResponse
    {
        /// <summary>Primary Key</summary>
        public int OfferId { get; set; }

        /// <summary>Item Image</summary>
        public string? ItemName { get; set; }

        /// <summary>Item Image</summary>
        public string? Image { get; set; }

        /// <summary>Price</summary>
        public decimal? Price { get; set; }

        /// <summary>Sender Name</summary>
        public string? SenderName { get; set; }

        /// <summary>Receiver Name</summary>
        public string? ReceiverName { get; set; }

        /// <summary>Created</summary>
        public DateTime CreateDate { get; set; }
    }
}
