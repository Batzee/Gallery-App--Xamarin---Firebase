using Android.App;
using Android.Widget;
using Android.OS;
using System;
using SimpleLoginAndroid.ViewControllers;
using Android.Content;
using SimpleLoginAndroid.ViewModel;
using Android.Net;

namespace SimpleLoginAndroid
{
    [Activity(Label = "SimpleLoginAndroid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        EditText UserNameTextField;
        EditText PasswordTextField;
        Button LoginButton;
        Button RegisterButton;

        LocalDBHandler LocalDBHandler;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            LoginButton = FindViewById<Button>(Resource.Id.loginButton);
            RegisterButton = FindViewById<Button>(Resource.Id.registerButton);

            UserNameTextField = FindViewById<EditText>(Resource.Id.usernameText);
            PasswordTextField = FindViewById<EditText>(Resource.Id.passwordText);

            LoginButton.Click += Login_Click;
            RegisterButton.Click += Register_Click;

            LocalDBHandler = new LocalDBHandler();
            LocalDBHandler.CreateDatabase();  
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (CheckForInternet())
            {
                FBHandler FBHandler = new FBHandler();

                FBHandler.SyncUserData();
                FBHandler.SyncImageData();

                LoginButton.Enabled = true;
                RegisterButton.Enabled = true;
            }
            else
            {
                LoginButton.Enabled = false;
                RegisterButton.Enabled = false;
            }
        }

        private void Register_Click(object sender, System.EventArgs e)
        {
            StartActivity(typeof(RegisterActivity));
        }

        private void Login_Click(object sender, System.EventArgs e)
        {
            try
            {
                //Get UserTable from DBHandler 
                var Connection = LocalDBHandler.GetConnection();
                var UserTable = Connection.Table<User>();

                //Get CurrentUser from UserTable
                var CurrentUser = UserTable.Where(x => x.Username == UserNameTextField.Text && x.Password == PasswordTextField.Text).FirstOrDefault();

                if (CurrentUser != null)
                {
                    Toast.MakeText(this, "Login Success", ToastLength.Short).Show();

                    //Instantiating next ImagePickerView and Passing Username to that view
                    var ImagePickerActivity = new Intent(this, typeof(ImagePickerActivity));
                    ImagePickerActivity.PutExtra("ActivityData", CurrentUser.Username);

                    StartActivity(ImagePickerActivity);
                }
                else
                {
                    Toast.MakeText(this, "Username or Password invalid", ToastLength.Short).Show();
                }
            }

            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
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

