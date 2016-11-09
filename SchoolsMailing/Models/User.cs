using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsMailing.Models
{
    public class User
    {
        public int ID { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }
        public string userInitials { get; set; }
    }
}
