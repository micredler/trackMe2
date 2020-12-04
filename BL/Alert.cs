using Android.App;

namespace trackMe.BL
{
    public static class Alert
    {
        public static void AlertMessage(Activity currentObj, string title, string msg)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(currentObj);
            alert.SetTitle(title);
            alert.SetMessage(msg);

            Dialog dialog = alert.Create();
            dialog.Show();
        }
    }
}