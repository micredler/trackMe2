using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using trackMe.Data;

namespace trackMe
{
    public class ViewHolder : Java.Lang.Object
    {
        public TextView txtStopName { get; set; }
        public TextView txtStopDes { get; set; }
    }
    public class ListViewAdapter : BaseAdapter
    {
        private Activity activity;
        private List<StopStation> listStopStation;
        public ListViewAdapter(Activity activity, List<StopStation> listStopStation)
        {
            this.activity = activity;
            this.listStopStation = listStopStation;
        }
        public override int Count
        {
            get { return listStopStation.Count; }
        }
        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }
        public override long GetItemId(int position)
        {
            return listStopStation[position].stop_code;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.src_by_station, parent, false);
            var txtDestination = view.FindViewById<TextView>(Resource.Id.textView4);
            txtDestination.Text = listStopStation[position].stop_des;
            return view;
        }
    }
}