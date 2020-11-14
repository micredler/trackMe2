using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using SQLite;
using trackMe.Data;

namespace trackMe
{  
    public class Database  
    {  
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);  
        public bool createDatabase()  
        {  
            try  
            {  
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "StopStation.db")))  
                {  
                    connection.CreateTable<StopStation>();  
                    return true;  
                }  
            }  
            catch(SQLiteException ex)  
            {  
                Log.Info("SQLiteEx", ex.Message);  
                return false;  
            }  
        }  
         //Add or Insert Operation  
  
        public bool insertIntoTable(StopStation stopStation)  
        {  
            try  
            {  
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "StopStation.db")))  
                {  
                    connection.Insert(stopStation);  
                    return true;  
                }  
            }  
            catch (SQLiteException ex)  
            {  
                Log.Info("SQLiteEx", ex.Message);  
                return false;  
            }  
        }  
        public List<StopStation> selectTable()  
        {  
            try  
            {  
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "StopStation.db")))  
                {  
                    return connection.Table<StopStation>().ToList();  
                }  
            }  
            catch (SQLiteException ex)  
            {  
                Log.Info("SQLiteEx", ex.Message);  
                return null;  
            }  
        }  
         //Edit Operation  
  
        public bool updateTable(StopStation stopStation)  
        {  
            try  
            {  
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "StopStation.db")))  
                {  
                    connection.Query<StopStation>("UPDATE StopStation set stop_name=?, stop_des=?, Where stop_code=?", stopStation.stop_code, stopStation.stop_name, stopStation.stop_des);  
                    return true;  
                }  
            }  
            catch (SQLiteException ex)  
            {  
                Log.Info("SQLiteEx", ex.Message);  
                return false;  
            }  
        }  
         //Delete Data Operation  
  
        public bool removeTable(StopStation stopStation)  
        {  
            try  
            {  
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "StopStation.db")))  
                {  
                    connection.Delete(stopStation);  
                    return true;  
                }  
            }  
            catch (SQLiteException ex)  
            {  
                Log.Info("SQLiteEx", ex.Message);  
                return false;  
            }  
        }  
         //Select Operation  
  
        public bool selectTable(int Id)  
        {  
            try  
            {  
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "StopStation.db")))  
                {  
                    connection.Query<StopStation>("SELECT * FROM StopStation Where Id=?", Id);  
                    return true;  
                }  
            }  
            catch (SQLiteException ex)  
            {  
                Log.Info("SQLiteEx", ex.Message);  
                return false;  
            }  
        }  
    }  
}  