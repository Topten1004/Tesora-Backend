using Nethereum.Web3;

namespace NFTApplication.Services
{
    /// <summary>
    /// NFT IPFS Helper
    /// </summary>
    public class NFTIpfsService
    {
        private readonly string? _userName;
        private readonly string? _password;
        private readonly string _ipfsUrl;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ipfsUrl"></param>
        public NFTIpfsService(string ipfsUrl)
        {
            _ipfsUrl = ipfsUrl;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ipfsUrl"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public NFTIpfsService(string ipfsUrl, string userName, string password) :this(ipfsUrl)
        {
            _userName = userName;
            _password = password;
        }

        /// <summary>
        /// Add the meta data to IPFS
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="metadata"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Task<IPFSFileInfo> AddNftsMetadataToIpfsAsync<T>(T metadata, string fileName)
        {
            var ipfsClient = GetSimpleHttpIpfs();

            return ipfsClient.AddObjectAsJson<T>(metadata, fileName);
        }

        /// <summary>
        /// Add a file from disk
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<IPFSFileInfo> AddFileToIpfsAsync(string path)
        {
            var ipfsClient = GetSimpleHttpIpfs();
            
            return await ipfsClient.AddFileAsync(path);
        }

        /// <summary>
        /// Add an array of bytes and assign a file name
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<IPFSFileInfo> Add(byte[] data, string fileName)
        {
            var ipfsClient = GetSimpleHttpIpfs();

            return await ipfsClient.AddAsync(data, fileName);
        }


        private SimpleHttpIPFS GetSimpleHttpIpfs()
        {
            if (_userName == null) 
                return new SimpleHttpIPFS(_ipfsUrl);
            
            return new SimpleHttpIPFS(_ipfsUrl, _userName, _password);
        }
    }
}
