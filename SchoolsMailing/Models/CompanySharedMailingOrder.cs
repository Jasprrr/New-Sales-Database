using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsMailing.Models
{
    public class CompanySharedMailingOrder
    {
        public long companyID { get; set; }
        public string companyName { get; set; }
        public long sharedID { get; set; }
        public string sharedDeliveryCode { get; set; }
        public DateTime sharedDate { get; set; }
        public string sharedMailingTo { get; set; }
        public DateTime sharedArtworkDate { get; set; }
        public string sharedLeafletName { get; set; }
        public long sharedNumberOfLeaflets { get; set; }
        public string sharedFAO { get; set; }
        public string sharedLeafletSize { get; set; }
        public int sharedLeafletWeight { get; set; }
        public DateTime sharedDeliveryDate { get; set; }
        public long sharedPack { get; set; }
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
        public long packID { get; set; }
        public DateTime packDate { get; set; }
        public string packName { get; set; }
        public string packTo { get; set; }
    }
}
