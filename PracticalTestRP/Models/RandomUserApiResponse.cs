using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalTestRP.Models
{
    public class RandomUserApiResponse
    {
        public RandomUserResult[] Results {  get; set; }
    }

    public class RandomUserResult
    {
        public Name Name { get; set; }
        public string Email { get; set; }
        public Login Login { get; set; }
    }

    public class Name
    {
        public string Title { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
    }

    public class Login
    {
        public string Uuid { get; set; }
    }
}
