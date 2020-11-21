using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using trackMe.Data;

namespace trackMe.BL
{
    class DataGenerator
    {
        DBHelper dbHelper = new DBHelper();
        public void SetTableData(List<MonitoredStopVisit> data, TableLayout mTableLayout, Context context, Resources resources
            , string searchType)
        {
            while (mTableLayout.ChildCount > 2)
            {
                mTableLayout.RemoveViewAt(2);
            }
            TableRow.LayoutParams tableRowLayoutparams = new TableRow.LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent);

            Boolean colored = false;
            foreach (MonitoredStopVisit Row in data)
            {
                TableRow tr = new TableRow(context);
                if (colored) tr.SetBackgroundColor(resources.GetColor(Resource.Color.eventRowResult));
                colored = !colored;
                tr.LayoutParameters = tableRowLayoutparams;
                TextView tv1 = new TextView(context);
                switch (searchType)
                {
                    case "station":                      
                        tv1.Text = Row.MonitoredVehicleJourney.PublishedLineName;
                        tv1.TextAlignment = TextAlignment.ViewEnd;
                        break;
                    case "line":
                        tv1.Text = dbHelper.ReadStationName(Row.MonitoredVehicleJourney.MonitoredCall.StopPointRef);
                        tv1.TextAlignment = TextAlignment.ViewEnd;
                        break;
                    default:
                        break;
                }

                TextView tv2 = new TextView(context);
                tv2.Text = Row.MonitoredVehicleJourney.MonitoredCall.ExpectedArrivalTime.ToShortTimeString();
                tv2.TextAlignment = TextAlignment.ViewEnd;

                TextView tv3 = new TextView(context);
                tv3.Text = dbHelper.ReadStationName(Row.MonitoredVehicleJourney.DestinationRef);
                tv3.TextAlignment = TextAlignment.ViewEnd;

                TextView tv4 = new TextView(context);
                if (Row.MonitoredVehicleJourney.OnwardCalls.OnwardCall.Count > 0)
                {
                    OnwardCall endStationCall = Row.MonitoredVehicleJourney.OnwardCalls.OnwardCall.Where(a => a.StopPointRef == Row.MonitoredVehicleJourney.DestinationRef).FirstOrDefault();
                    tv4.Text = endStationCall is null ? "לא ידוע" : endStationCall.ExpectedArrivalTime.ToShortTimeString();
                }
                else
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

    }
}