using PracticalTestRP.Interfaces;
using PracticalTestRP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Xml;

namespace PracticalTestRP.Services
{
    public class JsonExporter : IDataExporter
    {
        public async Task ExportAsync(string folderPath, string fileName, IEnumerable<User> users)
        {
            var filePath = Path.Combine(folderPath, $"{fileName}.json");

            var json = JsonConvert.SerializeObject(users, Newtonsoft.Json.Formatting.Indented);
            await File.WriteAllTextAsync(filePath, json);
            Console.WriteLine($"Users saved to: {filePath}");
        }
    }
}
