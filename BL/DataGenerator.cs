using System;
using System.Collections.Generic;
using System.Globalization;
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
            // TO DO
            // חיפוש לפי קו לשמור כל סבב שכבר נכנסתי אליו לפי המשתנה
            // DatedVehicleJourneyRef
            // ואז לקרוא את כל התחנות שלו פעם אחת לפי המערך של ה
            // OnwardCalls
            if (searchType == "line")
            {
                List<string> idLoop = new List<string>();
               
                foreach (MonitoredStopVisit Row in data.OrderBy(s => s.MonitoredVehicleJourney.FramedVehicleJourneyRef.DatedVehicleJourneyRef))
                {
                    
                    string crntIdLoop = Row.MonitoredVehicleJourney.FramedVehicleJourneyRef.DatedVehicleJourneyRef;
                    if (crntIdLoop == "0" || idLoop.FindIndex(x => x == crntIdLoop) >= 0)
                    {
                        // go to next row
                        // in search by line we display only one time per bus route
                    }
                    else
                    {

                        idLoop.Add(crntIdLoop);
                        if (Row.MonitoredVehicleJourney.OnwardCalls.OnwardCall.Count > 0)
                        {
                            if (idLoop.Count > 0) // after complete bus round adding space
                            {
                                Space space = new Space(context);
                                space.SetMinimumHeight(40);
                                mTableLayout.AddView(space);
                            }
                            foreach (OnwardCall call in Row.MonitoredVehicleJourney.OnwardCalls.OnwardCall)
                            {
                                TableRow tr = new TableRow(context);
                                if (colored) tr.SetBackgroundColor(resources.GetColor(Resource.Color.eventRowResult));
                                colored = !colored;
                                tr.LayoutParameters = tableRowLayoutparams;

                                TextView tv1 = new TextView(context);
                                string firstStationName = dbHelper.ReadStationName(call.StopPointRef);
                                firstStationName = firstStationName.Replace("/הורדה", "");
                                tv1.Text = firstStationName;
                                tv1.TextAlignment = TextAlignment.ViewEnd;
                                tv1.SetPadding(0, 0, 13, 0);
                                TextView tv2 = new TextView(context);
                                tv2.Text = DateTime.Parse(call.ExpectedArrivalTime.ToShortTimeString()).ToString("HH:mm", CultureInfo.CurrentCulture);
                                tv2.TextAlignment = TextAlignment.Center;

                                TextView tv3 = new TextView(context);
                                string stationName = dbHelper.ReadStationName(Row.MonitoredVehicleJourney.DestinationRef);
                                stationName = stationName.Replace("/הורדה", "");
                                tv3.Text = stationName;
                                tv3.TextAlignment = TextAlignment.Center;

                                TextView tv4 = new TextView(context);
                                if (Row.MonitoredVehicleJourney.OnwardCalls.OnwardCall.Count > 0)
                                {
                                    OnwardCall endStationCall = Row.MonitoredVehicleJourney.OnwardCalls.OnwardCall.Where(a => a.StopPointRef == Row.MonitoredVehicleJourney.DestinationRef).FirstOrDefault();
                                    tv4.Text = endStationCall is null ? "לא ידוע" : DateTime.Parse(endStationCall.ExpectedArrivalTime.ToShortTimeString()).ToString("HH:mm", CultureInfo.CurrentCulture);
                                }
                                else
                                {
                                    tv4.Text = tv2.Text;
                                }
                                //tv4.Text = Row.MonitoredVehicleJourney.DestinationRef;
                                tv4.TextAlignment = TextAlignment.Center;

                                tr.AddView(tv4);
                                tr.AddView(tv3);
                                tr.AddView(tv2);
                                tr.AddView(tv1);
                                mTableLayout.AddView(tr);
                            }
                        }
        
                    }

                }
            }
            else // by station and train
            {

                foreach (MonitoredStopVisit Row in data)
                {

                    TableRow tr = new TableRow(context);
                    if (colored) tr.SetBackgroundColor(resources.GetColor(Resource.Color.eventRowResult));
                    colored = !colored;
                    tr.LayoutParameters = tableRowLayoutparams;
                    TextView tv1 = new TextView(context);
                    tv1.SetPadding(0, 0, 13, 0);
                    switch (searchType)
                    {
                        case "station":
                            tv1.Text = Row.MonitoredVehicleJourney.PublishedLineName;
                            tv1.TextAlignment = TextAlignment.Center;
                            break;
                        default:
                            break;
                    }

                    TextView tv2 = new TextView(context);
                    tv2.Text = DateTime.Parse(Row.MonitoredVehicleJourney.MonitoredCall.ExpectedArrivalTime.ToShortTimeString()).ToString("HH:mm", CultureInfo.CurrentCulture);
                    tv2.TextAlignment = TextAlignment.Center;

                    TextView tv3 = new TextView(context);
                    string stationName = dbHelper.ReadStationName(Row.MonitoredVehicleJourney.DestinationRef);
                    stationName = stationName.Replace("/הורדה", "");
                    tv3.Text = stationName;
                    tv3.TextAlignment = TextAlignment.Center;

                    TextView tv4 = new TextView(context);
                    if (Row.MonitoredVehicleJourney.OnwardCalls.OnwardCall.Count > 0)
                    {
                        OnwardCall endStationCall = Row.MonitoredVehicleJourney.OnwardCalls.OnwardCall.Where(a => a.StopPointRef == Row.MonitoredVehicleJourney.DestinationRef).FirstOrDefault();
                        tv4.Text = endStationCall is null ? "לא ידוע" : DateTime.Parse(endStationCall.ExpectedArrivalTime.ToShortTimeString()).ToString("HH:mm", CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        tv4.Text = tv2.Text;
                    }
                    //tv4.Text = Row.MonitoredVehicleJourney.DestinationRef;
                    tv4.TextAlignment = TextAlignment.Center;

                    tr.AddView(tv4);
                    tr.AddView(tv3);
                    tr.AddView(tv2);
                    tr.AddView(tv1);
                    mTableLayout.AddView(tr);

                }
            }
        }

    }
}