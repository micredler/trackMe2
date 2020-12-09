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
using trackMe.BL;
using trackMe.Data;

namespace trackMe
{
    [Activity(Label = "Favorite")]
    public class Favorite : Activity
    {
        DBHelper dBHelper = new DBHelper();
        DataGenerator dataGenerator = new DataGenerator();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.favorite);
            // Create your application here

            //dBHelper.AddNewFavorite("400 to jeru", @"www.google.com");
            List<FavoriteData> favorites = dBHelper.GetFavorites();
            
            GetDataToScreen();

        }

        public void GetDataToScreen()
        {
            TableLayout mTableLayout = FindViewById<TableLayout>(Resource.Id.table_favorite);
            List<FavoriteData> favorites = dBHelper.GetFavorites();
            List<FavoriteElementId> favoritsElementsId = dataGenerator.setFavorites(favorites, this, Resources, this, mTableLayout);

            SetFunctions(favoritsElementsId);
        }

        public void SetFunctions(List<FavoriteElementId> favoritsElementsId)
        {
            foreach (FavoriteElementId Row in favoritsElementsId)
            {
                Row.Btn.Click += delegate
                {
                    dBHelper.DeleteFavorite(this, Row.Name);
                    Alert.AlertMessage(this, "הודעת מערכת", Row.Name + " נמחק");
                    GetDataToScreen();


                };

                Row.Txt.Click += delegate
                {
                    Intent activity = new Intent(this, typeof(SrcByStation));

                    activity.PutExtra("Url", dBHelper.GetUrlFavoriteByName(Row.Name));
                    StartActivity(activity);

                };
            }

        }
    }
}