using System.Collections.Generic;
using System.Linq;
using Firebase.Xamarin.Database;
using SQLite;

namespace SimpleLoginAndroid.ViewModel
{
    class FBHandler
    {
        private const string FirebaseURL = "Add your Firebase DB URL";

        public SQLiteConnection Connection;

        public FirebaseClient Firebase;

        public FBHandler()
        {
            //Get LocalDB Connection via DBHandler 
            LocalDBHandler LocalDBHandler = new LocalDBHandler();
            Connection = LocalDBHandler.GetConnection();

            //Firebase DB Connection
            Firebase = new FirebaseClient(FirebaseURL);
        }

        public async void SyncUserData()
        {
            //Getting LocalUsers List
            var LocalUsers = Connection.Table<User>().ToList();

            //Creating new list for LocalUserNames
            List<string> LocalUserNames = new List<string>();

            foreach (var LocalUser in LocalUsers)
            {
                LocalUserNames.Add(LocalUser.UserName);
            }

            //Getting FBUsers List
            var FBUserTable = await Firebase.Child("Users").OnceAsync<User>();

            //Creating two lists for RemoteUsers and RemoteUserNames
            List<User> RemoteUsers = new List<User>();
            List<string> RemoteUserNames = new List<string>();

            foreach (var Row in FBUserTable)
            {
                RemoteUsers.Add(Row.Object);
                RemoteUserNames.Add(Row.Object.UserName);
            }

            //Creating a list for Unique UserNames
            var FinalUserNames = LocalUserNames.Union(RemoteUserNames).ToList();

            //Creating a list for Final Unique Users
            List<User> FinalUsers = new List<User>();

            foreach (var FinalUserName in FinalUserNames)
            {
                //Checking both Local And Remote Users whether those include the name
                if (LocalUserNames.Contains(FinalUserName))
                {
                    User FinalUser = LocalUsers.Where(User => User.UserName == FinalUserName).FirstOrDefault();
                    FinalUsers.Add(FinalUser);
                }
                else
                {
                    User FinalUser = RemoteUsers.Where(User => User.UserName == FinalUserName).FirstOrDefault();
                    FinalUsers.Add(FinalUser);
                }
            }

            //Recreating Local DB Table with new list
            Connection.DropTable<User>();
            Connection.CreateTable<User>();
            Connection.InsertAll(FinalUsers);


            //Recreating FB DB Table with new list
            await Firebase.Child("Users").DeleteAsync();

            foreach (var FinalUser in FinalUsers)
            {
                await Firebase.Child("Users").PostAsync(FinalUser);
            }
        }

        public async void SyncImageData()
        {
            //Getting Local User Image List
            var LocalImages = Connection.Table<Image>().ToList();

            //Creating a list for Local Image IDs
            List<int> LocalImageIDs = new List<int>();

            foreach (var LocalImage in LocalImages)
            {
                LocalImageIDs.Add(LocalImage.ImageID);
            }

            //Getting FB Image Table
            var FBImageTable = await Firebase.Child("Images").OnceAsync<Image>();

            //Creating two lists for RemoteImages and RemoteImageIDs
            List<Image> RemoteImages = new List<Image>();
            List<int> RemoteImageIDs = new List<int>();

            foreach (var Row in FBImageTable)
            {
                RemoteImages.Add(Row.Object);
                RemoteImageIDs.Add(Row.Object.ImageID);
            }

            //Creating a list for Unique LocalImageIDs
            var FinalImageIDs = LocalImageIDs.Union(RemoteImageIDs).ToList();

            //Creating a list for Final Unique Users
            List<Image> FinalImages = new List<Image>();

            foreach (var FinalImageID in FinalImageIDs)
            {
                //Checking both Local And Remote Users whether those include the name
                if (LocalImageIDs.Contains(FinalImageID))
                {
                    Image FinalImage = LocalImages.Where(Image => Image.ImageID == FinalImageID).FirstOrDefault();
                    FinalImages.Add(FinalImage);
                }
                else
                {
                    Image FinalImage = RemoteImages.Where(Image => Image.ImageID == FinalImageID).FirstOrDefault();
                    FinalImages.Add(FinalImage);
                }
            }

            //Recreating Local DB Table with new list
            Connection.DropTable<Image>();
            Connection.CreateTable<Image>();
            Connection.InsertAll(FinalImages);

            //Recreating FB DB ImageTable with new list
            await Firebase.Child("Images").DeleteAsync();

            foreach (var FinalImage in FinalImages)
            {
                await Firebase.Child("Images").PostAsync(FinalImage);
            }
        }
    }
}
        