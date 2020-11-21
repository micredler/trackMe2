using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace trackMe.Data
{
    public class DBHelper
    {
        public DBConnection dbConnection = new DBConnection();
        public DBHelper()
        {

        }


        public string ReadStationName(string stopCode)
        {
            SQLiteConnection connection = dbConnection.CreateConnection();
            var stops = connection.Query<Stop>($"SELECT stop_name FROM stops WHERE stop_code = {stopCode}");

            try
            {
                return stops[0].stop_name.ToString();
            }

            catch (Exception)
            {
                return "שם התחנה לא ידוע";
            }
        }
    }
}