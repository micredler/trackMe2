﻿using System;
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

namespace trackMe
{
    [Activity(Label = "SrcByStation")]
    public class SrcByStation : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.src_by_station);
            // Create your application here
            TextView txtStation = FindViewById<TextView>(Resource.Id.txt_staion_num);
            Button btnSearch = FindViewById<Button>(Resource.Id.btn_search_station);
            TableLayout mTableLayout = FindViewById<TableLayout>(Resource.Id.table_by_station);

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