using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using SQLite;
using trackMe.BL;

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
            try
            {
                SQLiteConnection connection = dbConnection.CreateConnection();
                var stops = connection.Query<StopData>($"SELECT stop_name FROM stops WHERE stop_code = {stopCode}");

                return stops[0].stop_name.ToString();
            }

            catch (Exception)
            {
                return null;
            }
        }

        internal string GetRouteIdFromDB(string lineNumFromUser, string agencySelected)
        {
            try
            {
                int agencyNumber = GetAgencyNumberByName(agencySelected);

                SQLiteConnection connection = dbConnection.CreateConnection();
                List<RouteData> routeId = connection.Query<RouteData>($"SELECT * FROM routes WHERE route_short_name = {lineNumFromUser} and agency_id = {agencyNumber}");

                return routeId.First().route_id.ToString();
            }

            catch (Exception)
            {
                return null;
            }
        }

        public string[] GetAllTrainStopsName()
        {
            try
            {
                SQLiteConnection connection = dbConnection.CreateConnection();
                List<TrainStopData> trainStops = connection.Query<TrainStopData>($"SELECT stop_merged FROM train_stops");

                return trainStops.Select(s => s.stop_merged).OfType<string>().ToArray();
            }

            catch (Exception)
            {
                return null;
            }
        }

        public string[] GetAllAgencies()
        {
            try
            {
                SQLiteConnection connection = dbConnection.CreateConnection();
                List<Agency> agencyNames = connection.Query<Agency>($"SELECT agency_name FROM agency");

                return agencyNames.Select(s => s.agency_name).OfType<string>().ToArray();
            }

            catch (Exception)
            {
                return null;
            }
        }

        public int GetTrainStationNumberByName(string trainStationName)
        {
            try
            {
                SQLiteConnection connection = dbConnection.CreateConnection();
                TrainStopData trainStops = connection.Query<TrainStopData>($"SELECT * FROM train_stops WHERE stop_merged LIKE '%{trainStationName}%'").First();
                return trainStops.stop_code;
            }

            catch (Exception)
            {
                return 0;
            }

        }

        public int GetAgencyNumberByName(string agencyName)
        {
            try
            {
                SQLiteConnection connection = dbConnection.CreateConnection();
                var agency = connection.Query<Agency>($"SELECT * FROM agency WHERE agency_name LIKE '%{agencyName}%'").First();
                return agency.agency_id;
            }

            catch (Exception)
            {
                return 0;
            }

        }
        public void AddNewFavorite(Activity activity, string name, string url, int searchType)
        {
            try
            {
                SQLiteConnection connection = dbConnection.CreateConnection();
                connection.Query<FavoriteData>($"INSERT INTO favorites (name, url, searchType) VALUES('{name}', '{url}', '{searchType}')");
            }

            catch (Exception)
            {
                Alert.AlertMessage(activity, "הודעת מערכת", "המועדף לא נוסף במערכת");
                return;
            }
        }
        public void DeleteFavorite(Activity activity, string name)
        {
            try
            {
                SQLiteConnection connection = dbConnection.CreateConnection();
                connection.Query<FavoriteData>($"Delete from favorites where name = '{name}'");
            }

            catch (Exception)
            {
                Alert.AlertMessage(activity, "הודעת מערכת", "המועדף לא נוסף במערכת");
                return;
            }
        }

        public string GetUrlFavoriteByName(string name)
        {
            try
            {
                SQLiteConnection connection = dbConnection.CreateConnection();
                FavoriteData favorite = connection.Query<FavoriteData>($"SELECT * FROM favorites WHERE name LIKE '%{name}%'").First();
                return favorite.url;
            }

            catch (Exception)
            {
                return null;
            }

        }


        public List<FavoriteData> GetFavorites()
        {
            try
            {
                SQLiteConnection connection = dbConnection.CreateConnection();
                List<FavoriteData> favorites = connection.Query<FavoriteData>($"SELECT * FROM favorites").ToList<FavoriteData>();
                return favorites;
            }

            catch (Exception)
            {
                return null;
            }

        }
        public List<RouteData> GetDirections(string lineNumber, string operatorName)
        {
            try
            {
                SQLiteConnection connection = dbConnection.CreateConnection();
                var response = connection.Query<RouteData>
                    ($"SELECT route_id, destination FROM routes WHERE route_short_name like '{lineNumber}' and agency_id like '{operatorName}' GROUP BY destination").ToList();

                    // next line to get result for test
                    // ($"SELECT route_id, destination FROM routes WHERE route_short_name like '{lineNumber}' GROUP BY destination").ToList();
                return response;
            }

            catch (Exception)
            {
                return null;
            }
        }
    }
}