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

namespace trackMe.Data
{
    public class DBHelper
    {
        public DBHelper()
        {

        }

        public static List<Stop> Read(string db_path)
        {
            List<Stop> stops = new List<Stop>();

            using (var connection = new SQLite.SQLiteConnection(db_path))
            {
                stops = connection.Table<Stop>().ToList();

            }
                return stops;
        }
        
        public static string Read(string db_path, string stopCode)
        {
            List<Stop> stops = new List<Stop>();

            using (var connection = new SQLite.SQLiteConnection(db_path))
            {
                stops = connection.Table<Stop>().ToList();

            }
            return stops.ToString(); ;
        }




    }
}