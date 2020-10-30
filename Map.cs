using System;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using Android.Webkit;

namespace trackMe
{
    [Activity(Label = "Map", Theme = "@style/AppTheme", MainLauncher = true)]
    public class Map : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.map);

            WebView webView = FindViewById<WebView>(Resource.Id.webView);
            webView.SetWebViewClient(new WebViewClient());
            webView.Settings.JavaScriptEnabled = true;
            webView.LoadUrl("https://www.govmap.gov.il/map.html?b=0&c=180611.64,663993.55&z=4&lay=TRAIN_STATOINS,BUS_STOPS");
        }
    }
}