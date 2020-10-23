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
            btnMap.Click += async delegate {
                await Launcher.OpenAsync("geo:0,0?q=394+Pacific+Ave+San+Francisco+CA");
            };

            Button btnSrcNum = FindViewById<Button>(Resource.Id.btn_src_num);
            btnSrcNum.Click += delegate {
                StartActivity(typeof(SrcByNum));
            };

            Button btnSrcStation = FindViewById<Button>(Resource.Id.btn_src_station);
            btnSrcStation.Click += delegate {
                StartActivity(typeof(SrcByStation));
            };

            Button btnSrcAdd = FindViewById<Button>(Resource.Id.btn_src_add);
            btnSrcAdd.Click += delegate {
                StartActivity(typeof(SrcByAdd));
            };
            Button btnTrain = FindViewById<Button>(Resource.Id.btn_train);
            btnTrain.Click += delegate {
                StartActivity(typeof(Train));
            };
        }
    }
}