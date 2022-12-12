// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using Npgsql;


namespace NFTDatabase.DataAccess
{
    internal partial class PostgreSql : IPostgreSql
    {
        private readonly string connString;

        public PostgreSql(string conn)
        {
            connString = conn;
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


        private byte[]? ReadBytes(NpgsqlDataReader reader, int ordinal)
        {
            byte[]? image = null;

            if (!reader.IsDBNull(ordinal))
            {
                //get the length of data
                long size = reader.GetBytes(ordinal, 0, null, 0, 0);
                image = new byte[size];

                int bufferSize = 1024;
                long bytesRead = 0;
                int curPos = 0;

                while (bytesRead < size)
                {
                    if (size - bytesRead < (long)bufferSize)
                        bufferSize = (int)(size - bytesRead);

                    bytesRead += reader.GetBytes(ordinal, curPos, image, curPos, bufferSize);
                    curPos += bufferSize;
                }
            }

            return image;
        }
    }
}
