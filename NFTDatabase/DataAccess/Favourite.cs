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
        /// Create a new record in Favourite
        /// </summary>
        /// <param name="record">Favourite</param>
        /// <returns></returns>
       public async Task<bool> CreateFavourite(Favourite record)
        {
            bool exists = true;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "insert into tesora_nft.favourites (item_id,user_id,create_date)" +
                              " select @item_id, @user_id, now() where not exists" +
                              "  (select favourite_id from tesora_nft.favourites where item_id = @item_id and user_id = @user_id)";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = record.ItemId;
                    cmd.Parameters.Add("@user_id", NpgsqlDbType.Integer).Value = record.UserId;
                    cmd.Parameters.Add("@create_date", NpgsqlDbType.TimestampTz).Value = record.CreateDate;

                    var result = await cmd.ExecuteNonQueryAsync();

                    if ((Int64)result == 1)
                        exists = false;
                }

                // increase like_count in items
                sSQL = "update tesora_nft.items" +
                        " set like_count = like_count + 1" +
                        " where" +
                        " item_id = @item_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = record.ItemId;

                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return exists;
        }


        /// <summary>
        /// Retrieve all Favourite records
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List of Favourite records</returns>
       public async Task<List<Favourite>> RetrieveFavourites(int userId)
        {
            var lstFavourite = new List<Favourite>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select favourite_id,item_id,user_id,create_date" +
                              " from tesora_nft.favourites" + 
                              " where user_id = @user_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@user_id", NpgsqlDbType.Integer).Value = userId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lstFavourite.Add(new Favourite
                            {
                                FavouriteId = reader.GetInt32(0),
                                ItemId = reader.GetInt32(1),
                                UserId = reader.GetInt32(2),
                                CreateDate = reader.GetDateTime(3),
                            });
                        }
                    }
                }
            }

            return lstFavourite;
        }


        /// <summary>
        /// Retrieve a Favourite record
        /// </summary>
        /// <param name="favouriteId">Primary Key</param>
        /// <returns></returns>
       public async Task<Favourite?> RetrieveFavourite(int favouriteId)
        {
            Favourite? favourite = default;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select favourite_id,item_id,user_id,create_date" +
                              " from tesora_nft.favourites" +
                              " where favourite_id = @favourite_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@favourite_id", NpgsqlDbType.Integer).Value = favouriteId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            favourite = new Favourite
                            {
                                FavouriteId = reader.GetInt32(0),
                                ItemId = reader.GetInt32(1),
                                UserId = reader.GetInt32(2),
                                CreateDate = reader.GetDateTime(3),
                            };

                        }
                    }
                }
            }

            return favourite;
        }


        /// <summary>
        /// Update a Favourite record
        /// </summary>
        /// <param name="record">Favourite</param>
        /// <returns></returns>
       public async Task UpdateFavourite(Favourite record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "update tesora_nft.favourites set" +
                              " item_id = @item_id," +
                              " user_id = @user_id," +
                              " create_date = @create_date" +
                              " where favourite_id = @favourite_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@favourite_id", NpgsqlDbType.Integer).Value = record.FavouriteId;
                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = record.ItemId;
                    cmd.Parameters.Add("@user_id", NpgsqlDbType.Integer).Value = record.UserId;
                    cmd.Parameters.Add("@create_date", NpgsqlDbType.TimestampTz).Value = record.CreateDate;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        /// <summary>
        /// Delete a new record in favourites
        /// </summary>
        /// <param name="favouriteId">Primary Key</param>
        /// <returns></returns>
        public async Task DeleteFavourite(int favouriteId)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "delete from tesora_nft.favourites where favourite_id = @favourite_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@favourite_id", NpgsqlDbType.Integer).Value = favouriteId;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        /// <summary>
        /// Remove a new record in favourites
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="itemId">Item Id</param>
        /// <returns></returns>
        public async Task RemoveFavourite(int userId, int itemId)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "delete from tesora_nft.favourites where user_id = @user_id and item_id = @item_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@user_id", NpgsqlDbType.Integer).Value = userId;
                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = itemId;

                    await cmd.ExecuteNonQueryAsync();
                }

                // decrease like_count in items
                sSQL = "update tesora_nft.items" +
                        " set like_count = like_count - 1" +
                        " where" +
                        " item_id = @item_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = itemId;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        /// <summary>
        /// Retrieve all my favorite records
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List of my favorite records</returns>
        public async Task<List<FavoriteCollectionItemCategory>> RetrieveMyFavorites(int userId)
        {
            var lstFavorites = new List<FavoriteCollectionItemCategory>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select f.favourite_id,f.item_id,f.create_date,i.item_id,i.name,i.price,c.name,i.media_ipfs,i.enable_auction,i.accept_offer" +
                              " from tesora_nft.favourites f" +
                              " join tesora_nft.items i on i.item_id = f.item_id" +
                              " join tesora_nft.collections c on c.collection_id = i.collection_id" +
                              " where f.user_id = @user_id" +
                              " order by f.favourite_id desc";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@user_id", NpgsqlDbType.Integer).Value = userId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lstFavorites.Add(new FavoriteCollectionItemCategory
                            {
                                Favorite = new Favourite
                                {
                                    FavouriteId = reader.GetInt32(0),
                                    ItemId = reader.GetInt32(1),
                                    UserId = userId,
                                    CreateDate = reader.GetDateTime(2)
                                },
                                Item = new Item
                                {
                                    ItemId = reader.GetInt32(3),
                                    Name = reader.GetString(4),
                                    Price = reader.IsDBNull(5) ? null : reader.GetDecimal(5),
                                    MediaIpfs = reader.GetString(7),
                                    EnableAuction = reader.GetBoolean(8),
                                    AcceptOffer = reader.GetBoolean(9),
                                },
                                Collection = new Collection
                                {
                                    Name = reader.GetString(6)
                                }
                            });
                        }
                    }
                }
            }

            return lstFavorites;
        }

    }
}

