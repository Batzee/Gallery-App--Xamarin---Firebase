using SQLite.Net.Attributes;

namespace testapp
{
    public class UserTable
    {
        [PrimaryKey, AutoIncrement, Column("Id")]

        public int id { get; set; }

        [MaxLength(10)]
        public string username { get; set; }

        [MaxLength(10)]
        public string password { get; set; }

    }

}