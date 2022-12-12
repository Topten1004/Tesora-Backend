using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using Nethereum.Web3;


namespace NFTApplication.Services
{
    /// <summary>
    ///  Simple http ipfs to add a file, for a complete implementation please use https://github.com/richardschneider/net-ipfs-http-client
    /// </summary>
    public class SimpleHttpIPFS
    {
        private readonly string Url;
        private readonly AuthenticationHeaderValue? _authHeaderValue;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="url"></param>
        public SimpleHttpIPFS(string url)
        {
            if (!url.EndsWith("api/v0"))
            {
                url = url.TrimEnd('/') + "/api/v0";
            }

            Url = url;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="url"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public SimpleHttpIPFS(string url, string userName, string password) : this(url)
        {
            _authHeaderValue = GetBasicAuthenticationHeaderValue(userName, password);
        }

        /// <summary>
        /// Build the Auth Header
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public AuthenticationHeaderValue GetBasicAuthenticationHeaderValue(string userName, string password)
        {
            var byteArray = Encoding.UTF8.GetBytes(userName + ":" + password);
            
            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        /// <summary>
        /// Add a byte array to IPFS, give it a file name, and pin it
        /// </summary>
        /// <param name="fileBytes"></param>
        /// <param name="fileName"></param>
        /// <param name="pin"></param>
        /// <returns></returns>
        public async Task<IPFSFileInfo> AddAsync(byte[] fileBytes, string fileName, bool pin = true)
        {
            var content = new MultipartFormDataContent();
            var streamContent = new ByteArrayContent(fileBytes);

            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            content.Add(streamContent, "file", fileName);

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = _authHeaderValue;
                var query = pin ? "?pin=true&cid-version=1" : "?cid-version=1";
                var fullUrl = Url + "/add" + query;

                var response = await httpClient.PostAsync(fullUrl, content);
                response.EnsureSuccessStatusCode();

                var responseStream = await response.Content.ReadAsStringAsync();

                if (responseStream != null)
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    return JsonSerializer.Deserialize<IPFSFileInfo>(responseStream, options);
                }
                else
                    return new IPFSFileInfo
                    {
                        Name = "unknonw",
                        Size = "0",
                        Hash = ""
                    };
            }
        }

        /// <summary>
        /// Add an object to IPFS, give it a file name and pin it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToSerialise"></param>
        /// <param name="fileName"></param>
        /// <param name="pin"></param>
        /// <returns></returns>
        public async Task<IPFSFileInfo> AddObjectAsJson<T>(T objectToSerialise, string fileName, bool pin = true)
        {
            var options = new JsonSerializerOptions 
            { 
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var byteArr = JsonSerializer.SerializeToUtf8Bytes<T>(objectToSerialise, options);

            var node = await AddAsync(byteArr, fileName, true);

            return node;
        }

        /// <summary>
        /// Add a physical file from disk and pin it
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pin"></param>
        /// <returns></returns>
        public Task<IPFSFileInfo> AddFileAsync(string path, bool pin = true)
        {
            var fileBytes = File.ReadAllBytes(path);
            var fileName = Path.GetFileName(path);

            return AddAsync(fileBytes, fileName, pin);
        }
    }
}
