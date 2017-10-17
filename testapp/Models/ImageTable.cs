using SQLite.Net.Attributes;

namespace testapp
{
    class ImageTable
    {
        [PrimaryKey, AutoIncrement, Column("Id")]

        public int id { get; set; }

        public string userName { get; set; }
        
        public string imageUri { get; set; }
        
        
    }
}