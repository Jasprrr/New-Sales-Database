using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsMailing.Models
{
    public class SchoolSend
    {
        public long ID { get; set; }
        public long orderID { get; set; }
        public long schoolsendPackage { get; set; }
        public DateTime schoolsendStart { get; set; }
        public DateTime schoolsendEnd { get; set; }
        public double schoolsendCost { get; set; }
        public long schoolsendCredits { get; set; }
        public DateTime schoolsendCreated { get; set; }
        public DateTime schoolsendModified { get; set; }
    }
}
