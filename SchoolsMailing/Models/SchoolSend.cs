using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsMailing.Models
{
    public class SchoolSend
    {
        public int ID { get; set; }
        public int orderID { get; set; }
        public int schoolSendPackage { get; set; }
        public DateTime schoolSendStartDate { get; set; }
        public DateTime schoolSendEndDate { get; set; }
        public double schoolSendCost { get; set; }
    }
}
