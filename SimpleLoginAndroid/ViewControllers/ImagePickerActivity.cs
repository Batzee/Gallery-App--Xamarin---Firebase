using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using SimpleLoginAndroid.ViewModel;
using Android.Net;

namespace SimpleLoginAndroid.ViewControllers
{
    [Activity(Label = "AddImage")]
    public class ImagePickerActivity : Activity
    {
        Button PickImageButton;
        Button AddImageButton;
        Button SyncButton;

        Android.Net.Uri PickedImageURI;

        string userName;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ImagePicker);
            // Create your application here

            userName = Intent.GetStringExtra("ActivityData") ?? "Data not available";

            this.Title = "Hi " + userName;

            PickImageButton = FindViewById<Button>(Resource.Id.pickImageButton);
            AddImageButton = FindViewById<Button>(Resource.Id.addImageButton);
            SyncButton = FindViewById<Button>(Resource.Id.syncButton);

            PickImageButton.Click += PickImageButton_Click;
            AddImageButton.Click += AddImageButton_Click;
            SyncButton.Click += SyncButton_Click;

            AddImageButton.Enabled = false;
        }
        private void PickImageButton_Click(object sender, EventArgs e)
        {
            //Showing the picked Image in the view
            var ImageIntent = new Intent();
            ImageIntent.SetType("image/*");
            ImageIntent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(
                Intent.CreateChooser(ImageIntent, "Select photo"), 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            //base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                var imageView = FindViewById<ImageView>(Resource.Id.imageView);

                PickedImageURI = data.Data;
                imageView.SetImageURI(PickedImageURI);
                AddImageButton.Enabled = true;
            }
        }

        private void AddImageButton_Click(object sender, EventArgs e)
        {
            //Get LocalDB Connection via DBHandler 
            LocalDBHandler LocalDBHandler = new LocalDBHandler();
            var Connection = LocalDBHandler.GetConnection();

            //Get CurrentUser from UserTable 
            var CurrentUser = Connection.Table<User>().Where(c => c.UserName.Equals(userName)).FirstOrDefault();

            //Get the list of users current/exisitng images
            var CurrentUserImages = Connection.Table<Image>().Where(d => d.UserName.Equals(userName)).ToList();

            //Creating the list for Image URIs of current user
            List<string> CurrentUserImageURIs = new List<string>();

            //Iterate Images of CurrentUser to get ImageURIs of those Images
            if (CurrentUserImages.Count > 0)
            {
                foreach (var CurrentUserImage in CurrentUserImages)
                {
                    CurrentUserImageURIs.Add(CurrentUserImage.ImageURI);
                }
            }

            //Checking whether user has already added the image
            if (CurrentUserImageURIs.Contains(PickedImageURI.ToString()))
            {
                Toast.MakeText(this, "You have already addded this image...,", ToastLength.Short).Show();
            }
            else
            {
                //Creating new image object with image details if user has not added the specific image
                Image image = new Image();
                image.UserName = CurrentUser.UserName;
                image.ImageURI = PickedImageURI.ToString();

                Connection.Insert(image);

                Toast.MakeText(this, "Image Successfully Added...,", ToastLength.Short).Show();
            }

            AddImageButton.Enabled = false;
        }


        private void SyncButton_Click(object sender, EventArgs e)
        {
            if (CheckForInternet())
            {
                FBHandler FBHandler = new FBHandler();

                FBHandler.SyncUserData();
                FBHandler.SyncImageData();
            }
        }

        public bool CheckForInternet()
        {
            //CheckForInternet
            ConnectivityManager connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);

            NetworkInfo networkInfo = connectivityManager.ActiveNetworkInfo;

            if (networkInfo != null)
            {
                Toast.MakeText(this, "Syncing...", ToastLength.Short).Show();

                return true;
            }
            else
            {
                Toast.MakeText(this, "Internet Connection Appears To Be Offline...", ToastLength.Short).Show();
                return false;
            }
        }
    }
}