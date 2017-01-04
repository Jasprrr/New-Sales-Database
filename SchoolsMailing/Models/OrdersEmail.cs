using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsMailing.Models
{
    class OrdersEmail
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

        //Email
        public DateTime emailDate { get; set; }
        public string emailDetails { get; set; }
        public double emailAdminCost { get; set; }
        public double emailDirectCost { get; set; }
        public double emailCost { get; set; }
        public string emailSubject { get; set; }
        public bool emailSetUp { get; set; }
        public DateTime emailCreated { get; set; }
        public DateTime emailModified { get; set; }
    }
}
