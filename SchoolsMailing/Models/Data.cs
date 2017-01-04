using SQLite.Net.Attributes;
using System;

namespace SchoolsMailing.Models
{
    public class Data
    {
        [PrimaryKey, AutoIncrement]
        public long ID { get; set; }
        public long orderID { get; set; }
        public string dataDetails { get; set; }
        public double dataCost { get; set; }
        public DateTime dataStart { get; set; }
        public DateTime dataEnd { get; set; }
        public DateTime dataCreated { get; set; }
        public DateTime dataModified { get; set; }
    }
}
