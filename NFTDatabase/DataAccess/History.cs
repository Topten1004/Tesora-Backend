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
        /// Create a new record in History
        /// </summary>
        /// <param name="record">History</param>
        /// <returns></returns>
       public async Task CreateHistory(History record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "insert into tesora_nft.histories" +
                              " (item_id,collection_id,from_id,to_id,transaction_hash,price,currency,history_type,is_valid,create_date)" +
                              " values" +
                              " (@item_id,@collection_id,@from_id,@to_id,@transaction_hash,@price,@currency,@history_type,@is_valid,@create_date)";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = record.ItemId;
                    cmd.Parameters.Add("@collection_id", NpgsqlDbType.Integer).Value = record.CollectionId;
                    cmd.Parameters.Add("@from_id", NpgsqlDbType.Integer).Value = record.FromId;
                    cmd.Parameters.Add("@to_id", NpgsqlDbType.Integer).Value = record.ToId;
                    cmd.Parameters.Add("@transaction_hash", NpgsqlDbType.Varchar).Value = record.TransactionHash;
                    cmd.Parameters.Add("@price", NpgsqlDbType.Numeric).Value = record.Price;
                    cmd.Parameters.Add("@currency", NpgsqlDbType.Varchar).Value = record.Currency;
                    cmd.Parameters.Add("@history_type", NpgsqlDbType.Unknown).Value = record.HistoryType.ToString();
                    cmd.Parameters.Add("@is_valid", NpgsqlDbType.Boolean).Value = record.IsValid;
                    cmd.Parameters.Add("@create_date", NpgsqlDbType.TimestampTz).Value = record.CreateDate;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        /// <summary>
        /// Retrieve all History records
        /// </summary>
        /// <returns>List of History records</returns>
       public async Task<List<History>> RetrieveHistories()
        {
            var lstHistory = new List<History>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select history_id,item_id,collection_id,from_id,to_id,transaction_hash,price,currency,history_type,is_valid,create_date" +
                              " from tesora_nft.histories";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        History record;

                        while (await reader.ReadAsync())
                        {
                            record = new History
                            {
                                HistoryId = reader.GetInt32(0),
                                ItemId = reader.IsDBNull(1) ? null : (int?)reader.GetInt32(1),
                                CollectionId = reader.IsDBNull(2) ? null : (int?)reader.GetInt32(2),
                                FromId = reader.IsDBNull(3) ? null : (int?)reader.GetInt32(3),
                                ToId = reader.IsDBNull(4) ? null : (int?)reader.GetInt32(4),
                                TransactionHash = reader.IsDBNull(5) ? null  : (string?)reader.GetString(5),
                                Price = reader.IsDBNull(6) ? null : (Decimal?)reader.GetDecimal(6),
                                Currency = reader.IsDBNull(7) ? null : (string?)reader.GetString(7),
                                HistoryType = (History.HistoryTypes)Enum.Parse(typeof(History.HistoryTypes), reader.GetString(8)),
                                IsValid = reader.IsDBNull(9) ? null  : (bool?)reader.GetBoolean(9),
                                CreateDate = reader.GetDateTime(10),
                            };


                        lstHistory.Add(record);
                        }
                    }
                }
            }

            return lstHistory;
        }


        /// <summary>
        /// Retrieve a History record
        /// </summary>
        /// <param name="historyId">Primary Key</param>
        /// <returns></returns>
       public async Task<History?> RetrieveHistory(int historyId)
        {
            History? history = default;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select history_id,item_id,collection_id,from_id,to_id,transaction_hash,price,currency,history_type,is_valid,create_date" +
                              " from tesora_nft.histories" +
                              " where history_id = @history_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@history_id", NpgsqlDbType.Integer).Value = historyId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            history = new History
                            {
                                HistoryId = reader.GetInt32(0),
                                ItemId = reader.IsDBNull(1) ? null : (int)reader.GetInt32(1),
                                CollectionId = reader.IsDBNull(2) ? null : (int)reader.GetInt32(2),
                                FromId = reader.IsDBNull(3) ? null : (int)reader.GetInt32(3),
                                ToId = reader.IsDBNull(4) ? null : (int)reader.GetInt32(4),
                                TransactionHash = reader.IsDBNull(5) ? null : (string?)reader.GetString(5),
                                Price = reader.IsDBNull(6) ? null : (decimal?)reader.GetDecimal(6),
                                Currency = reader.IsDBNull(7) ? null : (string?)reader.GetString(7),
                                HistoryType = (History.HistoryTypes)Enum.Parse(typeof(History.HistoryTypes), reader.GetString(8)),
                                IsValid = reader.IsDBNull(9) ? null  : (bool?)reader.GetBoolean(9),
                                CreateDate = reader.GetDateTime(10),
                            };

                        }
                    }
                }
            }

            return history;
        }


        /// <summary>
        /// Update a History record
        /// </summary>
        /// <param name="record">History</param>
        /// <returns></returns>
       public async Task UpdateHistory(History record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "update tesora_nft.histories set" +
                              " item_id = @item_id," +
                              " collection_id = @collection_id," +
                              " from_id = @from_id," +
                              " to_id = @to_id," +
                              " transaction_hash = @transaction_hash," +
                              " price = @price," +
                              " currency = @currency," +
                              " history_type = @history_type," +
                              " is_valid = @is_valid," +
                              " create_date = @create_date" +
                              " where history_id = @history_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@history_id", NpgsqlDbType.Integer).Value = record.HistoryId;
                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = record.ItemId;
                    cmd.Parameters.Add("@collection_id", NpgsqlDbType.Integer).Value = record.CollectionId;
                    cmd.Parameters.Add("@from_id", NpgsqlDbType.Integer).Value = record.FromId;
                    cmd.Parameters.Add("@to_id", NpgsqlDbType.Integer).Value = record.ToId;
                    cmd.Parameters.Add("@transaction_hash", NpgsqlDbType.Varchar).Value = record.TransactionHash;
                    cmd.Parameters.Add("@price", NpgsqlDbType.Numeric).Value = record.Price;
                    cmd.Parameters.Add("@currency", NpgsqlDbType.Varchar).Value = record.Currency;
                    cmd.Parameters.Add("@history_type", NpgsqlDbType.Unknown).Value = record.HistoryType.ToString();
                    cmd.Parameters.Add("@is_valid", NpgsqlDbType.Boolean).Value = record.IsValid;
                    cmd.Parameters.Add("@create_date", NpgsqlDbType.TimestampTz).Value = record.CreateDate;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        /// <summary>
        /// Delete a new record in histories
        /// </summary>
        /// <param name="historyId">Primary Key</param>
        /// <returns></returns>
        public async Task DeleteHistory(int historyId)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "delete from tesora_nft.histories where history_id = @history_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@history_id", NpgsqlDbType.Integer).Value = historyId;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}

