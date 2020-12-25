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
            
            List<FavoriteData> favorites = dBHelper.GetFavorites();
            
            GetDataToScreen();

        }

        public void GetDataToScreen()
        {
            TableLayout mTableLayout = FindViewById<TableLayout>(Resource.Id.table_favorite);
            List<FavoriteData> favorites = dBHelper.GetFavorites();
            List<FavoriteElementId> favoritsElementsId = dataGenerator.SetTableData(favorites, this, Resources, this, mTableLayout);

            SetFunctions(favoritsElementsId);
        }

        public void SetFunctions(List<FavoriteElementId> favoritsElementsId)
        {
            foreach (FavoriteElementId Row in favoritsElementsId)
            {
                Row.Btn.Click += delegate
                {
                    dBHelper.DeleteFavorite(this, Row.Name);
                    Alert.AlertMessage(this, Row.Name + " נמחק");
                    GetDataToScreen();


                };

                Row.Txt.Click += delegate
                {
                    Intent activity;
                    switch (Row.SearchType)
                    {
                        case (int) SEARCH_TYPE.line:
                            activity = new Intent(this, typeof(SearchByNum));
                            activity.PutExtra("direction", (Row as FavoriteLineElementId).Direction);
                            break;
                        case (int)SEARCH_TYPE.train:
                            activity = new Intent(this, typeof(SearchTrain));
                            break;
                        case (int)SEARCH_TYPE.station:
                            activity = new Intent(this, typeof(SearchByStation));
                            break;
                        default:
                            return;
                    }
                    
                    activity.PutExtra("url", dBHelper.GetUrlFavoriteByName(Row.Name));
                    activity.PutExtra("searchName", Row.Name);
                    StartActivity(activity);

                };
            }

        }
    }
}