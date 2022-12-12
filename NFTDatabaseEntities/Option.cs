// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

namespace NFTDatabaseEntities
{
    /// <summary>
    /// Option
    /// </summary>
    public class Option
    {
        /// <summary>Primary Key</summary>
        public int OptionId { get; set; }

        /// <summary>Option Name</summary>
        public string Name { get; set; }

        /// <summary>Option Value</summary>
        public string? Value { get; set; }

    }
}
