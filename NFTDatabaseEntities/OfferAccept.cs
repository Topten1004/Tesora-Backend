// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

namespace NFTDatabaseEntities
{
    /// <summary>
    /// Offer Accept
    /// </summary>
    public class OfferAccept
    {
        /// <summary>
        /// User Id 
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Offer Id
        /// </summary>
        public int OfferId { get; set; }

        /// <summary>
        /// Transaction Hash
        /// </summary>
        public string? TransactionHash { get; set; }
    }
}
