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
    public class SearchByNum : Activity
    {
        DBHelper dbHelper = new DBHelper();
        ApiService apiService = new ApiService();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.search_by_num);

            TextView txtLine = FindViewById<TextView>(Resource.Id.txt_line_num);
            Button btnSearch = FindViewById<Button>(Resource.Id.btn_search_num);
            TableLayout mTableLayout = FindViewById<TableLayout>(Resource.Id.table_by_num);
            ImageButton btnFavorite = FindViewById<ImageButton>(Resource.Id.save_favorite);
            TextView labelFavorite = FindViewById<TextView>(Resource.Id.labelFavorite);

            string[] OperatorList = dbHelper.GetAllAgencies();

            AutoCompleteTextView operatorAutoComplete = FindViewById<AutoCompleteTextView>(Resource.Id.autoComplete_operator);
            var adapter = new ArrayAdapter<String>(this, Resource.Layout.list_item, OperatorList);

            operatorAutoComplete.Adapter = adapter;

            operatorAutoComplete.ItemClick += new EventHandler<AdapterView.ItemClickEventArgs>(OperatorSelected);
            txtLine.FocusChange += new EventHandler<Android.Views.View.FocusChangeEventArgs>(LineChange);
            txtLine.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(TextLineChange);
            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner);

            btnSearch.Click += delegate
            {
                List<RouteData> directions = GetDirections(txtLine.Text, operatorAutoComplete.Text);

                if (directions.Count == 0)
                {
                    Alert.AlertMessage(this, "אין קווים העונים לחיפוש זה");
                    return;
                }

                int routeIdOfDirectionChoosen = directions.First(d => d.destination.Equals(spinner.SelectedItem.ToString())).route_id;
                labelFavorite.Visibility = Android.Views.ViewStates.Invisible;
                labelFavorite.Text = "";
                GetData(mTableLayout, routeIdOfDirectionChoosen);
            };

            btnFavorite.Click += delegate
            {
                List<RouteData> directions = GetDirections(txtLine.Text, operatorAutoComplete.Text);
                int routeIdOfDirectionChoosen = directions.First(d => d.destination.Equals(spinner.SelectedItem.ToString())).route_id;
                string favoriteName = operatorAutoComplete.Text + " קו " + txtLine.Text;
                dbHelper.AddNewFavorite(this, favoriteName, apiService.GetSrcUrl(this, routeIdOfDirectionChoosen),
                    (int)SEARCH_TYPE.line, spinner.SelectedItem.ToString());

            };

            string favoriteUrl = "";
            favoriteUrl = Intent.GetStringExtra("url");

            if (favoriteUrl != null && favoriteUrl != "")
            {
                labelFavorite.Visibility = Android.Views.ViewStates.Visible;
                string searchName = Intent.GetStringExtra("searchName") + " " + Intent.GetStringExtra("direction");
                labelFavorite.Text = searchName;
                GetData(mTableLayout, 0, favoriteUrl);
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
            TextView txtLine = (TextView)sender;
            AutoCompleteTextView operatorAutoComplete = FindViewById<AutoCompleteTextView>(Resource.Id.autoComplete_operator);

            if (operatorAutoComplete.Text != "" && txtLine.Text != "")
            {
                SetDataForSpinner(operatorAutoComplete.Text, txtLine.Text);
            }
        }

        private void SetDataForSpinner(string operatorText, string line)
        {
            List<RouteData> directions = GetDirections(line, operatorText);
            List<string> optionalDirections = directions.Select(direction => direction.destination).ToList();
            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner);
            var adapterForDirection = new ArrayAdapter<String>(this, Resource.Layout.list_item, optionalDirections);

            adapterForDirection.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapterForDirection;
        }

        public async void GetData(TableLayout mTableLayout, int routeIdOfDirectionChoosen, string favoriteUrl = "")
        {
            string urlToSend = favoriteUrl != "" ? favoriteUrl : apiService.GetSrcUrl(this, routeIdOfDirectionChoosen);
            ApiResponse apiResponse = await apiService.GetDataFromApi(urlToSend);

            if (apiResponse.Siri != null)
            {
                List<MonitoredStopVisit> visits = apiResponse.Siri.ServiceDelivery.StopMonitoringDelivery[0].MonitoredStopVisit.ToList();

                if (visits.Count == 0)
                {
                    Alert.AlertMessage(this, "לא נמצאו נסיעות קרובות לקו זה");
                    return;
                }
                DataGenerator dataGenerator = new DataGenerator();
                dataGenerator.SetTableData(visits, mTableLayout, this, Resources, "line");
            }
        }
        private List<RouteData> GetDirections(string lineNumber, string operatorName)
        {
            int operatorId = dbHelper.GetAgencyNumberByName(operatorName);
            return dbHelper.GetDirections(lineNumber, operatorId.ToString());
        }
    }
}