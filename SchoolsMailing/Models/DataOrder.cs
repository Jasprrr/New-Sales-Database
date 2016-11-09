using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsMailing.Models
{
    public class DataOrder
    {
        public int ID { get; set; }
        public int companyID { get; set; }
        public DateTime orderDate { get; set; }
        public string orderCode { get; set; }
        public double dataCost { get; set; }
        public string dataDetails { get; set; }
        public int orderCreator { get; set; }
    }
}
