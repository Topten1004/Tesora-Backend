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
        /// Create a new record in Cart
        /// </summary>
        /// <param name="record">Cart</param>
        /// <returns></returns>
        public async Task CreateCartItem(CartItem record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();


                string sSQL = "insert into tesora_nft.cart" +
                              " (ring,section,block,lot,usd_price,bitcoin_price,ethereum_price,tether_price,price_expiration,user_id,eur_price)" +
                              " values" +
                              " (@ring,@section,@block,@lot,@usd_price,@bitcoin_price,@ethereum_price,@tether_price,@price_expiration,@user_id,@eur_price)";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@ring", NpgsqlDbType.Varchar).Value = record.Ring;
                    cmd.Parameters.Add("@section", NpgsqlDbType.Varchar).Value = record.Section;
                    cmd.Parameters.Add("@block", NpgsqlDbType.Varchar).Value = record.Block;
                    cmd.Parameters.Add("@lot", NpgsqlDbType.Varchar).Value = record.Lot;
                    cmd.Parameters.Add("@usd_price", NpgsqlDbType.Numeric).Value = record.UsdPrice;
                    cmd.Parameters.Add("@bitcoin_price", NpgsqlDbType.Numeric).Value = record.BitcoinPrice;
                    cmd.Parameters.Add("@ethereum_price", NpgsqlDbType.Numeric).Value = record.EthereumPrice;
                    cmd.Parameters.Add("@tether_price", NpgsqlDbType.Numeric).Value = record.TetherPrice;
                    cmd.Parameters.Add("@price_expiration", NpgsqlDbType.TimestampTz).Value = record.PriceExpiration;
                    cmd.Parameters.Add("@user_id", NpgsqlDbType.Integer).Value = record.UserId;
                    cmd.Parameters.Add("@eur_price", NpgsqlDbType.Numeric).Value = record.EurPrice;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        /// <summary>
        /// Retrieve all Cart records for a user
        /// </summary>
        /// <returns>List of Cart records</returns>
        public async Task<List<CartItem>> RetrieveCartItems(int userId)
        {
            var lstCart = new List<CartItem>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select line_id,ring,section,block,lot,usd_price,bitcoin_price,ethereum_price,tether_price,price_expiration,user_id,eur_price" +
                              " from tesora_nft.cart" +
                              " where user_id = @user_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@user_id", NpgsqlDbType.Integer).Value = userId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lstCart.Add(new CartItem
                            {
                                LineId = reader.GetInt32(0),
                                Ring = reader.GetString(1),
                                Section = reader.GetString(2),
                                Block = reader.GetString(3),
                                Lot = reader.GetString(4),
                                UsdPrice = reader.GetDecimal(5),
                                BitcoinPrice = reader.IsDBNull(6) ? null : reader.GetDecimal(6),
                                EthereumPrice = reader.IsDBNull(7) ? null : reader.GetDecimal(7),
                                TetherPrice = reader.IsDBNull(8) ? null : reader.GetDecimal(8),
                                PriceExpiration = reader.GetDateTime(9),
                                UserId = reader.GetInt32(10),
                                EurPrice = reader.IsDBNull(11) ? null : reader.GetDecimal(11)
                            });
                        }
                    }
                }
            }

            return lstCart;
        }


        /// <summary>
        /// Retrieve a Cart record
        /// </summary>
        /// <param name="lineId">Primary Key</param>
        /// <returns></returns>
        public async Task<CartItem?> RetrieveCartItem(int lineId)
        {
            CartItem? cartItem = default;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select line_id,ring,section,block,lot,usd_price,bitcoin_price,ethereum_price,tether_price,price_expiration,user_id,eur_price" +
                              " from tesora_nft.cart" +
                              " where line_id = @line_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@line_id", NpgsqlDbType.Integer).Value = lineId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            cartItem = new CartItem
                            {
                                LineId = reader.GetInt32(0),
                                Ring = reader.GetString(1),
                                Section = reader.GetString(2),
                                Block = reader.GetString(3),
                                Lot = reader.GetString(4),
                                UsdPrice = reader.GetDecimal(5),
                                BitcoinPrice = reader.IsDBNull(6) ? null : reader.GetDecimal(6),
                                EthereumPrice = reader.IsDBNull(7) ? null : reader.GetDecimal(7),
                                TetherPrice = reader.IsDBNull(8) ? null : reader.GetDecimal(8),
                                PriceExpiration = reader.GetDateTime(9),
                                UserId = reader.GetInt32(10),
                                EurPrice = reader.IsDBNull(11) ? null : reader.GetDecimal(11)
                            };
                        }
                    }
                }
            }

            return cartItem;
        }

        /// <summary>
        /// Delete a cart item
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public async Task DeleteCartItem(int lineId)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "delete from tesora_nft.cart where line_id = @line_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@line_id", NpgsqlDbType.Integer).Value = lineId;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        /// <summary>
        /// Delete all cart items
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task DeleteCartItems(int userId)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "delete from tesora_nft.cart where user_id = @user_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@user_id", NpgsqlDbType.Integer).Value = userId;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

    }
}

