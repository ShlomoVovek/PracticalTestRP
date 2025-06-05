using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalTestRP.Models
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string SourceId { get; set; }


        public User(string firstName, string lastName, string email, string sourceId)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            SourceId = sourceId;
        }


    }
}
