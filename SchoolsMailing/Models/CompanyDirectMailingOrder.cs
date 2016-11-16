using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsMailing.Models
{
    public class CompanyDirectMailingOrder
    {
        public long companyID { get; set; }
        public string companyName { get; set; }
        public long directID { get; set; }
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
        public long orderID { get; set; }
        public long userID { get; set; }
        public long contactID { get; set; }
        public long promotionID { get; set; }
        public string orderCode { get; set; }
        public double orderTotal { get; set; }
        public double orderTotalVAT { get; set; }
        public DateTime orderDate { get; set; }
        public DateTime orderModified { get; set; }
        public bool orderPayment { get; set; }
        public bool orderContent { get; set; }
        public bool orderRemove { get; set; }
    }
}
