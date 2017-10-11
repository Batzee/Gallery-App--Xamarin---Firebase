using SQLite;
using System.IO;

namespace SimpleLoginAndroid.ViewModel
{
    class LocalDBHandler
    {
        private const string DBNameString = "Your local db file name";

        public SQLiteConnection Connection;

        public LocalDBHandler()
        { 
            //Local DB Path 
            var LocalDBPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), DBNameString);
            Connection = new SQLiteConnection(LocalDBPath);
        }

        public string CreateDatabase()
        {
            try
            {
                Connection.CreateTable<User>();
                Connection.CreateTable<Image>();

                return "Database created";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        public SQLiteConnection GetConnection()
        {
            return Connection;
        }
    }
}