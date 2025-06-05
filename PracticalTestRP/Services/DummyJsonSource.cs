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
    public class DummyJsonSource : IUserSource
    {
        private readonly HttpClient _httpClient;
        public string SourceName => "DummyJson";

        public DummyJsonSource(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<DummyJsonApiResponse>("https://dummyjson.com/users");
            return response.Users.Select(u => new User(
                u.FirstName,
                u.LastName,
                u.Email,
                u.Id.ToString()
            ));
        }
    }
}
