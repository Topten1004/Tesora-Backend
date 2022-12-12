// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

namespace NFTDatabaseEntities
{
    /// <summary>
    /// Category
    /// </summary>
    public class Contract
    {
        /// <summary>Primary key</summary>
        public int ContractId { get; set; }

        /// <summary>Contract Name</summary>
        public string ContractName { get; set; }

        /// <summary>Contract Version</summary>
        public string ContractVersion { get; set; }

        /// <summary>Contract Interface</summary>
        public string ContractInterface { get; set; }

        /// <summary>Contract Byte Code</summary>
        public string ContractByteCode { get; set; }

        /// <summary>Created date</summary>
        public DateTime CreateDate { get; set; }

    }
}
