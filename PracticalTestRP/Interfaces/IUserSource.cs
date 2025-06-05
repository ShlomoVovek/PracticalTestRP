using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PracticalTestRP.Models;

namespace PracticalTestRP.Interfaces
{
    public interface IUserSource
    {
        string SourceName { get; }
        Task<IEnumerable<User>> GetUsersAsync();
    }
}
