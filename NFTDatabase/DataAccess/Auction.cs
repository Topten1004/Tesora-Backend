// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using Npgsql;
using NpgsqlTypes;

using NFTDatabaseEntities;
using System.ComponentModel.DataAnnotations;

namespace NFTDatabase.DataAccess
{
    internal partial class PostgreSql : IPostgreSql
    {
        /// <summary>
        /// Create a new record in Auction
        /// </summary>
        /// <param name="record">Auction</param>
        /// <returns></returns>
        public async Task CreateAuction(Auction record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "insert into tesora_nft.auctions" +
                              " (item_id,sender_id,receiver_id,price,currency,create_date)" +
                              " values" +
                              " (@item_id,@sender_id,@receiver_id,@price,@currency,@create_date)";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = record.ItemId;
                    cmd.Parameters.Add("@sender_id", NpgsqlDbType.Integer).Value = record.SenderId;
                    cmd.Parameters.Add("@receiver_id", NpgsqlDbType.Integer).Value = record.ReceiverId;
                    cmd.Parameters.Add("@price", NpgsqlDbType.Numeric).Value = record.Price;
                    cmd.Parameters.Add("@currency", NpgsqlDbType.Varchar).Value = record.Currency;
                    cmd.Parameters.Add("@create_date", NpgsqlDbType.TimestampTz).Value = record.CreateDate;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        /// <summary>
        /// Retrieve a Auction record
        /// </summary>
        /// <param name="auctionId">Primary Key</param>
        /// <returns></returns>
        public async Task<Auction?> RetrieveAuction(int auctionId)
        {
            Auction? auction = default;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select auction_id,item_id,sender_id,receiver_id,price,currency,create_date" +
                              " from tesora_nft.auctions" +
                              " where auction_id = @auction_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@auction_id", NpgsqlDbType.Integer).Value = auctionId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            auction = new Auction
                            {
                                AuctionId = reader.GetInt32(0),
                                ItemId = reader.IsDBNull(1) ? null : (int)reader.GetInt32(1),
                                SenderId = reader.IsDBNull(2) ? null : (int)reader.GetInt32(2),
                                ReceiverId = reader.IsDBNull(3) ? null : (int)reader.GetInt32(3),
                                Price = reader.IsDBNull(4) ? null : (Decimal?)reader.GetDecimal(4),
                                Currency = reader.IsDBNull(5) ? null : (string?)reader.GetString(5),
                                CreateDate = reader.GetDateTime(6)
                            };
                        }
                    }
                }
            }

            return auction;
        }

        /// <summary>
        /// Retrieve a Auction record
        /// </summary>
        /// <param name="itemId">Primary Key of the Item</param>
        /// <returns>List of Auctions</returns>
        public async Task<List<Auction>> RetrieveAuctionsByItemId(int itemId)
        {
            var auctions = new List<Auction>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select auction_id,item_id,sender_id,receiver_id,price,currency,create_date" +
                              " from tesora_nft.auctions" +
                              " where item_id = @item_id" +
                              " order by price asc";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = itemId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            auctions.Add(new Auction
                            {
                                AuctionId = reader.GetInt32(0),
                                ItemId = reader.IsDBNull(1) ? null : (int)reader.GetInt32(1),
                                SenderId = reader.IsDBNull(2) ? null : (int)reader.GetInt32(2),
                                ReceiverId = reader.IsDBNull(3) ? null : (int)reader.GetInt32(3),
                                Price = reader.IsDBNull(4) ? null : (Decimal?)reader.GetDecimal(4),
                                Currency = reader.IsDBNull(5) ? null : (string?)reader.GetString(5),
                                CreateDate = reader.GetDateTime(6)
                            });
                        }
                    }
                }
            }

            return auctions;
        }


        /// <summary>
        /// Retrieve all my auction records
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List of my auction records</returns>
        public async Task<List<AuctionUserCollectionItemCategory>> RetrieveMyAuctions(int userId)
        {
            var lstAuction = new List<AuctionUserCollectionItemCategory>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select o.auction_id,o.price,o.currency,o.create_date,i.item_id,i.name,i.price,i.currency,u.user_id,u.username,u.first_name,u.last_name,c.name" +
                              " from tesora_nft.auctions o" +
                              " join tesora_nft.items i on i.item_id = o.item_id" +
                              " join tesora_nft.users u on u.user_id = o.sender_id" +
                              " join tesora_nft.collections c on c.collection_id = i.collection_id" +
                              " where o.receiver_id = @receiver_id and i.enable_auction = true and i.start_date <= o.create_date and i.end_date >= o.create_date" +
                              " order by c.collection_id asc, i.item_id asc, o.auction_id desc";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@receiver_id", NpgsqlDbType.Integer).Value = userId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lstAuction.Add(new AuctionUserCollectionItemCategory
                            {
                                AuctionId = reader.GetInt32(0),
                                Price = reader.IsDBNull(1) ? null : (decimal?)reader.GetDecimal(1),
                                Currency = reader.IsDBNull(2) ? null : (string?)reader.GetString(2),
                                CreateDate = reader.GetDateTime(3),
                                Item = new Item
                                {
                                    ItemId = reader.GetInt32(4),
                                    Name = reader.GetString(5),
                                    Price = reader.IsDBNull(6) ? null : (decimal?)reader.GetDecimal(6),
                                    Currency = reader.IsDBNull(7) ? null : (string?)reader.GetString(7)
                                },
                                Sender = new User
                                {
                                    UserId = reader.GetInt32(8),
                                    Username = reader.GetString(9),
                                    FirstName = reader.GetString(10),
                                    LastName = reader.GetString(11)
                                },
                                Collection = new Collection
                                {
                                    Name = reader.GetString(12)
                                }
                            });
                        }
                    }
                }
            }

            return lstAuction;
        }

        /// <summary>
        /// Get my auction
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="auctionId">Auction Id</param>
        /// <returns>List of my auction records</returns>
        public async Task<AuctionUserCollectionItemCategory?> RetrieveMyAuction(int userId, int auctionId)
        {
            AuctionUserCollectionItemCategory? auction = null;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select o.auction_id,o.price,o.currency,o.create_date" +
                              ",i.item_id,i.name,i.price,i.currency,i.media_ipfs,i.description,i.token_id" +
                              ",u.user_id,u.username,u.first_name,u.last_name,u.master_user_id" +
                              ",c.collection_id,c.name" +
                              ",g.category_id,g.title" +
                              " from tesora_nft.auctions o" +
                              " join tesora_nft.items i on i.item_id = o.item_id" +
                              " join tesora_nft.users u on u.user_id = o.sender_id" +
                              " join tesora_nft.collections c on c.collection_id = i.collection_id" +
                              " join tesora_nft.categories g on g.category_id = i.category_id" +
                              " where o.receiver_id = @receiver_id and i.enable_auction = true and i.start_date <= o.create_date and i.end_date >= o.create_date and o.auction_id = @auction_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@receiver_id", NpgsqlDbType.Integer).Value = userId;
                    cmd.Parameters.Add("@auction_id", NpgsqlDbType.Integer).Value = auctionId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            auction = new AuctionUserCollectionItemCategory
                            {
                                AuctionId = reader.GetInt32(0),
                                Price = reader.IsDBNull(1) ? null : (decimal?)reader.GetDecimal(1),
                                Currency = reader.IsDBNull(2) ? null : (string?)reader.GetString(2),
                                CreateDate = reader.GetDateTime(3),
                                Item = new Item
                                {
                                    ItemId = reader.GetInt32(4),
                                    Name = reader.GetString(5),
                                    Price = reader.IsDBNull(6) ? null : (decimal?)reader.GetDecimal(6),
                                    Currency = reader.IsDBNull(7) ? null : (string?)reader.GetString(7),
                                    MediaIpfs = reader.IsDBNull(8) ? null : (string?)reader.GetString(8),
                                    Description = reader.IsDBNull(9) ? null : (string?)reader.GetString(9),
                                    TokenId = (int?)reader.GetInt32(10)
                                },
                                Sender = new User
                                {
                                    UserId = reader.GetInt32(11),
                                    Username = reader.GetString(12),
                                    FirstName = reader.GetString(13),
                                    LastName = reader.GetString(14),
                                    MasterUserId = reader.GetString(15)
                                },
                                Collection = new Collection
                                {
                                    CollectionId = reader.GetInt32(16),
                                    Name = reader.GetString(17)
                                },
                                Category = new Category
                                {
                                    CategoryId = reader.GetInt32(18),
                                    Title = reader.GetString(19)
                                }
                            };
                        }
                    }
                }
            }

            return auction;
        }

        /// <summary>
        /// Accept my auction
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="auctionId">Auction Id</param>
        /// <param name="txHash">Transaction Hash</param>
        /// <returns></returns>
        public async Task AcceptMyAuction(int userId, int auctionId, string txHash)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL;
                int receiverId;
                int senderId;
                int itemId;
                decimal price;
                string currency;

                // Get data needed
                sSQL = "select receiver_id,sender_id,item_id,price,currency from tesora_nft.auctions where auction_id = @auction_id";
                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@auction_id", NpgsqlDbType.Integer).Value = auctionId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            receiverId = reader.GetInt32(0);
                            senderId = reader.GetInt32(1);
                            itemId = reader.GetInt32(2);
                            price = reader.GetDecimal(3);
                            currency = reader.GetString(4);
                        }
                        else
                            throw new Exception("Invalid auction id");
                    }
                }

                // Validate that request was not tampered with
                if (receiverId != userId)
                    throw new Exception("The auction is not for this user");

                // Terminate auction and move item to new owner
                sSQL = "update tesora_nft.items set enable_auction = @enable_auction, status = @status, current_owner = @current_owner where item_id = @item_id";
                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@enable_auction", NpgsqlDbType.Boolean).Value = false;
                    cmd.Parameters.Add("@status", NpgsqlDbType.Unknown).Value = Item.ItemStatuses.inactive.ToString();
                    cmd.Parameters.Add("@current_owner", NpgsqlDbType.Integer).Value = senderId;

                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = itemId;

                    await cmd.ExecuteNonQueryAsync();
                }

                // Delete auctions when auction ends
                sSQL = "delete from tesora_nft.auctions where item_id = @item_id and receiver_id = @receiver_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = itemId;
                    cmd.Parameters.Add("@receiver_id", NpgsqlDbType.Integer).Value = receiverId;

                    await cmd.ExecuteNonQueryAsync();
                }

                // Make history entry
                sSQL = "insert into tesora_nft.histories" +
                        " (item_id,collection_id,from_id,to_id,transaction_hash,price,currency,history_type,is_valid,create_date)" +
                        " values" +
                        " (@item_id,@collection_id,@from_id,@to_id,@transaction_hash,@price,@currency,@history_type,@is_valid,@create_date)";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = itemId;
                    cmd.Parameters.Add("@collection_id", NpgsqlDbType.Integer).Value = (object)DBNull.Value;
                    cmd.Parameters.Add("@from_id", NpgsqlDbType.Integer).Value = userId;
                    cmd.Parameters.Add("@to_id", NpgsqlDbType.Integer).Value = senderId;
                    cmd.Parameters.Add("@transaction_hash", NpgsqlDbType.Varchar).Value = txHash;
                    cmd.Parameters.Add("@price", NpgsqlDbType.Numeric).Value = price;
                    cmd.Parameters.Add("@currency", NpgsqlDbType.Varchar).Value = currency;
                    cmd.Parameters.Add("@history_type", NpgsqlDbType.Unknown).Value = History.HistoryTypes.transfer.ToString();
                    cmd.Parameters.Add("@is_valid", NpgsqlDbType.Boolean).Value = true;
                    cmd.Parameters.Add("@create_date", NpgsqlDbType.TimestampTz).Value = DateTime.UtcNow;

                    await cmd.ExecuteNonQueryAsync();
                }
            }

        }


        /// <summary>
        /// Update a Auction record
        /// </summary>
        /// <param name="record">Auction</param>
        /// <returns></returns>
        public async Task UpdateAuction(Auction record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "update tesora_nft.auctions set" +
                              " item_id = @item_id," +
                              " sender_id = @sender_id," +
                              " receiver_id = @receiver_id," +
                              " price = @price," +
                              " currency = @currency," +
                              " create_date = @create_date" +
                              " where auction_id = @auction_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@auction_id", NpgsqlDbType.Integer).Value = record.AuctionId;
                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = record.ItemId;
                    cmd.Parameters.Add("@sender_id", NpgsqlDbType.Integer).Value = record.SenderId;
                    cmd.Parameters.Add("@receiver_id", NpgsqlDbType.Integer).Value = record.ReceiverId;
                    cmd.Parameters.Add("@price", NpgsqlDbType.Numeric).Value = record.Price;
                    cmd.Parameters.Add("@currency", NpgsqlDbType.Varchar).Value = record.Currency;
                    cmd.Parameters.Add("@create_date", NpgsqlDbType.TimestampTz).Value = record.CreateDate;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        /// <summary>
        /// Delete a new record in auctions
        /// </summary>
        /// <param name="auctionId">Primary Key</param>
        /// <returns></returns>
        public async Task DeleteAuction(int auctionId)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "delete from tesora_nft.auctions where auction_id = @auction_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@auction_id", NpgsqlDbType.Integer).Value = auctionId;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        /// <summary>
        /// Get the bids in the auction for an item, sorted highest to lowest
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public async Task<List<Auction>> GetAuctionBids(int itemId)
        {
            var lstAuctions = new List<Auction>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select auction_id,item_id,sender_id,receiver_id,price,currency,create_date" +
                              " from tesora_nft.auctions" +
                              " where item_id = @item_id" +
                              " order by price desc";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = itemId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lstAuctions.Add(new Auction
                            {
                                AuctionId = reader.GetInt32(0),
                                ItemId = reader.IsDBNull(1) ? null : (int)reader.GetInt32(1),
                                SenderId = reader.IsDBNull(2) ? null : (int)reader.GetInt32(2),
                                ReceiverId = reader.IsDBNull(3) ? null : (int)reader.GetInt32(3),
                                Price = reader.IsDBNull(4) ? null : (decimal?)reader.GetDecimal(4),
                                Currency = reader.IsDBNull(5) ? null : (string?)reader.GetString(5),
                                CreateDate = reader.GetDateTime(6),
                            });
                        }
                    }
                }
            }

            return lstAuctions;
        }

    }
}

