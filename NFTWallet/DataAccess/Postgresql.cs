// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using Npgsql;


namespace NFTWallet.DataAccess
{
    internal partial class PostgreSql : IPostgreSql
    {
        private static string connString;
        private readonly Engine.IRSA _rsa;


        public PostgreSql(string conn)
        {
            connString = conn;
            _rsa = new Engine.RSA();
        }


        [Serializable]
        public class RecordNotFound : Exception
        {
            public RecordNotFound() { }
            public RecordNotFound(string message) : base(message) { }
        }


        [Serializable]
        public class RecordExists : Exception
        {
            public RecordExists() { }
            public RecordExists(string message) : base(message) { }
        }
    }
}
