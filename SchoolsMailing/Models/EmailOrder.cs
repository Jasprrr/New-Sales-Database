using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsMailing.Models
{
    class EmailOrder
    {
        public int ID { get; set; }
        public int companyID { get; set; }
        public DateTime sendDate { get; set; }
        public string orderCode { get; set; }
        public double emailCost { get; set; }
        public string emailDetails { get; set; }
        public int orderCreator { get; set; }
    }
}
