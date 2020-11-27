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

        internal string GetRouteIdFromDB(string lineNumFromUser, string agencySelected)
        {
            SQLiteConnection connection = dbConnection.CreateConnection();
            List<Route> routeId = connection.Query<Route>($"SELECT * FROM routes WHERE route_short_name = {lineNumFromUser}");

            return routeId.Where(r => r.route_type == Int32.Parse(agencySelected)).First().route_id.ToString();
        }

        public string[] GetAllTrainStopsName()
        {
            SQLiteConnection connection = dbConnection.CreateConnection();
            List<TrainStop> trainStops = connection.Query<TrainStop>($"SELECT stop_merged FROM train_stops");

            return trainStops.Select(s => s.stop_merged).OfType<string>().ToArray();
        }
        
        public string[] GetAllAgency()
        {
            SQLiteConnection connection = dbConnection.CreateConnection();
            List<Agency> agencyNames = connection.Query<Agency>($"SELECT agency_name FROM agency");

            return agencyNames.Select(s => s.agency_name).OfType<string>().ToArray();
        }

        public int GetTrainStationNumberByName(string trainStationName)
        {
            SQLiteConnection connection = dbConnection.CreateConnection();
            TrainStop trainStops = connection.Query<TrainStop>($"SELECT * FROM train_stops WHERE stop_merged LIKE '%{trainStationName}%'").First();

            return trainStops.stop_code;
        }
    }
}