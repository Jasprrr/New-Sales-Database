using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsMailing.Models
{
    public class CompanyEmailOrder
    {
        public long companyID { get; set; }
        public string companyName { get; set; }
        public long emailID { get; set; }
        public DateTime emailDate { get; set; }
        public string emailDetails { get; set; }
        public double emailAdminCost { get; set; }
        public double emailDirectCost { get; set; }
        public double emailCost { get; set; }
        public string emailSubject { get; set; }
        public bool emailSetUp { get; set; }
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
