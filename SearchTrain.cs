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
        readonly ApiService apiService = new ApiService();
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
                int stationNumber = FindTrainStationNumberByName(srcTrain.Text);
                
                if (stationNumber == 0)
                {
                    return;
                }

                GetData(stationNumber, mTableLayout);
            };

            btnFavorite.Click += delegate
            {
                string favoriteName = "תחנת רכבת " + srcTrain.Text;
                int stationNumber = FindTrainStationNumberByName(srcTrain.Text);

                if (stationNumber == 0)
                {
                    return;
                }

                dbHelper.AddNewFavorite(this, favoriteName, apiService.GetSrcUrl(stationNumber), (int)SEARCH_TYPE.train);
            };

            string favoriteUrl = "";
            favoriteUrl = Intent.GetStringExtra("url");

            if (favoriteUrl != null && favoriteUrl != "")
            {
                labelFavorite.Visibility = Android.Views.ViewStates.Visible;
                string searchName = Intent.GetStringExtra("searchName");
                labelFavorite.Text = searchName;
                GetData(0, mTableLayout, favoriteUrl);
            }

        }

        private int FindTrainStationNumberByName(string stationName)
        {
            int stationNumber = dbHelper.GetTrainStationNumberByName(stationName);

            if (stationNumber == 0)
            {
                Alert.AlertMessage(this, "תחנת הרכבת לא מופיעה במערכת");
            }

            return stationNumber;
        }


        public async void GetData(int stationNumber, TableLayout mTableLayout, string favoriteUrl = "")
        {
            string urlToSend = favoriteUrl != "" ? favoriteUrl : apiService.GetSrcUrl(stationNumber);
            ApiResponse apiResponse = await apiService.GetDataFromApi(urlToSend);

            if (apiResponse?.Siri?.ServiceDelivery?.StopMonitoringDelivery?[0]?.MonitoredStopVisit?.Count == 0)
            {
                Alert.AlertMessage(this, "אין רכבות בתחנה ב30 הדקות הקרובות");
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