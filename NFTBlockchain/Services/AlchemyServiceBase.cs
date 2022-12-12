// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NFTBlockchain.Services
{
    /// <summary>
    /// Alchemy Base Service
    /// </summary>
    public partial class AlchemyServiceBase
    {
        private readonly HttpClient _httpClient;


        public AlchemyServiceBase(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<T> MakeServiceGetCall<T>(string apiEndPoint)
        {
            T result = default;

            var apiRequest = new HttpRequestMessage(HttpMethod.Get, apiEndPoint);

            var response = await _httpClient.SendAsync(apiRequest);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, Converters = { new JsonStringEnumConverter() } };

                result = JsonSerializer.Deserialize<T>(responseStream, options);
            }
            else
                throw new Exception(await response.Content.ReadAsStringAsync());


            return result;
        }

        public async Task MakeServicePostCall(string apiEndPoint, Object request)
        {
            var apiRequest = new HttpRequestMessage(HttpMethod.Post, apiEndPoint);

            var content = JsonSerializer.Serialize(request);
            apiRequest.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStringAsync();
                throw new Exception(responseStream);
            }
        }

        public async Task<T> MakeServicePostCall<T>(string apiEndPoint, Object request)
        {
            T result = default;

            var apiRequest = new HttpRequestMessage(HttpMethod.Post, apiEndPoint);

            var content = JsonSerializer.Serialize(request);
            apiRequest.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(apiRequest);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, Converters = { new JsonStringEnumConverter() } };

                result = JsonSerializer.Deserialize<T>(responseStream, options);
            }
            else
            {
                var responseStream = await response.Content.ReadAsStringAsync();
                throw new Exception(responseStream);
            }


            return result;
        }

        public async Task<int> MakeServiePostCall(string apiEndPoint, Object request)
        {
            int result = -1;

            var apiRequest = new HttpRequestMessage(HttpMethod.Post, apiEndPoint);

            var content = JsonSerializer.Serialize(request);
            apiRequest.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(apiRequest);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                result = Convert.ToInt32(responseString);
            }
            else
            {
                var responseStream = await response.Content.ReadAsStringAsync();
                throw new Exception(responseStream);
            }

            return result;
        }

        public async Task MakeServiceQueryPostCall(string apiEndPoint, string queryString)
        {
            var apiRequest = new HttpRequestMessage(HttpMethod.Post, $"{apiEndPoint}?{queryString}");

            var response = await _httpClient.SendAsync(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStringAsync();
                throw new Exception(responseStream);
            }

        }

        public async Task MakeServicePutCall(string apiEndPoint, Object request)
        {
            var apiRequest = new HttpRequestMessage(HttpMethod.Put, apiEndPoint);

            var content = JsonSerializer.Serialize(request);
            apiRequest.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStringAsync();
                throw new Exception(responseStream);
            }
        }


        public async Task MakeServiceDeleteCall(string apiEndPoint)
        {
            var apiRequest = new HttpRequestMessage(HttpMethod.Delete, apiEndPoint);

            var response = await _httpClient.SendAsync(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStringAsync();
                throw new Exception(responseStream);
            }

        }

    }
}
