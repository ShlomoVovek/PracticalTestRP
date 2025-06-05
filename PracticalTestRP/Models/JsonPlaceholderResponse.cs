using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalTestRP.Models
{
    public class JsonPlaceholderResponse
    {
        public DummyJsonUser[] Users { get; set; }
    }
    public class JsonPlaceholderUser() 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
