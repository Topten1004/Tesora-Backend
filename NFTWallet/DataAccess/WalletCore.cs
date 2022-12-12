// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using Npgsql;
using NpgsqlTypes;

using NFTWallet.Models;
using NFTWallet.Engine;


namespace NFTWallet.DataAccess
{
    internal partial class PostgreSql : IPostgreSql
    {
        /// <summary>
        /// Wallet Exists
        /// </summary>
        /// <param name="masterUserId"></param>
        /// <returns></returns>
        public async Task<bool> WalletExists(string masterUserId)
        {
            var filterHash = Security.GenerateHash(masterUserId);
            bool exists = false;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select count(*) from tesora_vault.wallets where filter = @filter";
                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@filter", NpgsqlDbType.Varchar).Value = filterHash;

                    var cnt = await cmd.ExecuteScalarAsync();

                    exists = ((Int64)(cnt ?? 0)) > 0;
                }
            }

            return exists;
        }

        /// <summary>
        /// Create a new wallet
        /// </summary>
        /// <param name="record">Category</param>
        /// <returns></returns>
        public async Task CreateWalletCore(WalletCore record)
          {
            var filterHash = Security.GenerateHash(record.MasterUserId);
            var topicEncrypt = _rsa.EncryptString(record.Topic);
            var valueEncrypt = _rsa.EncryptString(record.Value);

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                var sSQL = "insert into tesora_vault.wallets (filter,topic,value) values (@filter,@topic,@value)";
                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@filter", NpgsqlDbType.Varchar).Value = filterHash;
                    cmd.Parameters.Add("@topic", NpgsqlDbType.Varchar).Value = topicEncrypt;
                    cmd.Parameters.Add("@value", NpgsqlDbType.Varchar).Value = valueEncrypt;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        /// <summary>
        /// Retrieve the wallet
        /// </summary>
        /// <param name="masterUserId">Master User Id</param>
        /// <returns></returns>
       public async Task<WalletCore?> RetrieveWalletCore(string masterUserId)
        {
            WalletCore? walletCore = default;

            var filterHash = Security.GenerateHash(masterUserId);

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select filter,topic,value from tesora_vault.wallets where filter = @filter";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@filter", NpgsqlDbType.Varchar).Value = filterHash;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            walletCore = new WalletCore
                            {
                                MasterUserId = masterUserId,
                                Topic = _rsa.DecryptString(reader.GetString(1)),
                                Value = _rsa.DecryptString(reader.GetString(2))
                            };
                        }
                    }
                }
            }

            return walletCore;
        }


        /// <summary>
        /// Delete a new record in wallets
        /// </summary>
        /// <param name="masterUserId">Master User Id</param>
        /// <returns></returns>
        public async Task DeleteWalletCore(string masterUserId)
        {
            var filterHash = Security.GenerateHash(masterUserId);

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "delete from tesora_vault.wallets where filter = @filter";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@filter", NpgsqlDbType.Varchar).Value = filterHash;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}

