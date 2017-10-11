using SQLite;

namespace SimpleLoginAndroid
{
    class Image
    {
        [PrimaryKey, AutoIncrement, Column("_ImageID")]

        public int ImageID { get; set; } // AutoIncrement and set primarykey

        public string UserName { get; set; }

        public string ImageURI { get; set; }
    }
}