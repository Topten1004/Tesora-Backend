// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using Npgsql;
using NpgsqlTypes;

using NFTDatabaseEntities;


namespace NFTDatabase.DataAccess
{
    internal partial class PostgreSql : IPostgreSql
    {
        /// <summary>
        /// Create a new record in User
        /// </summary>
        /// <param name="record">User</param>
        /// <returns></returns>
       public async Task CreateUser(User record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "insert into tesora_nft.users" +
                              " (username,email,first_name,last_name,dob,phone,profile_image,facebook_info,twitter_info,google_info,mycom_info,role_id,is_notification,is_featured,status,device_info,create_date,master_user_id,profile_image_type)" +
                              " values" +
                              " (@username,@email,@first_name,@last_name,@dob,@phone,@profile_image,@facebook_info,@twitter_info,@google_info,@mycom_info,@role_id,@is_notification,@is_featured,@status,@device_info,@create_date,@master_user_id,@profile_image_type)";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@username", NpgsqlDbType.Varchar).Value = record.Username;
                    cmd.Parameters.Add("@email", NpgsqlDbType.Varchar).Value = record.Email ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@first_name", NpgsqlDbType.Varchar).Value = record.FirstName;
                    cmd.Parameters.Add("@last_name", NpgsqlDbType.Varchar).Value = record.LastName;
                    cmd.Parameters.Add("@dob", NpgsqlDbType.TimestampTz).Value = record.Dob ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@phone", NpgsqlDbType.Varchar).Value = record.Phone ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@profile_image", NpgsqlDbType.Bytea).Value = record.ProfileImage ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@facebook_info", NpgsqlDbType.Jsonb).Value = record.FacebookInfo ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@twitter_info", NpgsqlDbType.Jsonb).Value = record.TwitterInfo ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@google_info", NpgsqlDbType.Jsonb).Value = record.GoogleInfo ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@mycom_info", NpgsqlDbType.Jsonb).Value = record.MycomInfo ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@role_id", NpgsqlDbType.Integer).Value = record.RoleId ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@is_notification", NpgsqlDbType.Boolean).Value = record.IsNotification ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@is_featured", NpgsqlDbType.Boolean).Value = record.IsFeatured ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@status", NpgsqlDbType.Unknown).Value = record.Status.ToString();
                    cmd.Parameters.Add("@device_info", NpgsqlDbType.Jsonb).Value = record.DeviceInfo ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@create_date", NpgsqlDbType.TimestampTz).Value = DateTime.UtcNow;
                    cmd.Parameters.Add("@master_user_id", NpgsqlDbType.Varchar).Value = record.MasterUserId;
                    cmd.Parameters.Add("@profile_image_type", NpgsqlDbType.Varchar).Value = record.ProfileImageType ?? (object)DBNull.Value;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        /// <summary>
        /// Retrieve all User records
        /// </summary>
        /// <returns>List of User records</returns>
       public async Task<List<User>> RetrieveUsers()
        {
            var lstUser = new List<User>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select user_id,username,email,first_name,last_name,dob,phone,profile_image,facebook_info,twitter_info,google_info,mycom_info,role_id,is_notification,is_featured,status,device_info,create_date,master_user_id,profile_image_type" +
                              " from tesora_nft.users";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lstUser.Add(new User
                            {
                                UserId = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Email = reader.IsDBNull(2) ? null : (string?)reader.GetString(2),
                                FirstName = reader.GetString(3),
                                LastName = reader.GetString(4),
                                Dob = reader.IsDBNull(5) ? null : (DateTime?)reader.GetDateTime(5),
                                Phone = reader.IsDBNull(6) ? null : (string?)reader.GetString(6),
                                FacebookInfo = reader.IsDBNull(8) ? null : (string?)reader.GetString(8),
                                TwitterInfo = reader.IsDBNull(9) ? null : (string?)reader.GetString(9),
                                GoogleInfo = reader.IsDBNull(10) ? null : (string?)reader.GetString(10),
                                MycomInfo = reader.IsDBNull(11) ? null : (string?)reader.GetString(11),
                                RoleId = reader.IsDBNull(12) ? null : (int?)reader.GetInt32(12),
                                IsNotification = reader.IsDBNull(13) ? null : (bool?)reader.GetBoolean(13),
                                IsFeatured = reader.IsDBNull(14) ? null : (bool?)reader.GetBoolean(14),
                                Status = (User.UserStatuses)Enum.Parse(typeof(User.UserStatuses), reader.GetString(15)),
                                DeviceInfo = reader.IsDBNull(16) ? null : (string?)reader.GetString(16),
                                CreateDate = reader.GetDateTime(17),
                                MasterUserId = reader.GetString(18),
                                ProfileImageType = reader.IsDBNull(19) ? null : (string?)reader.GetString(19),
                                ProfileImage = ReadBytes(reader, 7)
                            });
                        }
                    }
                }
            }

            return lstUser;
        }


        /// <summary>
        /// Retrieve a User record
        /// </summary>
        /// <param name="userId">Primary Key</param>
        /// <returns></returns>
       public async Task<User?> RetrieveUser(int userId)
        {
            User? user = default;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select user_id,username,email,first_name,last_name,dob,phone,profile_image,facebook_info,twitter_info,google_info,mycom_info,role_id,is_notification,is_featured,status,device_info,create_date,master_user_id,profile_image_type" +
                              " from tesora_nft.users" +
                              " where user_id = @user_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@user_id", NpgsqlDbType.Integer).Value = userId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = new User
                            {
                                UserId = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Email = reader.IsDBNull(2) ? null : (string?)reader.GetString(2),
                                FirstName = reader.GetString(3),
                                LastName = reader.GetString(4),
                                Dob = reader.IsDBNull(5) ? null : (DateTime?)reader.GetDateTime(5),
                                Phone = reader.IsDBNull(6) ? null : (string?)reader.GetString(6),
                                FacebookInfo = reader.IsDBNull(8) ? null : (string?)reader.GetString(8),
                                TwitterInfo = reader.IsDBNull(9) ? null : (string?)reader.GetString(9),
                                GoogleInfo = reader.IsDBNull(10) ? null : (string?)reader.GetString(10),
                                MycomInfo = reader.IsDBNull(11) ? null : (string?)reader.GetString(11),
                                RoleId = reader.IsDBNull(12) ? null : (int?)reader.GetInt32(12),
                                IsNotification = reader.IsDBNull(13) ? null : (bool?)reader.GetBoolean(13),
                                IsFeatured = reader.IsDBNull(14) ? null : (bool?)reader.GetBoolean(14),
                                Status = (User.UserStatuses)Enum.Parse(typeof(User.UserStatuses), reader.GetString(15)),
                                DeviceInfo = reader.IsDBNull(16) ? null : (string?)reader.GetString(16),
                                CreateDate = reader.GetDateTime(17),
                                MasterUserId = reader.GetString(18),
                                ProfileImageType = reader.IsDBNull(19) ? null : (string?)reader.GetString(19),
                                ProfileImage = ReadBytes(reader, 7)
                            };
                        }
                    }
                }
            }

            return user;
        }


        /// <summary>
        /// Retrieve a User record
        /// </summary>
        /// <param name="masterUserId">Primary Key</param>
        /// <returns></returns>
        public async Task<User?> RetrieveUserMasterId(string masterUserId)
        {
            User? user = default;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select user_id,username,email,first_name,last_name,dob,phone,profile_image,facebook_info,twitter_info,google_info,mycom_info,role_id,is_notification,is_featured,status,device_info,create_date,master_user_id,profile_image_type" +
                              " from tesora_nft.users" +
                              " where master_user_id = @master_user_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@master_user_id", NpgsqlDbType.Varchar).Value = masterUserId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = new User
                            {
                                UserId = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Email = reader.IsDBNull(2) ? null : (string?)reader.GetString(2),
                                FirstName = reader.GetString(3),
                                LastName = reader.GetString(4),
                                Dob = reader.IsDBNull(5) ? null : (DateTime?)reader.GetDateTime(5),
                                Phone = reader.IsDBNull(6) ? null : (string?)reader.GetString(6),
                                FacebookInfo = reader.IsDBNull(8) ? null : (string?)reader.GetString(8),
                                TwitterInfo = reader.IsDBNull(9) ? null : (string?)reader.GetString(9),
                                GoogleInfo = reader.IsDBNull(10) ? null : (string?)reader.GetString(10),
                                MycomInfo = reader.IsDBNull(11) ? null : (string?)reader.GetString(11),
                                RoleId = reader.IsDBNull(12) ? null : (int?)reader.GetInt32(12),
                                IsNotification = reader.IsDBNull(13) ? null : (bool?)reader.GetBoolean(13),
                                IsFeatured = reader.IsDBNull(14) ? null : (bool?)reader.GetBoolean(14),
                                Status = (User.UserStatuses)Enum.Parse(typeof(User.UserStatuses), reader.GetString(15)),
                                DeviceInfo = reader.IsDBNull(16) ? null : (string?)reader.GetString(16),
                                CreateDate = reader.GetDateTime(17),
                                MasterUserId = reader.GetString(18),
                                ProfileImageType = reader.IsDBNull(19) ? null : (string?)reader.GetString(19),
                                ProfileImage = ReadBytes(reader, 7)
                            };
                        }
                    }
                }
            }

            return user;
        }


        /// <summary>
        /// Update a User record
        /// </summary>
        /// <param name="record">User</param>
        /// <returns></returns>
        public async Task UpdateUser(User record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "update tesora_nft.users set" +
                              " username = @username," +
                              " email = @email," +
                              " first_name = @first_name," +
                              " last_name = @last_name," +
                              " dob = @dob," +
                              " phone = @phone," +
                              " profile_image = @profile_image," +
                              " facebook_info = @facebook_info," +
                              " twitter_info = @twitter_info," +
                              " google_info = @google_info," +
                              " mycom_info = @mycom_info," +
                              " role_id = @role_id," +
                              " is_notification = @is_notification," +
                              " is_featured = @is_featured," +
                              " status = @status," +
                              " device_info = @device_info," +
                              " create_date = @create_date," +
                              " master_user_id = @master_user_id," +
                              " profile_image_type = @profile_image_type" +
                              " where user_id = @user_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@user_id", NpgsqlDbType.Integer).Value = record.UserId;
                    cmd.Parameters.Add("@username", NpgsqlDbType.Varchar).Value = record.Username;
                    cmd.Parameters.Add("@email", NpgsqlDbType.Varchar).Value = record.Email ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@first_name", NpgsqlDbType.Varchar).Value = record.FirstName;
                    cmd.Parameters.Add("@last_name", NpgsqlDbType.Varchar).Value = record.LastName;
                    cmd.Parameters.Add("@dob", NpgsqlDbType.TimestampTz).Value = record.Dob ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@phone", NpgsqlDbType.Varchar).Value = record.Phone ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@profile_image", NpgsqlDbType.Bytea).Value = record.ProfileImage ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@facebook_info", NpgsqlDbType.Jsonb).Value = record.FacebookInfo ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@twitter_info", NpgsqlDbType.Jsonb).Value = record.TwitterInfo ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@google_info", NpgsqlDbType.Jsonb).Value = record.GoogleInfo ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@mycom_info", NpgsqlDbType.Jsonb).Value = record.MycomInfo ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@role_id", NpgsqlDbType.Integer).Value = record.RoleId ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@is_notification", NpgsqlDbType.Boolean).Value = record.IsNotification ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@is_featured", NpgsqlDbType.Boolean).Value = record.IsFeatured ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@status", NpgsqlDbType.Unknown).Value = record.Status.ToString();
                    cmd.Parameters.Add("@device_info", NpgsqlDbType.Jsonb).Value = record.DeviceInfo ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@create_date", NpgsqlDbType.TimestampTz).Value = DateTime.UtcNow;
                    cmd.Parameters.Add("@master_user_id", NpgsqlDbType.Varchar).Value = record.MasterUserId;
                    cmd.Parameters.Add("@profile_image_type", NpgsqlDbType.Varchar).Value = record.ProfileImageType ?? (object)DBNull.Value;
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        /// <summary>
        /// Delete a new record in users
        /// </summary>
        /// <param name="userId">Primary Key</param>
        /// <returns></returns>
        public async Task DeleteUser(int userId)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "delete from tesora_nft.users where user_id = @user_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@user_id", NpgsqlDbType.Integer).Value = userId;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        /// <summary>
        /// User exists given the master user id
        /// </summary>
        /// <param name="masterUserId">Master User Id</param>
        /// <returns></returns>
        public async Task<bool> UserExists(string masterUserId)
        {
            bool exists = false;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select count(*) from tesora_nft.users where master_user_id = @master_user_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@master_user_id", NpgsqlDbType.Varchar).Value = masterUserId;

                    var result = await cmd.ExecuteScalarAsync();

                    if (result != null && (Int64)result > 0)
                        exists = true;
                }
            }

            return exists;
        }

        /// <summary>
        /// Retrieve a User Image
        /// </summary>
        /// <param name="userId">Primary Key</param>
        /// <returns></returns>
        public async Task<ImageBox?> RetrieveUserImage(int userId)
        {
            ImageBox? imageBox = null;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select profile_image,profile_image_type from tesora_nft.users where user_id = @user_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@user_id", NpgsqlDbType.Integer).Value = userId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            imageBox = new ImageBox
                            {
                                Data = ReadBytes(reader, 0),
                                Type = reader.GetString(1)
                            };
                        }
                    }
                }
            }

            return imageBox;
        }


        /// <summary>
        /// Update a User Image
        /// </summary>
        /// <param name="record">User Image info</param>
        /// <returns></returns>
        public async Task UpdateUserImage(User record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "update tesora_nft.users set" +
                              " profile_image = @profile_image," +
                              " profile_image_type = @profile_image_type" +
                              " where user_id = @user_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@user_id", NpgsqlDbType.Integer).Value = record.UserId;

                    cmd.Parameters.Add("@profile_image", NpgsqlDbType.Bytea).Value = record.ProfileImage ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@profile_image_type", NpgsqlDbType.Varchar).Value = record.ProfileImageType ?? (object)DBNull.Value;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }



    }
}

