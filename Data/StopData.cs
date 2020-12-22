using SQLite;

namespace trackMe.Data
{
    public class StopData
    {
        [PrimaryKey, AutoIncrement]
        public int stop_code { get; set; }
        public string stop_name { get; set; }
        public string stop_des { get; set; }
    }
}