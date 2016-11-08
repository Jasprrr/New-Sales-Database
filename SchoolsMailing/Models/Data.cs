using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsMailing.Models
{
    public class Data
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int orderID { get; set; }
        public string dataDetails { get; set; }
        public double dataCost { get; set; }
    }
}
