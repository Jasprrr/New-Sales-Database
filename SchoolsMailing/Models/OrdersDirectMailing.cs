using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsMailing.Models
{
    public class OrdersDirectMailing
    {
        public long ID { get; set; }
        public long userID { get; set; }
        public long companyID { get; set; }
        public long contactID { get; set; }
        public long promotionID { get; set; }
        public string orderCode { get; set; }

        public DateTime orderDate { get; set; }
        public DateTime orderModified { get; set; }
        public bool orderPayment { get; set; }
        public bool orderContent { get; set; }
        public bool orderRemove { get; set; }

        //Name & Address Details
        public string companyName { get; set; }
        public string companyAddress1 { get; set; }
        public string companyAddress2 { get; set; }
        public string companyCity { get; set; }
        public string companyCounty { get; set; }
        public string companyPostCode { get; set; }
        public string companyProspects { get; set; }

        //Contact Details
        public string contactTitle { get; set; }
        public string contactForename { get; set; }
        public string contactSurname { get; set; }
        public string contactEmail { get; set; }
        public string contactTelephone { get; set; }

        //Order Totals
        public double orderTotal { get; set; }
        public double orderTotalVAT { get; set; }

        //Direct Mailing
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
        public DateTime directCreated { get; set; }
        public DateTime directModified { get; set; }
        public double directCost { get; set; }
    }
}
