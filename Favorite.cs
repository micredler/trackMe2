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
using trackMe.Data;

namespace trackMe
{
    [Activity(Label = "Favorite")]
    public class Favorite : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.favorite);
            // Create your application here

            DBHelper dBHelper = new DBHelper();
            dBHelper.AddNewFavorite("400 to jeru", @"www.google.com");
            var url = dBHelper.GetFavoriteByName("400 to jeru");
        }
    }
}