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
    [Activity(Label = "SrcByNum")]
    public class SrcByNum : Activity
    {
        DBHelper dBHelper = new DBHelper();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.src_by_num);
            // Create your application here
            TextView txtLine = FindViewById<TextView>(Resource.Id.txt_line_num);
            Button btnSearch = FindViewById<Button>(Resource.Id.btn_search_num);
            TableLayout mTableLayout = FindViewById<TableLayout>(Resource.Id.table_by_num);
            //fake
            string agencySelected = "3";

            btnSearch.Click += delegate
            {
                GetData(txtLine.Text, agencySelected, mTableLayout);

            };

        }

        public async void GetData(string lineNumFromUser, string agencySelected, TableLayout mTableLayout)
        {
            const string AND_SIGN = "%26";
            const string STATION_PARAM = "MonitoringRef=all";
            const string LINE_PARAM = "LineRef=";
            const string CALLS = "StopVisitDetailLevel=calls";

            string routeId = dBHelper.GetRouteIdFromDB(lineNumFromUser, agencySelected);

            ApiService apiService = new ApiService();

            ApiResponse j = await apiService.GetDataFromApi(STATION_PARAM + AND_SIGN + LINE_PARAM + routeId + AND_SIGN + CALLS);
            if (!(j.Siri.ServiceDelivery.StopMonitoringDelivery[0].MonitoredStopVisit is null) &&
                !(j.Siri.ServiceDelivery.StopMonitoringDelivery[0].MonitoredStopVisit.Count == 0))
            {

                List<MonitoredStopVisit> visits = j.Siri.ServiceDelivery.StopMonitoringDelivery[0].MonitoredStopVisit.ToList();
                DataGenerator dataGenerator = new DataGenerator();
                dataGenerator.SetTableData(visits, mTableLayout, this, Resources, "line");
                //setTableData(visits, mTableLayout);
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