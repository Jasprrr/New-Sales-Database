using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsMailing.Models
{
    public class SharedPack
    {
        [PrimaryKey, AutoIncrement]
        public long ID { get; set; }

        public DateTime packDate { get; set; }
        public DateTime packArtworkDate { get; set; }
        public DateTime packDeliveryDate { get; set; }
        public string packName { get; set; }
        public string packTo { get; set; }
        public int packCurrentInserts { get; set; }
        public int packMaxInserts { get; set; }
        public double packCost { get; set; }
    }
}
