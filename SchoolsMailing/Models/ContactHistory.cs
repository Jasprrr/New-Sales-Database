using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsMailing.Models
{
    class ContactHistory
    {
        [PrimaryKey, AutoIncrement]
        public long ID { get; set; }
        public long contactID { get; set; }
        public string contactHistoryDetails { get; set; }
        public DateTime contactHistoryDate { get; set; }
    }
}
