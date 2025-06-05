using CsvHelper;
using PracticalTestRP.Interfaces;
using PracticalTestRP.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalTestRP.Services
{
    public class CsvExporter : IDataExporter
    {
        public async Task ExportAsync(string folderPath, string fileName, IEnumerable<User> users)
        {
            var filePath = Path.Combine(folderPath, $"{fileName}.csv");
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(users); // CsvHelper handles writing the header and all records
            }
            Console.WriteLine($"Users saved to: {filePath}");
        }
    }
}
