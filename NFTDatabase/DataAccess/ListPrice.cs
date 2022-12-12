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
        /// Create a new record in ListPrice
        /// </summary>
        /// <param name="record">ListPrice</param>
        /// <returns></returns>
       public async Task CreateListPrice(ListPrice record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "insert into tesora_nft.list_prices" +
                              " (item_id,price,currency,user_id,create_date)" +
                              " values" +
                              " (@item_id,@price,@currency,@user_id,@create_date)";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = record.ItemId;
                    cmd.Parameters.Add("@price", NpgsqlDbType.Numeric).Value = record.Price;
                    cmd.Parameters.Add("@currency", NpgsqlDbType.Varchar).Value = record.Currency;
                    cmd.Parameters.Add("@user_id", NpgsqlDbType.Integer).Value = record.UserId;
                    cmd.Parameters.Add("@create_date", NpgsqlDbType.TimestampTz).Value = record.CreateDate;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        /// <summary>
        /// Retrieve all ListPrice records
        /// </summary>
        /// <returns>List of ListPrice records</returns>
       public async Task<List<ListPrice>> RetrieveListPrices()
        {
            var lstListPrice = new List<ListPrice>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select list_price_id,item_id,price,currency,user_id,create_date" +
                              " from tesora_nft.list_prices";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lstListPrice.Add(new ListPrice
                            {
                                ListPriceId = reader.GetInt32(0),
                                ItemId = reader.IsDBNull(1) ? null : (int?)reader.GetInt32(1),
                                Price = reader.IsDBNull(2) ? null : (Decimal?)reader.GetDecimal(2),
                                Currency = reader.IsDBNull(3) ? null : (string?)reader.GetString(3),
                                UserId = reader.IsDBNull(4) ? null : (int?)reader.GetInt32(4),
                                CreateDate = reader.GetDateTime(5),
                            });
                        }
                    }
                }
            }

            return lstListPrice;
        }


        /// <summary>
        /// Retrieve a ListPrice record
        /// </summary>
        /// <param name="listPriceId">Primary Key</param>
        /// <returns></returns>
       public async Task<ListPrice?> RetrieveListPrice(int listPriceId)
        {
            ListPrice? listPrice = default;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select list_price_id,item_id,price,currency,user_id,create_date" +
                              " from tesora_nft.list_prices" +
                              " where list_price_id = @list_price_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@list_price_id", NpgsqlDbType.Integer).Value = listPriceId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            listPrice = new ListPrice
                            {
                                ListPriceId = reader.GetInt32(0),
                                ItemId = reader.IsDBNull(1) ? null : (int?)reader.GetInt32(1),
                                Price = reader.IsDBNull(2) ? null : (decimal?)reader.GetDecimal(2),
                                Currency = reader.IsDBNull(3) ? null : (string?)reader.GetString(3),
                                UserId = reader.IsDBNull(4) ? null : (int?)reader.GetInt32(4),
                                CreateDate = reader.GetDateTime(5),
                            };

                        }
                    }
                }
            }

            return listPrice;
        }


        /// <summary>
        /// Update a ListPrice record
        /// </summary>
        /// <param name="record">ListPrice</param>
        /// <returns></returns>
       public async Task UpdateListPrice(ListPrice record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "update tesora_nft.list_prices set" +
                              " item_id = @item_id," +
                              " price = @price," +
                              " currency = @currency," +
                              " user_id = @user_id," +
                              " create_date = @create_date" +
                              " where list_price_id = @list_price_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@list_price_id", NpgsqlDbType.Integer).Value = record.ListPriceId;
                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = record.ItemId;
                    cmd.Parameters.Add("@price", NpgsqlDbType.Numeric).Value = record.Price;
                    cmd.Parameters.Add("@currency", NpgsqlDbType.Varchar).Value = record.Currency;
                    cmd.Parameters.Add("@user_id", NpgsqlDbType.Integer).Value = record.UserId;
                    cmd.Parameters.Add("@create_date", NpgsqlDbType.TimestampTz).Value = record.CreateDate;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        /// <summary>
        /// Delete a new record in list_prices
        /// </summary>
        /// <param name="listPriceId">Primary Key</param>
        /// <returns></returns>
        public async Task DeleteListPrice(int listPriceId)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "delete from tesora_nft.list_prices where list_price_id = @list_price_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@list_price_id", NpgsqlDbType.Integer).Value = listPriceId;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}

