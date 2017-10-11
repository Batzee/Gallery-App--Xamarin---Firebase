# Simple-Login-Xamarin-Android
This app uses sql lite to hadle local db which enables app to work 100% offline.
And uses firebase to handle remote db.
Synchronization between two tables happen when internet is working.
There are sample two tables to handle user details and image url which enables to extend beyond that. 

##Things to do do before start Developing the App

    1.You have to create the remote DB and tables inside Firebase in your own account. 
    2.Set your own Firebase URL to the "const string FirebaseURL" in ViewModel/FBHandler.cs
    3.Set your LocalDB file name to the "const string DBNameString" in ViewModel/LocalDBHandler.cs



