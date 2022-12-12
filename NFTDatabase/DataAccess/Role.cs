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
        /// Create a new record in Role
        /// </summary>
        /// <param name="record">Role</param>
        /// <returns></returns>
       public async Task CreateRole(Role record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "insert into tesora_nft.roles" +
                              " (name,read,write,delete)" +
                              " values" +
                              " (@name,@read,@write,@delete)";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@name", NpgsqlDbType.Varchar).Value = record.Name;
                    cmd.Parameters.Add("@read", NpgsqlDbType.Boolean).Value = record.Read;
                    cmd.Parameters.Add("@write", NpgsqlDbType.Boolean).Value = record.Write;
                    cmd.Parameters.Add("@delete", NpgsqlDbType.Boolean).Value = record.Delete;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        /// <summary>
        /// Retrieve all Role records
        /// </summary>
        /// <returns>List of Role records</returns>
       public async Task<List<Role>> RetrieveRoles()
        {
            var lstRole = new List<Role>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select role_id,name,read,write,delete" +
                              " from tesora_nft.roles";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        Role record;

                        while (await reader.ReadAsync())
                        {
                            record = new Role
                            {
                                RoleId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Read = reader.GetBoolean(2),
                                Write = reader.GetBoolean(3),
                                Delete = reader.GetBoolean(4),
                            };


                        lstRole.Add(record);
                        }
                    }
                }
            }

            return lstRole;
        }


        /// <summary>
        /// Retrieve a Role record
        /// </summary>
        /// <param name="roleId">Primary Key</param>
        /// <returns></returns>
       public async Task<Role?> RetrieveRole(int roleId)
        {
            Role? role = default;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select role_id,name,read,write,delete" +
                              " from tesora_nft.roles" +
                              " where role_id = @role_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@role_id", NpgsqlDbType.Integer).Value = roleId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            role = new Role
                            {
                                RoleId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Read = reader.GetBoolean(2),
                                Write = reader.GetBoolean(3),
                                Delete = reader.GetBoolean(4),
                            };

                        }
                    }
                }
            }

            return role;
        }


        /// <summary>
        /// Update a Role record
        /// </summary>
        /// <param name="record">Role</param>
        /// <returns></returns>
       public async Task UpdateRole(Role record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "update tesora_nft.roles set" +
                              " name = @name," +
                              " read = @read," +
                              " write = @write," +
                              " delete = @delete" +
                              " where role_id = @role_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@role_id", NpgsqlDbType.Integer).Value = record.RoleId;
                    cmd.Parameters.Add("@name", NpgsqlDbType.Varchar).Value = record.Name;
                    cmd.Parameters.Add("@read", NpgsqlDbType.Boolean).Value = record.Read;
                    cmd.Parameters.Add("@write", NpgsqlDbType.Boolean).Value = record.Write;
                    cmd.Parameters.Add("@delete", NpgsqlDbType.Boolean).Value = record.Delete;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        /// <summary>
        /// Delete a new record in roles
        /// </summary>
        /// <param name="roleId">Primary Key</param>
        /// <returns></returns>
        public async Task DeleteRole(int roleId)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "delete from tesora_nft.roles where role_id = @role_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@role_id", NpgsqlDbType.Integer).Value = roleId;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}

