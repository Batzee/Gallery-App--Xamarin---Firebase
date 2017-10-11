using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using SQLite;

namespace SimpleLoginAndroid
{
    class User
    {

        [PrimaryKey, AutoIncrement, Column("_Id")]

        public int ID { get; set; } // AutoIncrement and set primarykey  

        [MaxLength(25)]

        public string UserName { get; set; }

        [MaxLength(15)]

        public string Password { get; set; }
    }
}