using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsMailing.Models
{
    public class SharedPack
    {
        public long ID { get; set; }
        public DateTime packDate { get; set; }
        public string packName { get; set; }
        public string packTo { get; set; }
        public int packCurrentInserts { get; set; }
        public int packMaxInserts { get; set; }
    }
}
