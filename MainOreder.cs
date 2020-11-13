using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;

namespace trackMe
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainOreder : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main_order);

            Button btnFavorite = FindViewById<Button>(Resource.Id.btn_favorite);
            btnFavorite.Click += delegate {
                StartActivity(typeof(Favorite));
            };

            Button btnMap = FindViewById<Button>(Resource.Id.btn_map);
            btnMap.Click += delegate {
                StartActivity(typeof(Map));
            };

            Button btnSrcNum = FindViewById<Button>(Resource.Id.btn_src_num);
            btnSrcNum.Click += delegate {
                StartActivity(typeof(SrcByNum));
            };

            Button btnSrcStation = FindViewById<Button>(Resource.Id.btn_src_station);
            btnSrcStation.Click += delegate {
                StartActivity(typeof(SrcByStation));
            };

            //Button btnSrcAdd = FindViewById<Button>(Resource.Id.btn_src_add);
            //btnSrcAdd.Click += delegate {
            //    StartActivity(typeof(SrcByAdd));
            //};
            Button btnTrain = FindViewById<Button>(Resource.Id.btn_train);
            btnTrain.Click += delegate {
                StartActivity(typeof(Train));
            };
            //Button btnMaptst = FindViewById<Button>(Resource.Id.btn_maptst);
            //btnMaptst.Click += delegate {
            //    var uri = Android.Net.Uri.Parse("https://new.govmap.gov.il/?c=179239.52,666541.24&z=8&lay=KIDS_G,BUS_STOPS,TRAIN_STATOINS");
            //    var intent = new Intent(Intent.ActionView, uri);
            //    StartActivity(intent);
            //};
        }
    }
}