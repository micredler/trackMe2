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
    public class Train : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.train);
            // Create your application here
            DBHelper dbHelper = new DBHelper();
            string[] TRAIN_STATION = dbHelper.GetAllTrainStopsName();

        AutoCompleteTextView textView = FindViewById<AutoCompleteTextView>(Resource.Id.autocomplete_train);
            var adapter = new ArrayAdapter<String>(this, Resource.Layout.list_item, TRAIN_STATION);

            textView.Adapter = adapter;

            TextView txtStation = FindViewById<TextView>(Resource.Id.txt_train);
            Button btnSearch = FindViewById<Button>(Resource.Id.btn_search_train);
            TableLayout mTableLayout = FindViewById<TableLayout>(Resource.Id.table_by_train);
            AutoCompleteTextView srcTrain = FindViewById<AutoCompleteTextView>(Resource.Id.autocomplete_train);

       
            btnSearch.Click += delegate
            {
                GetData(txtStation.Text, mTableLayout);

            };




        }
        public async void GetData(string stationNum, TableLayout mTableLayout)
        {
            const string AND_SIGN = "%26";
            const string STATION_PARAM = "MonitoringRef=";
            const string CALLS = "StopVisitDetailLevel=calls";
            ApiService apiService = new ApiService();
            ApiResponse j = await apiService.GetDataFromApi(STATION_PARAM + stationNum + AND_SIGN + CALLS);
            if (!(j.Siri.ServiceDelivery.StopMonitoringDelivery[0].MonitoredStopVisit is null) &&
                !(j.Siri.ServiceDelivery.StopMonitoringDelivery[0].MonitoredStopVisit.Count == 0))
            {

                List<MonitoredStopVisit> visits = j.Siri.ServiceDelivery.StopMonitoringDelivery[0].MonitoredStopVisit.ToList();
                DataGenerator dataGenerator = new DataGenerator();
                dataGenerator.SetTableData(visits, mTableLayout, this, Resources, "station");
                // setTableData(visits, mTableLayout);
            }
            else
            {
                Alert("הודעת מערכת", "אין נתונים להצגה");
            }
            //System.Diagnostics.Debug.WriteLine(j);
        }

        public void Alert(string title, string msg)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle(title);
            alert.SetMessage(msg);

            Dialog dialog = alert.Create();
            dialog.Show();
        }
    }
}