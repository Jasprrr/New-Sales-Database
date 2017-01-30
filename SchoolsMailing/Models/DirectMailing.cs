using SQLite.Net.Attributes;
using System;

namespace SchoolsMailing.Models
{
    public class DirectMailing
    {
        [PrimaryKey, AutoIncrement]
        public long ID { get; set; }
        public long orderID { get; set; }
        public string directDeliveryCode { get; set; }
        public DateTime? directDataDate { get; set; }
        public DateTime? directInsertDate { get; set; }
        public DateTime? directArtworkDate { get; set; }
        public DateTime directDate { get; set; }
        public string directMailingTo { get; set; }
        public string directLeafletCode { get; set; }
        public string directDetails { get; set; }
        public double directFulfilmentCost { get; set; }
        public double directPrintCost { get; set; }
        public double directPostageCost { get; set; }
        public DateTime directCreated { get; set; }
        public DateTime directModified { get; set; }
        public double directCost { get; set; }
    }
}
