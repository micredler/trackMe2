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
        private string GetShortDest(string dest)
        {
            string shortDest = "";
            string[] destParts = dest.Replace("/הורדה", "").Split("/");
            int relevantIndex = destParts.GetUpperBound(0);
            Boolean secondPart = false;
            foreach (string part in destParts)
            {
                if (secondPart) { shortDest += "/"; }
                string[] destWords = part.Split(" ");
                if (destWords.Length > 1)
                {
                    shortDest += destWords[destWords.GetUpperBound(0) - 1];
                    shortDest += " " + destWords[destWords.GetUpperBound(0)];
                }
                else
                {
                    shortDest += part;
                }
                secondPart = true;
            }

            return shortDest;
        }
        public List<FavoriteElementId> SetTableData(List<FavoriteData> favorites,
            Context context, Resources resources, TableLayout mTableLayout)
        {
            while (mTableLayout.ChildCount > 0)
            {
                mTableLayout.RemoveViewAt(0);
            }

            TableRow.LayoutParams tableRowLayoutparams = new TableRow.LayoutParams(
            ViewGroup.LayoutParams.WrapContent,
            ViewGroup.LayoutParams.WrapContent);

            List<FavoriteElementId> response = new List<FavoriteElementId>();

            Boolean colored = true;
            foreach (FavoriteData Row in favorites)
            {

                TableRow tr = new TableRow(context);
                if (colored) tr.SetBackgroundColor(resources.GetColor(Resource.Color.eventRowResult));
                colored = !colored;
                tr.LayoutParameters = tableRowLayoutparams;
                Boolean WITH_DIRECTION = Row.direction != null;
                TextView tv1 = new TextView(context);
                tv1.Text = Row.name + (WITH_DIRECTION ? " " + Row.direction : "");
                tv1.TextAlignment = TextAlignment.ViewEnd;

                tv1.SetPadding(0, 0, 13, 0);
                tv1.SetMinimumWidth(320);
                tv1.SetTextSize(Android.Util.ComplexUnitType.Dip, WITH_DIRECTION ? 14 : 22);
                tv1.SetMaxWidth(1000);
                ImageButton deleteBtn = new ImageButton(context);

                deleteBtn.SetImageResource(Android.Resource.Drawable.IcMenuDelete);
                deleteBtn.SetMaxWidth(30);
                deleteBtn.SetBackgroundColor(Android.Graphics.Color.Transparent);

                deleteBtn.TextAlignment = TextAlignment.ViewEnd;

                tr.AddView(tv1);
                tr.AddView(deleteBtn);
                mTableLayout.AddView(tr);
                if (Row.direction == null)
                {
                    response.Add(new FavoriteElementId(deleteBtn, tv1, Row.name, Row.searchType));
                }
                else
                {
                    response.Add(new FavoriteLineElementId(deleteBtn, tv1, Row.name, Row.searchType, Row.direction));
                }
            }
            return response;
        }

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
                                firstStationName = GetShortDest(firstStationName); // design the destination

                                tv1.Text = firstStationName;
                                tv1.TextAlignment = TextAlignment.ViewEnd;
                                tv1.SetMaxWidth(400);
                                tv1.SetPadding(0, 0, 13, 0);
                                TextView tv2 = new TextView(context);
                                tv2.Text = DateTime.Parse(call.ExpectedArrivalTime.ToShortTimeString()).ToString("HH:mm", CultureInfo.CurrentCulture);
                                tv2.TextAlignment = TextAlignment.Center;

                                TextView tv3 = new TextView(context);
                                tv3.SetMaxWidth(400);
                                string stationName = dbHelper.ReadStationName(Row.MonitoredVehicleJourney.DestinationRef);
                                stationName = GetShortDest(stationName);

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