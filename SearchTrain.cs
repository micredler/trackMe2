using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.OS;
using Android.Widget;
using trackMe.BL;
using trackMe.Data;

namespace trackMe
{
    [Activity(Label = "Train")]
    public class SearchTrain : Activity
    {
        readonly DBHelper dbHelper = new DBHelper();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.search_train);
            
            string[] TRAIN_STATION = dbHelper.GetAllTrainStopsName();

            AutoCompleteTextView textView = FindViewById<AutoCompleteTextView>(Resource.Id.autocomplete_train);
            var adapter = new ArrayAdapter<String>(this, Resource.Layout.list_item, TRAIN_STATION);

            textView.Adapter = adapter;

            Button btnSearch = FindViewById<Button>(Resource.Id.btn_search_train);
            TableLayout mTableLayout = FindViewById<TableLayout>(Resource.Id.table_by_train);
            AutoCompleteTextView srcTrain = FindViewById<AutoCompleteTextView>(Resource.Id.autocomplete_train);
            ImageButton btnFavorite = FindViewById<ImageButton>(Resource.Id.save_favorite);
            TextView labelFavorite = FindViewById<TextView>(Resource.Id.labelFavorite);

            btnSearch.Click += delegate
            {
                labelFavorite.Visibility = Android.Views.ViewStates.Invisible;
                labelFavorite.Text = "";
                GetData(srcTrain.Text, mTableLayout);

            };

            btnFavorite.Click += delegate
            {
                string favoriteName = "תחנת רכבת " + srcTrain.Text;
                dbHelper.AddNewFavorite(this, favoriteName, GetSrcUrl(srcTrain.Text), (int) SEARCH_TYPE.train);
                Alert.AlertMessage(this, "הודעת מערכת", favoriteName + " נוסף למועדפים");
                
            };

            string favoriteUrl = "";
            favoriteUrl = Intent.GetStringExtra("url");
            if (favoriteUrl != null && favoriteUrl != "")
            {
                labelFavorite.Visibility = Android.Views.ViewStates.Visible;
                string searchName = Intent.GetStringExtra("searchName");
                labelFavorite.Text = searchName;
                GetData("", mTableLayout, favoriteUrl);
            }

        }

        public string GetSrcUrl(string trainStationName)
        {
            const string AND_SIGN = "%26";
            const string STATION_PARAM = "MonitoringRef=";
            const string CALLS = "StopVisitDetailLevel=calls";
            int stationNumber = dbHelper.GetTrainStationNumberByName(trainStationName);

            if (stationNumber == 0)
            {
                Alert.AlertMessage(this, "הודעת מערכת", "תחנת הרכבת לא מופיעה במערכת");
                return null;
            }

            else
            {

                return STATION_PARAM + stationNumber + AND_SIGN + CALLS;
            }
        }

        public async void GetData(string trainStationName, TableLayout mTableLayout, string favoriteUrl = "")
        {
            ApiService apiService = new ApiService();
            string urlToSend = favoriteUrl != "" ? favoriteUrl : GetSrcUrl(trainStationName);
            ApiResponse apiResponse = await apiService.GetDataFromApi(urlToSend);

            if (apiResponse?.Siri?.ServiceDelivery?.StopMonitoringDelivery?[0]?.MonitoredStopVisit?.Count == 0)
            { 
                Alert.AlertMessage(this, "הודעת מערכת", "אין רכבות בתחנה ב30 הדקות הקרובות");
            }

            else if (apiResponse.Siri != null)
            {
                List<MonitoredStopVisit> visits = apiResponse.Siri.ServiceDelivery.StopMonitoringDelivery[0].MonitoredStopVisit.ToList();
                DataGenerator dataGenerator = new DataGenerator();
                dataGenerator.SetTableData(visits, mTableLayout, this, Resources, "station");
            }

        }
    }
}