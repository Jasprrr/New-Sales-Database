using SQLite.Net.Attributes;
using System;

namespace SchoolsMailing.Models
{
    public class SchoolSend
    {
        [PrimaryKey, AutoIncrement]
        public long ID { get; set; }
        public long orderID { get; set; }
        public long schoolsendPackage { get; set; }
        public DateTime schoolsendStart { get; set; }
        public DateTime schoolsendEnd { get; set; }
        public double schoolsendCost { get; set; }
        public long schoolsendCredits { get; set; }
        public DateTime schoolsendCreated { get; set; }
        public DateTime schoolsendModified { get; set; }
    }
}
