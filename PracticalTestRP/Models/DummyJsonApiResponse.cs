using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalTestRP.Models
{
    public class DummyJsonApiResponse
    {
        public DummyJsonUser[] Users { get; set; }
    }

    public class DummyJsonUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
