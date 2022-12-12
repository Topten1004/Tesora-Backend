// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

namespace NFTDatabaseEntities
{
    /// <summary>
    /// History
    /// </summary>
    public class History
    {
        /// <summary>Primary Key</summary>
        public int HistoryId { get; set; }

        /// <summary>Item Id (FK to Items)</summary>
        public int? ItemId { get; set; }

        /// <summary>Collection Id (FK to Collections)</summary>
        public int? CollectionId { get; set; }

        /// <summary>From Id (FK to Users)</summary>
        public int? FromId { get; set; }

        /// <summary>To Id (FK to Users)</summary>
        public int? ToId { get; set; }

        /// <summary>Transaction Hask from Blockchain</summary>
        public string? TransactionHash { get; set; }

        /// <summary>Transaction Amount</summary>
        public decimal? Price { get; set; }

        /// <summary>Currency</summary>
        public string? Currency { get; set; }

        /// <summary>History Types</summary>
        public enum HistoryTypes {
            /// <summary>created</summary>
            created,
            /// <summary>minted</summary>
            minted,
            /// <summary>bid</summary>
            bid,
            /// <summary>transfer</summary>
            transfer
        }

        /// <summary>History Type : </summary>
        public HistoryTypes HistoryType { get; set; }

        /// <summary>Is Valid?</summary>
        public bool? IsValid { get; set; }

        /// <summary>Created</summary>
        public DateTime CreateDate { get; set; }

    }
}
