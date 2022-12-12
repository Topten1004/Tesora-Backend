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
        /// Create a new record in Option
        /// </summary>
        /// <param name="record">Option</param>
        /// <returns></returns>
       public async Task CreateOption(Option record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "insert into tesora_nft.options" +
                              " (name,value)" +
                              " values" +
                              " (@name,@value)";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@name", NpgsqlDbType.Varchar).Value = record.Name;
                    cmd.Parameters.Add("@value", NpgsqlDbType.Varchar).Value = record.Value;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        /// <summary>
        /// Retrieve all Option records
        /// </summary>
        /// <returns>List of Option records</returns>
       public async Task<List<Option>> RetrieveOptions()
        {
            var lstOption = new List<Option>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select option_id,name,value" +
                              " from tesora_nft.options";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        Option record;

                        while (await reader.ReadAsync())
                        {
                            record = new Option
                            {
                                OptionId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Value = reader.IsDBNull(2) ? null  : (string?)reader.GetString(2),
                            };


                        lstOption.Add(record);
                        }
                    }
                }
            }

            return lstOption;
        }


        /// <summary>
        /// Retrieve a Option record
        /// </summary>
        /// <param name="optionId">Primary Key</param>
        /// <returns></returns>
       public async Task<Option?> RetrieveOption(int optionId)
        {
            Option? option = default;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select option_id,name,value" +
                              " from tesora_nft.options" +
                              " where option_id = @option_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@option_id", NpgsqlDbType.Integer).Value = optionId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            option = new Option
                            {
                                OptionId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Value = reader.IsDBNull(2) ? null  : (string?)reader.GetString(2),
                            };

                        }
                    }
                }
            }

            return option;
        }


        /// <summary>
        /// Update a Option record
        /// </summary>
        /// <param name="record">Option</param>
        /// <returns></returns>
       public async Task UpdateOption(Option record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "update tesora_nft.options set" +
                              " name = @name," +
                              " value = @value" +
                              " where option_id = @option_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@option_id", NpgsqlDbType.Integer).Value = record.OptionId;
                    cmd.Parameters.Add("@name", NpgsqlDbType.Varchar).Value = record.Name;
                    cmd.Parameters.Add("@value", NpgsqlDbType.Varchar).Value = record.Value;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        /// <summary>
        /// Delete a new record in options
        /// </summary>
        /// <param name="optionId">Primary Key</param>
        /// <returns></returns>
        public async Task DeleteOption(int optionId)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "delete from tesora_nft.options where option_id = @option_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@option_id", NpgsqlDbType.Integer).Value = optionId;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}

