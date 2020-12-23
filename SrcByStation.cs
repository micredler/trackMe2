using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using trackMe.BL;
using trackMe.Data;

namespace trackMe
{
    [Activity(Label = "SrcByStation")]
    public class SrcByStation : Activity
    {
        readonly DBHelper dbHelper = new DBHelper();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.src_by_station);

            TextView txtStation = FindViewById<TextView>(Resource.Id.txt_staion_num);
            Button btnSearch = FindViewById<Button>(Resource.Id.btn_search_station);
            TableLayout mTableLayout = FindViewById<TableLayout>(Resource.Id.table_by_station);
            ImageButton btnFavorite = FindViewById<ImageButton>(Resource.Id.save_favorite);
            TextView labelFavorite = FindViewById<TextView>(Resource.Id.labelFavorite);

            btnSearch.Click += delegate
            {
                labelFavorite.Visibility = Android.Views.ViewStates.Invisible;
                labelFavorite.Text = "";
                GetData(txtStation.Text, mTableLayout);

            };

            btnFavorite.Click += delegate
            {
                string favoriteName = "תחנה " + txtStation.Text;
                dbHelper.AddNewFavorite(this, favoriteName, GetSrcUrl(txtStation.Text), (int) SEARCH_TYPE.station);
                Alert.AlertMessage(this, "הודעת מערכת", favoriteName + " נוסף למועדפים");
            };
            string favoriteUrl = "";
            favoriteUrl = Intent.GetStringExtra("url");
            if (favoriteUrl != null && favoriteUrl != "")
            {
                string searchName = Intent.GetStringExtra("searchName");
                labelFavorite.Visibility = Android.Views.ViewStates.Visible;
                labelFavorite.Text = searchName;
                GetData("", mTableLayout, favoriteUrl);
            }
        }

        public string GetSrcUrl(string stationNum)
        {
            const string AND_SIGN = "%26";
            const string STATION_PARAM = "MonitoringRef=";
            const string CALLS = "StopVisitDetailLevel=calls";

            return STATION_PARAM + stationNum + AND_SIGN + CALLS;
        }

        public async void GetData(string stationNum, TableLayout mTableLayout, string favoriteUrl = "")
        {
            ApiService apiService = new ApiService();

            string urlToSend = favoriteUrl != "" ? favoriteUrl : GetSrcUrl(stationNum);
            ApiResponse apiResponse = await apiService.GetDataFromApi(urlToSend);
            try
            {
                
                List<MonitoredStopVisit> visits = apiResponse.Siri.ServiceDelivery.StopMonitoringDelivery[0].MonitoredStopVisit.ToList();
                if (visits.Count == 0)
                {
                    Alert.AlertMessage(this, "הודעת מערכת", "לא נמצאו נסיעות קרובות לתחנה זו");
                    return;
                }
                DataGenerator dataGenerator = new DataGenerator();
                dataGenerator.SetTableData(visits, mTableLayout, this, Resources, "station");
            }
            catch
            {
                Alert.AlertMessage(this, "הודעת מערכת", "מספר התחנה לא מופיע במערכת");
            }
        }
        
    }
}