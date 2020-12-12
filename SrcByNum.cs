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
            TextView labelFavorite = FindViewById<TextView>(Resource.Id.labelFavorite);

            string[] OperatorList = dbHelper.GetAllAgencies();

            AutoCompleteTextView operatorAutoComplete = FindViewById<AutoCompleteTextView>(Resource.Id.autoComplete_operator);
            var adapter = new ArrayAdapter<String>(this, Resource.Layout.list_item, OperatorList);

            operatorAutoComplete.Adapter = adapter;

            List<string> directions = GetDirections(txtLine.Text, operatorAutoComplete.Text).Select(direction => direction.destination).ToList();


            btnSearch.Click += delegate
            {
                labelFavorite.Visibility = Android.Views.ViewStates.Invisible;
                labelFavorite.Text = "";
                GetData(txtLine.Text, mTableLayout, operatorAutoComplete.Text);

            };

            btnFavorite.Click += delegate
            {
                string favoriteName = "חברה " + operatorAutoComplete.Text + " קו " + txtLine.Text;
                dbHelper.AddNewFavorite(this, favoriteName, GetSrcUrl(txtLine.Text, operatorAutoComplete.Text), (int)SEARCH_TYPE.line);
                Alert.AlertMessage(this, "הודעת מערכת", favoriteName + " נוסף למועדפים");
                //Alert("the url is", GetSrcUrl(txtLine.Text, operatorAutoComplete.Text));
            };

            string favoriteUrl = "";
            favoriteUrl = Intent.GetStringExtra("url");
            if (favoriteUrl != null && favoriteUrl != "")
            {
                labelFavorite.Visibility = Android.Views.ViewStates.Visible;
                string searchName = Intent.GetStringExtra("searchName");
                labelFavorite.Text = searchName;
                GetData("", mTableLayout, "", favoriteUrl);
            }

        }


        public string GetSrcUrl(string lineNumFromUser, string agencySelected)
        {
            const string AND_SIGN = "%26";
            const string STATION_PARAM = "MonitoringRef=all";
            const string LINE_PARAM = "LineRef=";
            const string CALLS = "StopVisitDetailLevel=calls";

            string routeId = dbHelper.GetRouteIdFromDB(lineNumFromUser, agencySelected);

            if (String.IsNullOrEmpty(routeId))
            {
                Alert.AlertMessage(this, "הודעת מערכת", "מספר הקו לא מופיע במערכת");
                return null;
            }

            else
            {
                return STATION_PARAM + AND_SIGN + LINE_PARAM + routeId + AND_SIGN + CALLS;
            }
        }

        public async void GetData(string lineNumFromUser, TableLayout mTableLayout, string agencySelected, string favoriteUrl = "")
        {
            ApiService apiService = new ApiService();
            string urlToSend = favoriteUrl != "" ? favoriteUrl : GetSrcUrl(lineNumFromUser, agencySelected);
            ApiResponse apiResponse = await apiService.GetDataFromApi(urlToSend);

            if (apiResponse.Siri != null)
            {
                List<MonitoredStopVisit> visits = apiResponse.Siri.ServiceDelivery.StopMonitoringDelivery[0].MonitoredStopVisit.ToList();
                if (visits.Count == 0)
                {
                    Alert.AlertMessage(this, "הודעת מערכת", "לא נמצאו נסיעות קרובות לקו זה");
                    return;
                }
                DataGenerator dataGenerator = new DataGenerator();
                dataGenerator.SetTableData(visits, mTableLayout, this, Resources, "line");
                //setTableData(visits, mTableLayout);
            }
            //System.Diagnostics.Debug.WriteLine(j);
        }
        private List<Route> GetDirections(string lineNumber, string operatorName)
        {
            return dbHelper.GetDirections(lineNumber, operatorName);
        }

    }
}