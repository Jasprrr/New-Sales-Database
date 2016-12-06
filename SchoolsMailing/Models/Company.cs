using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.MobileServices;

namespace SchoolsMailing.Models
{
    public class Company
    {
        [PrimaryKey, AutoIncrement]
        public long ID { get; set; }
        public string companyName { get; set; }
        public string companyAddress1 { get; set; }
        public string companyAddress2 { get; set; }
        public string companyCity { get; set; }
        public string companyCounty { get; set; }
        public string companyPostCode { get; set; }
        public string companyWebsite { get; set; }
        public string companyProspects { get; set; }
        public string companyTelephone { get; set; }
        public string companyProduct { get; set; }
        public bool companyRemove { get; set; }
        public long userID { get; set; }
        public string globalID { get; set; }
        public DateTime companyCreated { get; set; }
        public DateTime companyModified { get; set; }
        public bool companyActive { get; set; }
        public long companySchoolSendID { get; set; }
        public string companyInitial { get; set; }
        public DateTime companyCallBackDate { get; set; }
        public string companyCallBackDetails { get; set; }
    }
}
