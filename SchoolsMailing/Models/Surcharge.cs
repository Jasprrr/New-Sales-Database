using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsMailing.Models
{
    public class Surcharge
    {
        public long ID { get; set; }
        public long orderID { get; set; }
        public string surchargeDetails { get; set; }
        public double surchargeCost { get; set; }
    }
}
