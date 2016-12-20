using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsMailing.Models
{
    public class SchoolSendPack
    {
        [PrimaryKey, AutoIncrement]
        public long ID { get; set; }
        public string packName { get; set; }
        public long packCredits { get; set; }
        public double packCost { get; set; }
    }
}
