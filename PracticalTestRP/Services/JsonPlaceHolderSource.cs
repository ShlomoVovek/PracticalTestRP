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
    public class JsonPlaceholderSource : IUserSource
    {
        private readonly HttpClient _httpClient;
        public string SourceName => "JsonPlaceholder";

        public JsonPlaceholderSource(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var users = await _httpClient.GetFromJsonAsync<JsonPlaceholderUser[]>("https://jsonplaceholder.typicode.com/users");
            
            return users.Select(u => {
                var nameParts = u.Name.Split(' ', 2);
                return new User(
                    nameParts.Length > 0 ? nameParts[0] : "",
                    nameParts.Length > 1 ? nameParts[1] : "",
                    u.Email,
                    u.Id.ToString()
                );
            });
        }
    }
}
