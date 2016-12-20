using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsMailing.Models
{
    public class SharedMailing
    {
        [PrimaryKey, AutoIncrement]
        public long ID { get; set; }
        public long orderID { get; set; }
        public string sharedDeliveryCode { get; set; }
        public DateTime sharedDate { get; set; }
        public string sharedMailingTo { get; set; }
        public DateTime sharedArtworkDate { get; set; }
        public string sharedLeafletName { get; set; }
        public long sharedNumberOfLeaflets { get; set; }
        public string sharedFAO { get; set; }
        public string sharedLeafletSize { get; set; }
        public string sharedLeafletWeight { get; set; }
        public DateTime sharedDeliveryDate { get; set; }
        public long sharedPackage { get; set; }
        public double sharedCost { get; set; }
        public DateTime sharedCreated { get; set; }
        public DateTime sharedModified { get; set; }
    }
}
