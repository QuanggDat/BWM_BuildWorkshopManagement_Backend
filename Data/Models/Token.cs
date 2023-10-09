using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Token
    {
        public string Access_token { get; set; }
        public string Token_type { get; set; }
        public string userID { get; set; }
        public int Expires_in { get; set; }
        public string fullName { get; set; }
        public string PhoneNumber { get; set; }
        public Role Role { get; set; }
    }
}
