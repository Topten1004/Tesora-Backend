// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//


namespace NFTDatabaseEntities
{
    /// <summary>
    /// User
    /// </summary>
    public class User
    {
        /// <summary>Primary Key</summary>
        public int UserId { get; set; }

        /// <summary>Username for user</summary>
        public string Username { get; set; }

        /// <summary>email addres for user</summary>
        public string? Email { get; set; }

        /// <summary>User First Name</summary>
        public string FirstName { get; set; }

        /// <summary>User Last Name</summary>
        public string LastName { get; set; }

        /// <summary>User Date of Birth</summary>
        public DateTime? Dob { get; set; }

        /// <summary>User Phone</summary>
        public string? Phone { get; set; }

        /// <summary>User Image</summary>
        public byte[]? ProfileImage { get; set; }

        /// <summary>Facebook Claims</summary>
        public string? FacebookInfo { get; set; }

        /// <summary>Twitter Claims</summary>
        public string? TwitterInfo { get; set; }

        /// <summary>Google Claims</summary>
        public string? GoogleInfo { get; set; }

        /// <summary>MyCom Claims</summary>
        public string? MycomInfo { get; set; }

        /// <summary>Role Id (FK to Roles)</summary>
        public int? RoleId { get; set; }

        /// <summary></summary>
        public bool? IsNotification { get; set; }

        /// <summary></summary>
        public bool? IsFeatured { get; set; }

        /// <summary>User Statuses</summary>
        public enum UserStatuses {
            /// <summary>active</summary>
            active,
            /// <summary>inactive</summary>
            inactive,
            /// <summary>blocked</summary>
            blocked,
            /// <summary>reset</summary>
            reset
            }

        /// <summary>status</summary>
        public UserStatuses Status { get; set; }

        /// <summary></summary>
        public string? DeviceInfo { get; set; }

        /// <summary>Created</summary>
        public DateTime CreateDate { get; set; }

        /// <summary>Master User Id from SSO</summary>
        public string MasterUserId { get; set; }

        /// <summary>Profile ImageType</summary>
        public string? ProfileImageType { get; set; }
    }
}
