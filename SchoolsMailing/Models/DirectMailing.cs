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
        public string directMailingDeliveryCode { get; set; }
        public DateTime directMailingDataDate { get; set; }
        public DateTime directMailingInsertDate { get; set; }
        public DateTime directMailingArtworkDate { get; set; }
        public DateTime directMailingMailingDate { get; set; }
        public string directMailingMailingTo { get; set; }
        public string directMailingLeafletCode { get; set; }
        public string directMailingDetails { get; set; }
        public double directMailingFulfilmentCost { get; set; }
        public double directMailingPrintCost { get; set; }
        public double directMailingPostageCost { get; set; }
    }
}
