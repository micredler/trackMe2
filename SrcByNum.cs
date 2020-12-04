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
        DBHelper dbHelper = new DBHelper();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.src_by_num);
            // Create your application here
            TextView txtLine = FindViewById<TextView>(Resource.Id.txt_line_num);
            Button btnSearch = FindViewById<Button>(Resource.Id.btn_search_num);
            TableLayout mTableLayout = FindViewById<TableLayout>(Resource.Id.table_by_num);
            ImageButton btnFavorite = FindViewById<ImageButton>(Resource.Id.save_favorite);

            string[] OperatorList = dbHelper.GetAllAgencies();

            AutoCompleteTextView operatorAutoComplete = FindViewById<AutoCompleteTextView>(Resource.Id.autoComplete_operator);
            var adapter = new ArrayAdapter<String>(this, Resource.Layout.list_item, OperatorList);

            operatorAutoComplete.Adapter = adapter;


            btnSearch.Click += delegate
            {
                GetData(txtLine.Text, operatorAutoComplete.Text, mTableLayout);

            };

            btnFavorite.Click += delegate
            {
                Alert("the url is", GetSrcUrl(txtLine.Text));
            };

        }



        public string GetSrcUrl(string lineNumFromUser)


        {
            const string AND_SIGN = "%26";
            const string STATION_PARAM = "MonitoringRef=all";
            const string LINE_PARAM = "LineRef=";
            const string CALLS = "StopVisitDetailLevel=calls";

            string routeId = dbHelper.GetRouteIdFromDB(lineNumFromUser, agencySelected);

            return STATION_PARAM + AND_SIGN + LINE_PARAM + routeId + AND_SIGN + CALLS;
        }

        public async void GetData(string lineNumFromUser, TableLayout mTableLayout)
        {
 
            ApiService apiService = new ApiService();

            ApiResponse j = await apiService.GetDataFromApi(GetSrcUrl(lineNumFromUser));
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