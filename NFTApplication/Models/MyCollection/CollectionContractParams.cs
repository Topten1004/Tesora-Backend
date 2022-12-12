// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace NFTApplication.Models.MyCollection
{
    /// <summary>
    /// Collection Contract Parameters
    /// </summary>
    public class CollectionContractParams
    {
        /// <summary>
        /// Name
        /// </summary>
        [Parameter("string", "name_", 1)]
        public string Name { get; set; }

        /// <summary>
        /// Symbol
        /// </summary>
        [Parameter("string", "symbol_", 2)]
        public string Symbol { get; set; }

        /// <summary>
        /// Contract URI
        /// </summary>
        [Parameter("string", "contractURI_", 3)]
        public string ContractUri { get; set; }
        
    }
}
