﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsMailing.Models
{
    /// <summary>
    /// Adds order data to email data
    /// </summary>
    public class OrdersData
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

        //Data details
        public string dataDetails { get; set; }
        public double dataCost { get; set; }
        public DateTime dataStart { get; set; }
        public DateTime dataEnd { get; set; }
        public DateTime dataCreated { get; set; }
        public DateTime dataModified { get; set; }
    }
}
