using System;
using System.Collections.Generic;
using System.IO;
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
    public class DBConnection
    {
        string dbName = "trackMeDB.db";
        string folderpath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        public DBConnection()
        {
        }

        public SQLiteConnection CreateConnection()
        {
            string dbPath = Path.Combine(folderpath, dbName);

            if (!File.Exists(dbPath))
            {
                using (var binaryReader = new BinaryReader(Application.Context.Assets.Open(dbName)))
                {
                    using (var binaryWriter = new BinaryWriter(new FileStream(dbPath, FileMode.Create)))
                    {
                        byte[] buffer = new byte[2048];
                        int length = 0;
                        while ((length = binaryReader.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            binaryWriter.Write(buffer, 0, length);
                        }
                    }
                }
            }

            var connection = new SQLiteConnection(dbPath);

            return connection;
        }
    }
}