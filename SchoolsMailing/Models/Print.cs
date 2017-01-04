using SQLite.Net.Attributes;
using System;

namespace SchoolsMailing.Models
{
    public class Print
    {
        [PrimaryKey, AutoIncrement]
        public long ID { get; set; }
        public long orderID { get; set; }
        public string printPrinter { get; set; }
        public string printDetails { get; set; }
        public double printCharge { get; set; }
        public double printCost { get; set; }
        public DateTime printDate { get; set; }
        public DateTime printCreated { get; set; }
        public DateTime printModified { get; set; }
    }
}
