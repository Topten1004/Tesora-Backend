// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

namespace NFTDatabaseEntities
{
    /// <summary>
    /// Cart 
    /// </summary>
    public class Sale
    {
        /// <summary>Primary key</summary>
        public int SaleId { get; set; }

        /// <summary>Foreign key</summary>
        public int UserId { get; set; }

        /// <summary>Sale Types</summary>
        public enum SaleTypes
        {
            /// <summary>Credit</summary>
            Credit,
            /// <summary>Crypto</summary>
            Crypto
        }

        /// <summary>Type</summary>
        public SaleTypes SaleType { get; set; }

        /// <summary>Total Amount</summary>
        public decimal TotalAmount { get; set; }

        /// <summary>Paying Currency</summary>
        public string Currency { get; set; }

        /// <summary>Payment Statuses</summary>
        public enum PaymentStatuses
        {
            /// <summary>New</summary>
            New,
            /// <summary>Pending</summary>
            Pending,
            /// <summary>Completed</summary>
            Completed,
            /// <summary>Expired</summary>
            Expired,
            /// <summary>Unresolved</summary>
            Unresolved,
            /// <summary>Resolved</summary>
            Resolved,
            /// <summary>Cancelled</summary>
            Cancelled,
            /// <summary>PendingRefund</summary>
            PendingRefund,
            /// <summary>Refunded</summary>
            Refunded
        }

        /// <summary>Payment Status</summary>
        public PaymentStatuses PaymentStatus { get; set; }

        /// <summary>Payment Status Reason</summary>
        public string? Reason { get; set; }

        /// <summary>Address</summary>
        public string? Address { get; set; }

        /// <summary>Create date</summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Line items 
        /// </summary>
        public List<SaleItem> SaleItems { get; set; }
    }
}
