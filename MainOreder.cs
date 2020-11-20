using Android.App;
using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using Android.OS;
using Android.Views;
using Android.Widget;
using trackMe.Data;

namespace trackMe
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainOreder : Activity
    {
        DBHelper db;
        SQLiteDatabase sqliteDB;
        LinearLayout container;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main_order);

            db = new DBHelper(this);
            sqliteDB = db.WritableDatabase;
            container = FindViewById<LinearLayout>(Resource.Id.container);
            Button btnGetData;


            // when somrthing happen ------> GetData();
            btnGetData = FindViewById<Button>(Resource.Id.btnLoadData);
            btnGetData.Click += delegate
            {
                GetData();
            };

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
                StartActivity(typeof(SrcByNum));
            };

            Button btnSrcStation = FindViewById<Button>(Resource.Id.btn_src_station);
            btnSrcStation.Click += delegate
            {
                StartActivity(typeof(SrcByStation));
            };

            //Button btnSrcAdd = FindViewById<Button>(Resource.Id.btn_src_add);
            //btnSrcAdd.Click += delegate {
            //    StartActivity(typeof(SrcByAdd));
            //};
            Button btnTrain = FindViewById<Button>(Resource.Id.btn_train);
            btnTrain.Click += delegate
            {
                StartActivity(typeof(Train));
            };
            //Button btnMaptst = FindViewById<Button>(Resource.Id.btn_maptst);
            //btnMaptst.Click += delegate {
            //    var uri = Android.Net.Uri.Parse("https://new.govmap.gov.il/?c=179239.52,666541.24&z=8&lay=KIDS_G,BUS_STOPS,TRAIN_STATOINS");
            //    var intent = new Intent(Intent.ActionView, uri);
            //    StartActivity(intent);
            //};
        }

        public void GetData()
        {
            ICursor selectData = sqliteDB.RawQuery("select * from stops", new string[] { });
            if (selectData.Count > 0)
            {
                selectData.MoveToFirst();
                StopStation stopStation = new StopStation();
                stopStation.stop_name = selectData.GetString(selectData.GetColumnIndex("stop_name"));
                stopStation.stop_des = selectData.GetString(selectData.GetColumnIndex("stop_des"));
                selectData.Close();
                LayoutInflater layoutInflater = (LayoutInflater)BaseContext.GetSystemService(Context.LayoutInflaterService);
                View addView = layoutInflater.Inflate(Resource.Layout.data, null);
                TextView txtStopName = addView.FindViewById<TextView>(Resource.Id.txtStopName);
                TextView txtStopDes = addView.FindViewById<TextView>(Resource.Id.txtStopDes);
                txtStopName.Text = stopStation.stop_name;
                txtStopDes.Text = stopStation.stop_des;
                container.AddView(addView);
            }
        }
    }
}