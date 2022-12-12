// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

namespace NFTDatabaseEntities
{
    /// <summary>
    /// Cart 
    /// </summary>
    public class XPOVerseLot
    {
        /// <summary>Primary key</summary>
        public int LotId { get; set; }

        /// <summary>Neighborhood</summary>
        public string? Neighborhood { get; set; }

        /// <summary>Ring</summary>
        public string Ring { get; set; }

        /// <summary>Section</summary>
        public string Section { get; set; }

        /// <summary>Block</summary>
        public string Block { get; set; }

        /// <summary>Lot</summary>
        public string Lot { get; set; }

        /// <summary>Owner Id</summary>
        public int? OwnerId { get; set; }

        /// <summary>For Sale?</summary>
        public bool ForSale { get; set; }

        /// <summary>Reserved Cart Id</summary>
        public int? ReservedCartId { get; set; }
    }
}
