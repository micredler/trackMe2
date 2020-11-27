using System;
using System.Collections.Generic;
using System.Linq;

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

        internal string GetRouteIdFromDB(string lineNumFromUser)
        {
            SQLiteConnection connection = dbConnection.CreateConnection();
            var routeId = connection.Query<Route>($"SELECT * FROM routes WHERE route_short_name = {lineNumFromUser}");

            try
            {
                // Now it return the first  Egged route but we need to add suport agency in client
                return routeId.Where(r => r.route_type == 3).First().route_id.ToString();
                
            }

            catch (Exception)
            {
                return "מספר הקו לא מופיע במערכת";
            }
        }

        public string[] GetAllTrainStopsName()
        {
            SQLiteConnection connection = dbConnection.CreateConnection();
            List<TrainStop> trainStops = connection.Query<TrainStop>($"SELECT stop_merged FROM train_stops");

            return trainStops.Select(s => s.stop_merged).OfType<string>().ToArray();
        }
    }
}