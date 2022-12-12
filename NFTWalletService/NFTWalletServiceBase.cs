// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;


namespace NFTWalletService
{
    public class BooleanResult
    {
        /// <summary>Exists</summary>
        [JsonPropertyName("exists")]
        public bool Exists { get; set; }
    }

    /// <summary>
    /// NFTWalletService Service
    /// </summary>
    public partial class NFTWalletServiceBase
    {
        private readonly HttpClient _httpClient;
        private string AccessToken { get; set; } = "";
        private readonly string _tokenAuthServer;
        private readonly string _tokenClientId;
        private readonly string _tokenClientSecret;
        private readonly string _tokenGrantTyoe;
        private readonly string _tokenScope;

        public class SSOTokenResponse
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string token_type { get; set; }
            public string scope { get; set; }
        }


        public NFTWalletServiceBase(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;

            var env = configuration["Environment:Prefix"];

            _tokenAuthServer = configuration[$"Authorization:{env}TokenServer"];
            _tokenClientId = configuration[$"Authorization:{env}TokenClientId"];
            _tokenClientSecret = configuration[$"Authorization:{env}TokenClientSecret"];
            _tokenGrantTyoe = configuration[$"Authorization:{env}TokenGrantType"];
            _tokenScope = configuration[$"Authorization:{env}TokenScope"];
        }


        public async Task<T> MakeServiceGetCall<T>(string apiEndPoint)
        {
            T result = default;
            bool haveRecords = false;
            int tryCount = 0;

            while (haveRecords == false)
            {
                // Do we need to get an access token?
                if (string.IsNullOrEmpty(AccessToken))
                {
                    await SetAccessToken();
                }

                // Call API to get wallets
                var apiRequest = new HttpRequestMessage(HttpMethod.Get, apiEndPoint);

                apiRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

                var response = await _httpClient.SendAsync(apiRequest);

                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, Converters = { new JsonStringEnumConverter() } };

                    result = JsonSerializer.Deserialize<T>(responseStream, options);

                    haveRecords = true;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // Token has expired, need to get another one
                    AccessToken = "";

                    if (tryCount > 0)
                        throw new Exception("Unable to get authorization token");
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    tryCount = 3;
                }
                else
                {
                    throw new Exception(await response.Content.ReadAsStringAsync());
                }

                if (++tryCount > 3)
                    haveRecords = true;
            }

            return result;
        }

        public async Task MakeServicePostCall(string apiEndPoint, Object request)
        {
            bool haveRecords = false;
            int tryCount = 0;

            while (haveRecords == false)
            {
                // Do we need to get an access token?
                if (string.IsNullOrEmpty(AccessToken))
                {
                    await SetAccessToken();
                }

                // Call API to get receive
                var apiRequest = new HttpRequestMessage(HttpMethod.Post, apiEndPoint);

                var content = JsonSerializer.Serialize(request);
                apiRequest.Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json");

                apiRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

                var response = await _httpClient.SendAsync(apiRequest);

                if (response.IsSuccessStatusCode)
                {
                    haveRecords = true;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // Token has expired, need to get another one
                    AccessToken = "";
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    // Dont bang our head
                    tryCount = 3;
                }
                else
                {
                    var responseStream = await response.Content.ReadAsStringAsync();
                    throw new Exception(responseStream);
                }

                if (++tryCount > 3)
                    haveRecords = true;
            }
        }

        public async Task<T> MakeServiePostCall<T>(string apiEndPoint, Object request)
        {
            T result = default;
            bool hasRecords = false;
            int tryCount = 0;

            while (hasRecords == false)
            {
                // Do we need to get an access token?
                if (string.IsNullOrEmpty(AccessToken))
                {
                    await SetAccessToken();
                }

                // Call API to get receive
                var apiRequest = new HttpRequestMessage(HttpMethod.Post, apiEndPoint);

                var content = JsonSerializer.Serialize(request);
                apiRequest.Content = new StringContent(content, Encoding.UTF8, "application/json");

                apiRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

                var response = await _httpClient.SendAsync(apiRequest);

                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, Converters = { new JsonStringEnumConverter() } };

                    result = JsonSerializer.Deserialize<T>(responseStream, options);

                    hasRecords = true;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // Token has expired, need to get another one
                    AccessToken = "";
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    // Dont bang our head
                    tryCount = 3;
                }
                else
                {
                    var responseStream = await response.Content.ReadAsStringAsync();
                    throw new Exception(responseStream);
                }

                if (++tryCount > 3)
                    hasRecords = true;
            }

            return result;
        }

        public async Task<int> MakeServiePostCall(string apiEndPoint, Object request)
        {
            int result = -1;
            bool hasRecords = false;
            int tryCount = 0;

            while (hasRecords == false)
            {
                // Do we need to get an access token?
                if (string.IsNullOrEmpty(AccessToken))
                {
                    await SetAccessToken();
                }

                // Call API to get receive
                var apiRequest = new HttpRequestMessage(HttpMethod.Post, apiEndPoint);

                var content = JsonSerializer.Serialize(request);
                apiRequest.Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json");

                apiRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

                var response = await _httpClient.SendAsync(apiRequest);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    result = Convert.ToInt32(responseString);

                    hasRecords = true;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // Token has expired, need to get another one
                    AccessToken = "";
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    // Dont bang our head
                    tryCount = 3;
                }
                else
                {
                    var responseStream = await response.Content.ReadAsStringAsync();
                    throw new Exception(responseStream);
                }

                if (++tryCount > 3)
                    hasRecords = true;
            }

            return result;
        }

        public async Task MakeServiceQueryPostCall(string apiEndPoint, string queryString)
        {
            bool haveRecords = false;
            int tryCount = 0;

            while (haveRecords == false)
            {
                // Do we need to get an access token?
                if (string.IsNullOrEmpty(AccessToken))
                {
                    await SetAccessToken();
                }

                // Call API to get receive
                var apiRequest = new HttpRequestMessage(HttpMethod.Post, $"{apiEndPoint}?{queryString}");

                apiRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

                var response = await _httpClient.SendAsync(apiRequest);

                if (response.IsSuccessStatusCode)
                {
                    haveRecords = true;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // Token has expired, need to get another one
                    AccessToken = "";
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    // Dont bang our head
                    tryCount = 3;
                }
                else
                {
                    var responseStream = await response.Content.ReadAsStringAsync();
                    throw new Exception(responseStream);
                }

                if (++tryCount > 3)
                    haveRecords = true;
            }
        }

        public async Task MakeServicePutCall(string apiEndPoint, Object request)
        {
            bool haveRecords = false;
            int tryCount = 0;

            while (haveRecords == false)
            {
                // Do we need to get an access token?
                if (string.IsNullOrEmpty(AccessToken))
                {
                    await SetAccessToken();
                }

                // Call API to get receive
                var apiRequest = new HttpRequestMessage(HttpMethod.Put, apiEndPoint);

                var content = JsonSerializer.Serialize(request);
                apiRequest.Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json");

                apiRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

                var response = await _httpClient.SendAsync(apiRequest);

                if (response.IsSuccessStatusCode)
                {
                    haveRecords = true;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // Token has expired, need to get another one
                    AccessToken = "";
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    // Dont bang our head
                    tryCount = 3;
                }
                else
                {
                    var responseStream = await response.Content.ReadAsStringAsync();
                    throw new Exception(responseStream);
                }

                if (++tryCount > 3)
                    haveRecords = true;
            }
        }


        public async Task MakeServiceDeleteCall(string apiEndPoint)
        {
            bool haveRecords = false;
            int tryCount = 0;

            while (haveRecords == false)
            {
                // Do we need to get an access token?
                if (string.IsNullOrEmpty(AccessToken))
                {
                    await SetAccessToken();
                }

                // Call API to get receive
                var apiRequest = new HttpRequestMessage(HttpMethod.Delete, apiEndPoint);

                apiRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

                var response = await _httpClient.SendAsync(apiRequest);

                if (response.IsSuccessStatusCode)
                {
                    haveRecords = true;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // Token has expired, need to get another one
                    AccessToken = "";
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    // Dont bang our head
                    tryCount = 3;
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    tryCount = 3;
                }

                if (++tryCount > 3)
                    haveRecords = true;
            }
        }


        public async Task SetAccessToken()
        {
            HttpContent tokenContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", _tokenClientId),
                new KeyValuePair<string, string>("client_secret", _tokenClientSecret),
                new KeyValuePair<string, string>("grant_type", _tokenGrantTyoe),
                new KeyValuePair<string, string>("scope", _tokenScope)
            });

            var endpoint = new Uri(new Uri(_tokenAuthServer), "/connect/token");

            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, endpoint);
            tokenRequest.Method = HttpMethod.Post;
            tokenRequest.Content = tokenContent;
            tokenRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var tokenResponse = await _httpClient.SendAsync(tokenRequest);

            if (tokenResponse.IsSuccessStatusCode)
            {
                var tokenStream = await tokenResponse.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, Converters = { new JsonStringEnumConverter() } };

                SSOTokenResponse ssoResponse = JsonSerializer.Deserialize<SSOTokenResponse>(tokenStream, options);

                // Save token for future calls
                AccessToken = ssoResponse.access_token;
            }
            else
            {
                var error = await tokenResponse.Content.ReadAsStringAsync();

                throw new Exception(error);
            }

        }
    }
}
