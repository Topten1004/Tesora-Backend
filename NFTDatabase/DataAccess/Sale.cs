// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using Npgsql;
using NpgsqlTypes;

using NFTDatabaseEntities;
using System.Transactions;

namespace NFTDatabase.DataAccess
{
    internal partial class PostgreSql : IPostgreSql
    {
        /// <summary>
        /// Create a new record in Cart
        /// </summary>
        /// <param name="record">Cart</param>
        /// <returns></returns>
        public async Task CreateSalesOrder(Sale record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                using (var transaction = conn.BeginTransaction())
                {
                    int id;
                    string sSQL = "insert into tesora_nft.sales" +
                                  " (user_id,sale_type,total_amount,currency,payment_status,reason,address,create_date)" +
                                  " values" +
                                  " (@user_id,@sale_type,@total_amount,@currency,@payment_status,@reason,@address,@create_date)" +
                                  " returning sale_id";

                    using (var cmd = new NpgsqlCommand(sSQL, conn, transaction))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;

                        cmd.Parameters.Add("@user_id", NpgsqlDbType.Integer).Value = record.UserId;
                        cmd.Parameters.Add("@sale_type", NpgsqlDbType.Varchar).Value = record.SaleType.ToString();
                        cmd.Parameters.Add("@total_amount", NpgsqlDbType.Numeric).Value = record.TotalAmount;
                        cmd.Parameters.Add("@currency", NpgsqlDbType.Varchar).Value = record.Currency;
                        cmd.Parameters.Add("@payment_status", NpgsqlDbType.Varchar).Value = record.PaymentStatus.ToString();
                        cmd.Parameters.Add("@reason", NpgsqlDbType.Varchar).Value = record.Reason;
                        cmd.Parameters.Add("@address", NpgsqlDbType.Varchar).Value = record.Address;
                        cmd.Parameters.Add("@create_date", NpgsqlDbType.TimestampTz).Value = record.CreateDate;

                        var result = await cmd.ExecuteScalarAsync();

                        if (result == null)
                            throw new Exception("Error getting id for added record");

                        id = (int)result;
                    }

                    foreach(var item in record.SaleItems)
                    {
                        sSQL = "insert into tesora_nft.sale_items" +
                               " (ring,section,block,lot,price,currency,sale_id,expiration)" +
                               " values" +
                               " (@ring,@section,@block,@lot,@price,@currency,@sale_id,@expiration)";

                        using (var cmd = new NpgsqlCommand(sSQL, conn, transaction))
                        {
                            cmd.CommandType = System.Data.CommandType.Text;

                            cmd.Parameters.Add("@ring", NpgsqlDbType.Varchar).Value = item.Ring;
                            cmd.Parameters.Add("@section", NpgsqlDbType.Varchar).Value = item.Section;
                            cmd.Parameters.Add("@block", NpgsqlDbType.Varchar).Value = item.Block;
                            cmd.Parameters.Add("@lot", NpgsqlDbType.Varchar).Value = item.Lot;
                            cmd.Parameters.Add("@price", NpgsqlDbType.Numeric).Value = item.Price;
                            cmd.Parameters.Add("@currency", NpgsqlDbType.Varchar).Value = item.Currency;
                            cmd.Parameters.Add("@sale_id", NpgsqlDbType.Integer).Value = id;
                            cmd.Parameters.Add("@expiration", NpgsqlDbType.TimestampTz).Value = item.Expiration;

                            await cmd.ExecuteNonQueryAsync();
                        }
                    }

                    await transaction.CommitAsync();
                }
            }
        }

        public async Task UpdateSalesOrderStatus(string address, Sale.PaymentStatuses paymentStatus)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "update tesora_nft.sales set payment_status = @payment_status where address = @address";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@payment_status", NpgsqlDbType.Varchar).Value = paymentStatus.ToString();
                    cmd.Parameters.Add("@address", NpgsqlDbType.Varchar).Value = address;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


    }
}

