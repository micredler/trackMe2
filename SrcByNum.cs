using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        Boolean writeNow = false;
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

            // List<Route> directions = GetDirections(txtLine.Text, operatorAutoComplete.Text);
         
            // List<string> optionalDirections = directions.Select(direction => direction.destination).ToList();

            // here you'll use the direction
            // and choose string from optionalDirections
            // var fakeDirectionThatChoosen = "קו לתל אביב";

            // int routeIdOfDirectionChoosen = 5; // directions.First(d => d.destination.Equals(fakeDirectionThatChoosen)).route_id;
            operatorAutoComplete.ItemClick += new EventHandler<AdapterView.ItemClickEventArgs>(OperatorSelected);
            txtLine.FocusChange += new EventHandler<Android.Views.View.FocusChangeEventArgs> (LineChange);
            txtLine.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(TextLineChange);
            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner);




            btnSearch.Click += delegate
            {
                List<Route> directions = GetDirections(txtLine.Text, operatorAutoComplete.Text);
                int routeIdOfDirectionChoosen = directions.First(d => d.destination.Equals(spinner.SelectedItem.ToString())).route_id;
                labelFavorite.Visibility = Android.Views.ViewStates.Invisible;
                labelFavorite.Text = "";
                GetData(txtLine.Text, mTableLayout, operatorAutoComplete.Text, routeIdOfDirectionChoosen);
            };

            btnFavorite.Click += delegate
            {
                List<Route> directions = GetDirections(txtLine.Text, operatorAutoComplete.Text);
                int routeIdOfDirectionChoosen = directions.First(d => d.destination.Equals(spinner.SelectedItem.ToString())).route_id;
                string favoriteName = "חברה " + operatorAutoComplete.Text + " קו " + txtLine.Text;
                dbHelper.AddNewFavorite(this, favoriteName, GetSrcUrl(routeIdOfDirectionChoosen), (int)SEARCH_TYPE.line);
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
                GetData("", mTableLayout, "", 0, favoriteUrl);
            }
        }

        private void OperatorSelected(object sender, AdapterView.ItemClickEventArgs e)
        {
            AutoCompleteTextView operatorAutoComplete = (AutoCompleteTextView)sender;
            TextView txtLine = FindViewById<TextView>(Resource.Id.txt_line_num);
            string line = txtLine.Text;
            if (operatorAutoComplete.Text != "" && line != "")
            {
                SetDataForSpinner(operatorAutoComplete.Text, line);
            }
            // Alert.AlertMessage(this, "test", operatorAutoComplete.Text);
        }

        private void LineChange(object sender, Android.Views.View.FocusChangeEventArgs e)
        {
            if (!e.HasFocus)
            {
                TextView txtLine = (TextView)sender;
                AutoCompleteTextView operatorAutoComplete = FindViewById<AutoCompleteTextView>(Resource.Id.autoComplete_operator);
                if (operatorAutoComplete.Text != "" && txtLine.Text != "")
                {
                    SetDataForSpinner(operatorAutoComplete.Text, txtLine.Text);
                }
            }
        }
        private void TextLineChange(object sender, Android.Text.TextChangedEventArgs e)
        {
            writeNow = true;
            System.Threading.Tasks.Task.Factory.StartNew(() => {
                Thread.Sleep(800); // delay execution for 500 ms
               writeNow = false;
                Alert.AlertMessage(this, "1", "1");
                
            });
            System.Threading.Tasks.Task.Factory.StartNew(() => {
                Thread.Sleep(1500); // delay execution for 500 ms
                Alert.AlertMessage(this, "2", "2");
                if (!writeNow)
                {
                    TextView txtLine = (TextView)sender;
                    AutoCompleteTextView operatorAutoComplete = FindViewById<AutoCompleteTextView>(Resource.Id.autoComplete_operator);
                    if (operatorAutoComplete.Text != "" && txtLine.Text != "")
                    {
                        SetDataForSpinner(operatorAutoComplete.Text, txtLine.Text);
                        Alert.AlertMessage(this, "test", txtLine.Text);
                    }
                }

            });
            
        }
        
        private void SetDataForSpinner(string operatorText, string line)
        {
            List<Route> directions = GetDirections(line, operatorText);
            List<string> optionalDirections = directions.Select(direction => direction.destination).ToList();
            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner);
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapterForDirection = new ArrayAdapter<String>(this, Resource.Layout.list_item, optionalDirections);
            // var adapterForDirection = ArrayAdapter.CreateFromResource(this, Resource.Array.car_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapterForDirection.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapterForDirection;
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            string toast = string.Format("Selected car is {0}", spinner.GetItemAtPosition(e.Position));
            Toast.MakeText(this, toast, ToastLength.Long).Show();
        }

        public string GetSrcUrl(int routeIdOfDirectionChoosen)
        {
            const string AND_SIGN = "%26";
            const string STATION_PARAM = "MonitoringRef=all";
            const string LINE_PARAM = "LineRef=";
            const string CALLS = "StopVisitDetailLevel=calls";

            if (routeIdOfDirectionChoosen == 0)
            {
                Alert.AlertMessage(this, "הודעת מערכת", "מספר הקו לא מופיע במערכת");
                return null;
            }

            else
            {
                return STATION_PARAM + AND_SIGN + LINE_PARAM + routeIdOfDirectionChoosen + AND_SIGN + CALLS;
            }
        }

        public async void GetData(string lineNumFromUser, TableLayout mTableLayout, string agencySelected, int routeIdOfDirectionChoosen, string favoriteUrl = "")
        {
            ApiService apiService = new ApiService();
            string urlToSend = favoriteUrl != "" ? favoriteUrl : GetSrcUrl(routeIdOfDirectionChoosen);
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
            int operatorId = dbHelper.GetAgencyNumberByName(operatorName);
            return dbHelper.GetDirections(lineNumber, operatorId.ToString());
        }

    }
}