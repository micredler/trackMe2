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
            webView.Settings.DomStorageEnabled = true;

            webView.Settings.MixedContentMode = MixedContentHandling.AlwaysAllow;
            webView.LoadUrl("file:///android_asset/map.html");
        }
    }
}