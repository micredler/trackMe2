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

namespace trackMe
{
    [Activity(Label = "SrcByNum")]
    public class SrcByNum : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.src_by_num);
            // Create your application here
            TextView txtLine = FindViewById<TextView>(Resource.Id.txt_line_num);
            Button btnSearch = FindViewById<Button>(Resource.Id.btn_search_num);
            TableLayout mTableLayout = FindViewById<TableLayout>(Resource.Id.table_by_num);

            btnSearch.Click += delegate
            {
                GetData(txtLine.Text, mTableLayout);

            };

        }

        public async void GetData(string lineNum, TableLayout mTableLayout)
        {
            const string AND_SIGN = "%26";
            const string STATION_PARAM = "MonitoringRef=all";
            const string LINE_PARAM = "LineRef=";
            const string CALLS = "StopVisitDetailLevel=calls";
            ApiService apiService = new ApiService();

            ApiResponse j = await apiService.GetDataFromApi(STATION_PARAM + AND_SIGN + LINE_PARAM + lineNum + AND_SIGN + CALLS);
            if (!(j.Siri.ServiceDelivery.StopMonitoringDelivery[0].MonitoredStopVisit is null))
            {

                List<MonitoredStopVisit> visits = j.Siri.ServiceDelivery.StopMonitoringDelivery[0].MonitoredStopVisit.ToList();
                setTableData(visits, mTableLayout);
            }
            else
            {
                Alert("הודעת מערכת", "אין נתונים להצגה");
            }
            //System.Diagnostics.Debug.WriteLine(j);
        }

        public void setTableData(List<MonitoredStopVisit> data, TableLayout mTableLayout)
        {
            while (mTableLayout.ChildCount > 1)
            {
                mTableLayout.RemoveViewAt(1);
            }
            TableRow.LayoutParams tableRowLayoutparams = new TableRow.LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent);

            foreach (MonitoredStopVisit Row in data)
            {
                TableRow tr = new TableRow(this);

                tr.LayoutParameters = tableRowLayoutparams;

                TextView tv1 = new TextView(this);
                tv1.Text = Row.MonitoredVehicleJourney.MonitoredCall.StopPointRef;
                tv1.TextAlignment = TextAlignment.ViewEnd;

                TextView tv2 = new TextView(this);
                tv2.Text = Row.MonitoredVehicleJourney.MonitoredCall.ExpectedArrivalTime.ToShortTimeString();
                tv2.TextAlignment = TextAlignment.ViewEnd;

                TextView tv3 = new TextView(this);
                tv3.Text = Row.MonitoredVehicleJourney.DestinationRef;
                tv3.TextAlignment = TextAlignment.ViewEnd;

                TextView tv4 = new TextView(this);
                if (Row.MonitoredVehicleJourney.OnwardCalls.OnwardCall.Count > 0)
                {
                    OnwardCall endStationCall = Row.MonitoredVehicleJourney.OnwardCalls.OnwardCall.Where(a => a.StopPointRef == Row.MonitoredVehicleJourney.DestinationRef).FirstOrDefault();
                    tv4.Text = endStationCall is null ? "לא ידוע" : endStationCall.ExpectedArrivalTime.ToShortTimeString();
                } else
                {
                    tv4.Text = tv2.Text;
                }
                //tv4.Text = Row.MonitoredVehicleJourney.DestinationRef;
                tv4.TextAlignment = TextAlignment.ViewEnd;

                tr.AddView(tv4);
                tr.AddView(tv3);
                tr.AddView(tv2);
                tr.AddView(tv1);
                mTableLayout.AddView(tr);

            }
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