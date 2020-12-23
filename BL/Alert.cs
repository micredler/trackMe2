using Android.App;

namespace trackMe.BL
{
    public static class Alert
    {
        const string TITLE = "הודעת מערכת";
        public static void AlertMessage(Activity currentObj, string msg)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(currentObj);
            alert.SetTitle(TITLE);
            alert.SetMessage(msg);

            Dialog dialog = alert.Create();
            dialog.Show();
        }
    }
}