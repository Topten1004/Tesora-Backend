// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using System.Text.Json.Serialization;

namespace NFTApplicationAdmin.Models
{
    /// <summary>
    /// Update Contract
    /// </summary>
    public class UpdateContractRequest
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
        public IFormFile ContractInterface { get; set; }

        /// <summary>Contract Byte Code</summary>
        [JsonPropertyName("contractByteCode")]
        public IFormFile ContractByteCode { get; set; }
    }
}
