using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsMailing.Models
{
    class ContactHistory
    {
        public int ID { get; set; }
        public int contactID { get; set; }
        public string contactHistoryDetails { get; set; }
        public DateTime contactHistoryDate { get; set; }
    }
}
