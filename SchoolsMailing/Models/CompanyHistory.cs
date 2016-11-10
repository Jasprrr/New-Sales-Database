using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsMailing.Models
{
    public class CompanyHistory
    {
        public int ID { get; set; }
        public int companyID { get; set; }
        public string companyHistoryDetails { get; set; }
        public DateTime companyHistoryDate { get; set; }
        public bool isVisible { get; set; }
    }
}
