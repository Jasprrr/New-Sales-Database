using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsMailing.Models
{
    public class CompanyDataOrder
    {
        public long companyID { get; set; }
        public string companyName { get; set; }
        public long dataID { get; set; }
        public string dataDetails { get; set; }
        public double dataCost { get; set; }
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
