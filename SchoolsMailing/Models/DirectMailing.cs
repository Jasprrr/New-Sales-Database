using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsMailing.Models
{
    public class DirectMailing
    {
        public int ID { get; set; }
        public int userID { get; set; }
        public string directDeliveryCode { get; set; }
        public DateTime directDataDate { get; set; }
        public DateTime directInsertDate { get; set; }
        public DateTime directArtworkDate { get; set; }
        public DateTime directDate { get; set; }
        public string directMailingTo { get; set; }
        public string directLeafletCode { get; set; }
        public string directDetails { get; set; }
        public double directFulfilmentCost { get; set; }
        public double directPrintCost { get; set; }
        public double directPostageCost { get; set; }
    }
}
