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
    internal class RandomUserSource : IUserSource
    {
        private readonly HttpClient _httpClient;
        public string SourceName => "RandomUser";

        public RandomUserSource(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<RandomUserApiResponse>(
                "https://randomuser.me/api/?results=500");
            return response.Results.Select(r => new User(
                r.Name.First,
                r.Name.Last,
                r.Email,
                r.Login.Uuid // Use the UUID as SourceId
            ));
        }
    }
}
