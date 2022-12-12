// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using Npgsql;
using NpgsqlTypes;

using NFTDatabaseEntities;
using System;


namespace NFTDatabase.DataAccess
{
    internal partial class PostgreSql : IPostgreSql
    {
        /// <summary>
        /// Create a new record in Offer
        /// </summary>
        /// <param name="record">Offer</param>
        /// <returns></returns>
       public async Task CreateOffer(Offer record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "insert into tesora_nft.offers" +
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
        /// Retrieve all Offer records
        /// </summary>
        /// <returns>List of Offer records</returns>
       public async Task<List<Offer>> RetrieveOffers()
        {
            var lstOffer = new List<Offer>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select offer_id,item_id,sender_id,receiver_id,price,currency,create_date" +
                              " from tesora_nft.offers";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lstOffer.Add(new Offer
                            {
                                OfferId = reader.GetInt32(0),
                                ItemId = reader.IsDBNull(1) ? null : (int)reader.GetInt32(1),
                                SenderId = reader.IsDBNull(2) ? null : (int)reader.GetInt32(2),
                                ReceiverId = reader.IsDBNull(3) ? null : (int)reader.GetInt32(3),
                                Price = reader.IsDBNull(4) ? null : (decimal?)reader.GetDecimal(4),
                                Currency = reader.IsDBNull(5) ? null : (string?)reader.GetString(5),
                                CreateDate = reader.GetDateTime(6)
                            });
                        }
                    }
                }
            }

            return lstOffer;
        }


        /// <summary>
        /// Retrieve a Offer record
        /// </summary>
        /// <param name="offerId">Primary Key</param>
        /// <returns></returns>
       public async Task<Offer?> RetrieveOffer(int offerId)
        {
            Offer? offer = default;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select offer_id,item_id,sender_id,receiver_id,price,currency,create_date" +
                              " from tesora_nft.offers" +
                              " where offer_id = @offer_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@offer_id", NpgsqlDbType.Integer).Value = offerId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            offer = new Offer
                            {
                                OfferId = reader.GetInt32(0),
                                ItemId = reader.IsDBNull(1) ? null : (int)reader.GetInt32(1),
                                SenderId = reader.IsDBNull(2) ? null : (int)reader.GetInt32(2),
                                ReceiverId = reader.IsDBNull(3) ? null : (int)reader.GetInt32(3),
                                Price = reader.IsDBNull(4) ? null : (Decimal?)reader.GetDecimal(4),
                                Currency = reader.IsDBNull(5) ? null : (string?)reader.GetString(5),
                                CreateDate = reader.GetDateTime(6),
                            };
                        }
                    }
                }
            }

            return offer;
        }

        /// <summary>
        /// Retrieve all my offer records
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List of my offer records</returns>
        public async Task<List<OfferUserCollectionItemCategory>> RetrieveMyOffers(int userId)
        {
            var lstOffer = new List<OfferUserCollectionItemCategory>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select o.offer_id,o.price,o.currency,o.create_date,i.item_id,i.name,i.price,i.currency,u.user_id,u.username,u.first_name,u.last_name,c.name,i.media_ipfs,i.description" +
                              " from tesora_nft.offers o" +
                              " join tesora_nft.items i on i.item_id = o.item_id" +
                              " join tesora_nft.users u on u.user_id = o.sender_id" +
                              " join tesora_nft.collections c on c.collection_id = i.collection_id" +
                              " where o.receiver_id = @receiver_id and i.enable_auction = true and i.start_date <= o.create_date and i.end_date >= o.create_date" +
                              " order by c.collection_id asc, i.item_id asc, o.offer_id desc";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@receiver_id", NpgsqlDbType.Integer).Value = userId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lstOffer.Add(new OfferUserCollectionItemCategory
                            {
                                OfferId = reader.GetInt32(0),
                                Price = reader.IsDBNull(1) ? null : (Decimal?)reader.GetDecimal(1),
                                Currency = reader.IsDBNull(2) ? null : (string?)reader.GetString(2),
                                CreateDate = reader.GetDateTime(3),
                                Item = new Item
                                {
                                    ItemId = reader.GetInt32(4),
                                    Name = reader.GetString(5),
                                    Price = reader.IsDBNull(6) ? null : reader.GetDecimal(6),
                                    Currency = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    MediaIpfs = reader.IsDBNull(11) ? null : reader.GetString(13),
                                    Description = reader.IsDBNull(12) ? null : reader.GetString(14),
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

            return lstOffer;
        }

        /// <summary>
        /// Get my offer
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="offerId">Offer Id</param>
        /// <returns>List of my offer records</returns>
        public async Task<OfferUserCollectionItemCategory?> RetrieveMyOffer(int userId, int offerId)
        {
            OfferUserCollectionItemCategory? offer = null;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select o.offer_id,o.price,o.currency,o.create_date" +
                              ",i.item_id,i.name,i.price,i.currency,i.media_ipfs,i.description,i.token_id" +
                              ",u.user_id,u.username,u.first_name,u.last_name,u.master_user_id" +
                              ",c.collection_id,c.name" +
                              ",g.category_id,g.title" +
                              " from tesora_nft.offers o" +
                              " join tesora_nft.items i on i.item_id = o.item_id" +
                              " join tesora_nft.users u on u.user_id = o.sender_id" +
                              " join tesora_nft.collections c on c.collection_id = i.collection_id" +
                              " join tesora_nft.categories g on g.category_id = i.category_id" +
                              " where o.receiver_id = @receiver_id and o.offer_id = @offer_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@receiver_id", NpgsqlDbType.Integer).Value = userId;
                    cmd.Parameters.Add("@offer_id", NpgsqlDbType.Integer).Value = offerId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            offer = new OfferUserCollectionItemCategory
                            {
                                OfferId = reader.GetInt32(0),
                                Price = reader.IsDBNull(1) ? null : (Decimal?)reader.GetDecimal(1),
                                Currency = reader.IsDBNull(2) ? null : (string?)reader.GetString(2),
                                CreateDate = reader.GetDateTime(3),
                                Item = new Item
                                {
                                    ItemId = reader.GetInt32(4),
                                    Name = reader.GetString(5),
                                    Price = reader.IsDBNull(6) ? null : reader.GetDecimal(6),
                                    Currency = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    MediaIpfs = reader.IsDBNull(8) ? null : reader.GetString(8),
                                    Description = reader.IsDBNull(7) ? null : reader.GetString(9),
                                    TokenId = reader.GetInt32(10)
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

            return offer;
        }

        /// <summary>
        /// Accept my offer
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="offerId">Offer Id</param>
        /// <param name="txHash">Transaction Hash</param>
        /// <returns></returns>
        public async Task AcceptMyOffer(int userId, int offerId, string txHash)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL;
                int receiverId;
                int senderId;
                int itemId;
                int collectionId;
                decimal price;
                string currency;

                // Get data needed
                sSQL = "select o.receiver_id,o.sender_id,o.item_id,o.price,o.currency,i.collection_id from tesora_nft.offers o join tesora_nft.items i on i.item_id = o.item_id where offer_id = @offer_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@offer_id", NpgsqlDbType.Integer).Value = offerId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            receiverId = reader.GetInt32(0);
                            senderId = reader.GetInt32(1);
                            itemId = reader.GetInt32(2);
                            price = reader.GetDecimal(3);
                            currency = reader.GetString(4);
                            collectionId = reader.GetInt32(5);
                        }
                        else
                            throw new Exception("Invalid offer id");
                    }
                }

                // Validate that request was not tampered with
                if (receiverId != userId)
                    throw new Exception("The offer is not for this user");

                // Terminate offer and move item to new owner
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

                // Make history entry
                sSQL = "insert into tesora_nft.histories" +
                        " (item_id,collection_id,from_id,to_id,transaction_hash,price,currency,history_type,is_valid,create_date)" +
                        " values" +
                        " (@item_id,@collection_id,@from_id,@to_id,@transaction_hash,@price,@currency,@history_type,@is_valid,@create_date)";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = itemId;
                    cmd.Parameters.Add("@collection_id", NpgsqlDbType.Integer).Value = collectionId;
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

                // Delete past offers
                sSQL = "delete from tesora_nft.offers where item_id = @item_id and receiver_id = @receiver_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = itemId;
                    cmd.Parameters.Add("@receiver_id", NpgsqlDbType.Integer).Value = receiverId;

                    await cmd.ExecuteNonQueryAsync();
                }
            }

        }


        /// <summary>
        /// Update a Offer record
        /// </summary>
        /// <param name="record">Offer</param>
        /// <returns></returns>
        public async Task UpdateOffer(Offer record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "update tesora_nft.offers set" +
                              " item_id = @item_id," +
                              " sender_id = @sender_id," +
                              " receiver_id = @receiver_id," +
                              " price = @price," +
                              " currency = @currency," +
                              " create_date = @create_date" +
                              " where offer_id = @offer_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@offer_id", NpgsqlDbType.Integer).Value = record.OfferId;
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
        /// Delete a new record in offers
        /// </summary>
        /// <param name="offerId">Primary Key</param>
        /// <returns></returns>
        public async Task DeleteOffer(int offerId)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "delete from tesora_nft.offers where offer_id = @offer_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@offer_id", NpgsqlDbType.Integer).Value = offerId;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        /// <summary>
        /// Delete a new record in offers
        /// </summary>
        /// <param name="itemId">Item Id</param>
        /// <param name="currentOwnerId">Recevier Id</param>
        /// <returns></returns>
        public async Task DeletePastOffers(int itemId, int currentOwnerId)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "delete from tesora_nft.offers where item_id = @item_id and receiver_id = @receiver_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = itemId;
                    cmd.Parameters.Add("@receiver_id", NpgsqlDbType.Integer).Value = currentOwnerId;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}

