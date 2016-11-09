using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsMailing.Models
{
    public class Contact
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int companyID { get; set; }
        public string contactTitle { get; set; }
        public string contactForename { get; set; }
        public string contactSurname { get; set; }
        public string contactEmail { get; set; }
        public string contactTelephone { get; set; }
        public bool contactPrimary { get; set; }
        public bool contactRemove { get; set; }
    }
}
