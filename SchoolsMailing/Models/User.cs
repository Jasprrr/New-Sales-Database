using SQLite.Net.Attributes;

namespace SchoolsMailing.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public long ID { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }
        public string userInitials { get; set; }
        public long userCode { get; set; }
    }
}
