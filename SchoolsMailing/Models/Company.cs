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

        [JsonProperty(PropertyName = "companyName")]
        public string companyName { get; set; }

        [JsonProperty(PropertyName = "companyAddress1")]
        public string companyAddress1 { get; set; }

        [JsonProperty(PropertyName = "companyAddress2")]
        public string companyAddress2 { get; set; }

        [JsonProperty(PropertyName = "companyCity")]
        public string companyCity { get; set; }

        [JsonProperty(PropertyName = "companyCounty")]
        public string companyCounty { get; set; }

        [JsonProperty(PropertyName = "companyPostCode")]
        public string companyPostCode { get; set; }

        [JsonProperty(PropertyName = "companyWebsite")]
        public string companyWebsite { get; set; }

        [JsonProperty(PropertyName = "companyProspects")]
        public string companyProspects { get; set; }

        [JsonProperty(PropertyName = "companyTelephone")]
        public string companyTelephone { get; set; }

        [JsonProperty(PropertyName = "companyProduct")]
        public string companyProduct { get; set; }

        [JsonProperty(PropertyName = "companyRemove")]
        public bool companyRemove { get; set; }

        [JsonProperty(PropertyName = "userID")]
        public long userID { get; set; }

        [JsonProperty(PropertyName = "globalID")]
        public string globalID { get; set; }

        [JsonProperty(PropertyName = "companyCreated")]
        public DateTime companyCreated { get; set; }

        [JsonProperty(PropertyName = "companyModified")]
        public DateTime companyModified { get; set; }

        [JsonProperty(PropertyName = "companyActive")]
        public bool companyActive { get; set; }
        public long companySchoolSendID { get; set; }
        public string companyInitial { get; set; }
    }

}
