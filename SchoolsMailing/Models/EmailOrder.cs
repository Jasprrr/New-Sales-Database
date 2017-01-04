using SQLite.Net.Attributes;
using System;

namespace SchoolsMailing.Models
{
    class EmailOrder
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int companyID { get; set; }
        public DateTime sendDate { get; set; }
        public string orderCode { get; set; }
        public double emailCost { get; set; }
        public string emailDetails { get; set; }
        public int orderCreator { get; set; }
    }
}
