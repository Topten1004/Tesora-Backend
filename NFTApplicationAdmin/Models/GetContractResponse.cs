// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using System.Text.Json.Serialization;

namespace NFTApplicationAdmin.Models
{
    /// <summary>
    /// Get Contract
    /// </summary>
    public class GetContractResponse
    {
        /// <summary>Contract Id</summary>
        [JsonPropertyName("contractId")]
        public int ContractId { get; set; }

        /// <summary>Contract Name</summary>
        [JsonPropertyName("contractName")]
        public string ContractName { get; set; }

        /// <summary>Contract Version</summary>
        [JsonPropertyName("contractVersion")]
        public string ContractVersion { get; set; }

        /// <summary>Contract Interface</summary>
        [JsonPropertyName("contractInterface")]
        public string ContractInterface { get; set; }

        /// <summary>Contract Byte Code</summary>
        [JsonPropertyName("contractByteCode")]
        public string ContractByteCode { get; set; }

        /// <summary>Create Date</summary>
        [JsonPropertyName("createDate")]
        public DateTime CreateDate { get; set; }
    }
}
