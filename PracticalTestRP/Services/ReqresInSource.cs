using PracticalTestRP.Interfaces;
using PracticalTestRP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PracticalTestRP.Services
{
    public class ReqresInSource : IUserSource
    {
        private readonly HttpClient _httpClient;
        public string SourceName => "ReqresIn";

        private const string ApiUrl = "https://reqres.in/api/users";
        private const string ApiKeyHeaderName = "x-api-key";
        private const string ApiKeyValue = "reqres-free-v1";

        public ReqresInSource(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, ApiUrl);

            request.Headers.Add(ApiKeyHeaderName, ApiKeyValue);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var apiResponse = await response.Content.ReadFromJsonAsync<ReqresApiResponse>();

            if (apiResponse?.Data == null)
            {
                return Enumerable.Empty<User>();
            }

            return apiResponse.Data.Select(u => new User(
                u.First_Name,
                u.Last_Name,
                u.Email,
                u.Id.ToString()
            ));
        }
    }
}
