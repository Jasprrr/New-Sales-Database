using SQLite.Net.Attributes;

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
