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
        public int schoolsendPackage { get; set; }
        public DateTime schoolsendStartDate { get; set; }
        public DateTime schoolsendEndDate { get; set; }
        public double schoolsendCost { get; set; }
        public long schoolsendCredits { get; set; }
    }
}
