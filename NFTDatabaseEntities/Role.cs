// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

namespace NFTDatabaseEntities
{
    /// <summary>
    /// Role
    /// </summary>
    public class Role
    {
        /// <summary>Primary Key</summary>
        public int RoleId { get; set; }

        /// <summary>Role Name</summary>
        public string Name { get; set; }

        /// <summary>Read?</summary>
        public bool Read { get; set; }

        /// <summary>Write?</summary>
        public bool Write { get; set; }

        /// <summary>Delete?</summary>
        public bool Delete { get; set; }

    }
}
