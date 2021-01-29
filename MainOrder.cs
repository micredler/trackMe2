using Android.App;
using Android.OS;
using Android.Widget;

namespace trackMe
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainOrder : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main_order);

            Button btnFavorite = FindViewById<Button>(Resource.Id.btn_favorite);
            btnFavorite.Click += delegate
            {
                StartActivity(typeof(Favorite));
            };

            Button btnMap = FindViewById<Button>(Resource.Id.btn_map);
            btnMap.Click += delegate
            {
                StartActivity(typeof(Map));
            };

            Button btnSrcNum = FindViewById<Button>(Resource.Id.btn_src_num);
            btnSrcNum.Click += delegate
            {
                StartActivity(typeof(SearchByNum));
            };

            Button btnSrcStation = FindViewById<Button>(Resource.Id.btn_src_station);
            btnSrcStation.Click += delegate
            {
                StartActivity(typeof(SearchByStation));
            };

            Button btnTrain = FindViewById<Button>(Resource.Id.btn_train);
            btnTrain.Click += delegate
            {
                StartActivity(typeof(SearchTrain));
            };
        }
    }
}