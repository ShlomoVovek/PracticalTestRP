using PracticalTestRP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalTestRP.Interfaces
{
    public interface IDataExporter
    {
        Task ExportAsync(string folderPath, string fileName, IEnumerable<User> users);
    }
}
