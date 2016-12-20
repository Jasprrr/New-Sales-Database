using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsMailing.Models
{
    public class Email
    {
        [PrimaryKey, AutoIncrement]
        public long ID { get; set; }
        public long orderID { get; set; }
        public DateTime emailDate { get; set; }
        public string emailDetails { get; set; }
        public double emailAdminCost { get; set; }
        public double emailDirectCost { get; set; }
        public double emailCost { get; set; }
        public string emailSubject { get; set; }
        public bool emailSetUp { get; set; }
        public DateTime emailCreated { get; set; }
        public DateTime emailModified { get; set; }
    }
}
