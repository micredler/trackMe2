using System;
using System.IO;
using Android.Content;
using Android.Database.Sqlite;


namespace trackMe
{
    public class DBHelper : SQLiteOpenHelper
    {
        private static string DB_PATH = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        private static string DB_NAME = "trackMeDB.db";
        private static int VERSION = 1;
        private Context context;
        public DBHelper(Context context) : base(context, DB_NAME, null, VERSION)
        {
            this.context = context;
        }
        private string GetSQLiteDBPath()
        {
            return Path.Combine(DB_PATH, DB_NAME);
        }
        public override SQLiteDatabase WritableDatabase
        {
            get
            {
                return CreateSQLiteDB();
            }
        }
        private SQLiteDatabase CreateSQLiteDB()
        {
            SQLiteDatabase sqliteDB = null;
            string path = GetSQLiteDBPath();
            Stream streamSQLite = null;
            FileStream streamWriter = null;
            Boolean isSQLiteInit = false;
            try
            {
                if (File.Exists(path))
                    isSQLiteInit = true;
                else
                {
                    streamSQLite = context.Resources.OpenRawResource(Resource.Raw.trackMeDB);
                    streamWriter = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                    if (streamSQLite != null && streamWriter != null)
                    {
                        if (CopySQLiteDB(streamSQLite, streamWriter))
                            isSQLiteInit = true;
                    }
                }
                if (isSQLiteInit)
                    sqliteDB = SQLiteDatabase.OpenDatabase(path, null, DatabaseOpenFlags.OpenReadonly);
            }
            catch { }
            return sqliteDB;
        }
        private bool CopySQLiteDB(Stream streamSQLite, FileStream streamWriter)
        {
            bool isSuccess = false;
            int lenght = 256;
            Byte[] buffer = new Byte[lenght];
            try
            {
                int bytesRead = streamSQLite.Read(buffer, 0, lenght);
                while (bytesRead > 0)
                {
                    streamWriter.Write(buffer, 0, bytesRead);
                    bytesRead = streamSQLite.Read(buffer, 0, lenght);
                }
                isSuccess = true;
            }
            catch { }
            finally
            {
                streamSQLite.Close();
                streamWriter.Close();
            }
            return isSuccess;
        }
        public override void OnCreate(SQLiteDatabase db)
        {
        }
        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
        }
    }
}