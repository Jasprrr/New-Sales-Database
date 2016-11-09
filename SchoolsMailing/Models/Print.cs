using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsMailing.Models
{
    public class Print
    {
        public int ID { get; set; }
        public int orderID { get; set; }
        public string printPrinter { get; set; }
        public string printDetails { get; set; }
        public double printCharge { get; set; }
        public double printCost { get; set; }
    }
}
