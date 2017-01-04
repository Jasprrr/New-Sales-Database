using SQLite.Net.Attributes;
using System;

namespace SchoolsMailing.Models
{
    public class Surcharge
    {
        [PrimaryKey, AutoIncrement]
        public long ID { get; set; }
        public long orderID { get; set; }
        public DateTime surchargeDate { get; set; }
        public string surchargeDetails { get; set; }
        public double surchargeCost { get; set; }
        public DateTime surchargeCreated { get; set; }
        public DateTime surchargeModified { get; set; }
    }
}
