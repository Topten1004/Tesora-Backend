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
        public async Task<bool> CategoryExists(int categoryId)
        {
            bool result = false;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select exists(select 1 from tesora_nft.categories where category_id = @category_id)";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@category_id", NpgsqlDbType.Integer).Value = categoryId;

                    var col = await cmd.ExecuteScalarAsync();

                    if (col != null)
                        result = (bool)col;
                }
            }

            return result;
        }


        /// <summary>
        /// Create a new record in Category
        /// </summary>
        /// <param name="record">Category</param>
        /// <returns></returns>
        public async Task CreateCategory(Category record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "insert into tesora_nft.categories" +
                              " (title,category_image,status,create_date,category_image_type,contract_id)" +
                              " values" +
                              " (@title,@category_image,@status,@create_date,@category_image_type,@contract_id)";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@title", NpgsqlDbType.Varchar).Value = record.Title;
                    cmd.Parameters.Add("@category_image", NpgsqlDbType.Bytea).Value = record.CategoryImage;
                    cmd.Parameters.Add("@status", NpgsqlDbType.Unknown).Value = record.Status.ToString();
                    cmd.Parameters.Add("@create_date", NpgsqlDbType.TimestampTz).Value = DateTime.UtcNow;
                    cmd.Parameters.Add("@category_image_type", NpgsqlDbType.Varchar).Value = record.CategoryImageType;
                    cmd.Parameters.Add("@contract_id", NpgsqlDbType.Integer).Value = record.ContractId;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        /// <summary>
        /// Retrieve all Category records
        /// </summary>
        /// <returns>List of Category records</returns>
       public async Task<List<Category>> RetrieveCategories()
        {
            var lstCategory = new List<Category>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select category_id,title,status,create_date,contract_id" +
                              " from tesora_nft.categories" +
                              " order by title asc";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            //record.CategoryImage = ReadBytes(reader, 2);

                            lstCategory.Add(new Category
                            {
                                CategoryId = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Status = (Category.CategoryStatuses)Enum.Parse(typeof(Category.CategoryStatuses), reader.GetString(2)),
                                CreateDate = reader.GetDateTime(3),
                                ContractId = reader.GetInt32(4)
                            });
                        }
                    }
                }
            }

            return lstCategory;
        }


        /// <summary>
        /// Retrieve a Category record
        /// </summary>
        /// <param name="categoryId">Primary Key</param>
        /// <returns></returns>
       public async Task<Category?> RetrieveCategory(int categoryId)
        {
            Category? category = null;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select category_id,title,status,create_date,contract_id" +
                              " from tesora_nft.categories" +
                              " where category_id = @category_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@category_id", NpgsqlDbType.Integer).Value = categoryId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            category = new Category
                            {
                                CategoryId = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Status = (Category.CategoryStatuses)Enum.Parse(typeof(Category.CategoryStatuses), reader.GetString(2)),
                                CreateDate = reader.GetDateTime(3),
                                ContractId = reader.GetInt32(4)
                            };
                        }
                    }
                }
            }

            return category;
        }


        /// <summary>
        /// Retrieve a Category record
        /// </summary>
        /// <param name="categoryId">Primary Key</param>
        /// <returns></returns>
        public async Task<ImageBox?> RetrieveCategoryImage(int categoryId)
        {
            ImageBox? imageBox = null;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select category_image,category_image_type from tesora_nft.categories where category_id = @category_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@category_id", NpgsqlDbType.Integer).Value = categoryId;

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
        /// Update a Category record
        /// </summary>
        /// <param name="record">Category</param>
        /// <returns></returns>
        public async Task UpdateCategory(Category record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "update tesora_nft.categories set" +
                              " title = @title," +
                              " category_image = @category_image," +
                              " status = @status," +
                              " create_date = @create_date," +
                              " category_image_type = @category_image_type," +
                              " contract_id = @contract_id" +
                              " where category_id = @category_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@category_id", NpgsqlDbType.Integer).Value = record.CategoryId;
                    cmd.Parameters.Add("@title", NpgsqlDbType.Varchar).Value = record.Title;
                    cmd.Parameters.Add("@category_image", NpgsqlDbType.Bytea).Value = record.CategoryImage;
                    cmd.Parameters.Add("@status", NpgsqlDbType.Unknown).Value = record.Status.ToString();
                    cmd.Parameters.Add("@create_date", NpgsqlDbType.TimestampTz).Value = record.CreateDate;
                    cmd.Parameters.Add("@category_image_type", NpgsqlDbType.Varchar).Value = record.CategoryImageType;
                    cmd.Parameters.Add("@contract_id", NpgsqlDbType.Integer).Value = record.ContractId;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        /// <summary>
        /// Delete a new record in categories
        /// </summary>
        /// <param name="categoryId">Primary Key</param>
        /// <returns></returns>
        public async Task DeleteCategory(int categoryId)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "delete from tesora_nft.categories where category_id = @category_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@category_id", NpgsqlDbType.Integer).Value = categoryId;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        /// <summary>
        /// Get Category Id by Title
        /// </summary>
        /// <param name="title">Title</param>
        /// <returns>Category Id</returns>
        public async Task<int?> GetCategoryIdByTitle(string title)
        {
            int? id = null;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select id from tesora_nft.categories where title = @title";

                using var cmd = new NpgsqlCommand(sSQL, conn);
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.Parameters.Add("@title", NpgsqlDbType.Integer).Value = title;

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                    id = reader.GetInt32(0);
            }

            return id;
        }

    }
}

