using Android.App;
using Android.Widget;
using Android.OS;
using System;
using SimpleLoginAndroid.ViewModel;
using Android.Net;

namespace SimpleLoginAndroid
{
    [Activity(Label = "SimpleLoginAndroid", Icon = "@drawable/icon")]
    public class RegisterActivity : Activity
    {

        EditText UserNameTextField;
        EditText PasswordTextField;
        Button CreateButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Register);
            // Create your application here  

            CreateButton = FindViewById<Button>(Resource.Id.createButton);
            UserNameTextField = FindViewById<EditText>(Resource.Id.usernameText);
            PasswordTextField = FindViewById<EditText>(Resource.Id.passwordText);

            CreateButton.Click += CreateButton_Click;
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (CheckForInternet())
            {
                CreateButton.Enabled = true;
            }
            else
            {
                CreateButton.Enabled = false;
            }
        }

        private void CreateButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                LocalDBHandler LocalDBHandler = new LocalDBHandler();

                //Get UserTable from DBHandler 
                var Connection = LocalDBHandler.GetConnection();

                var UserTable = Connection.Table<User>().Where(User => User.Username == UserNameTextField.Text).FirstOrDefault();

                if (UserTable != null)
                {
                    Toast.MakeText(this, "This User Already Exists...", ToastLength.Short).Show();
                }
                else
                {
                    User user = new User();
                    user.Username = UserNameTextField.Text;
                    user.Password = PasswordTextField.Text;

                    Connection.Insert(user);

                    Toast.MakeText(this, "Record Added Successfully...,", ToastLength.Short).Show();
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

