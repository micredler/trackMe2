using SQLite;

namespace trackMe.Data
{
    public class StopStation
    {
        [PrimaryKey, AutoIncrement]
        public int stop_code { get; set; }
        public string stop_name { get; set; }
        public string stop_des { get; set; }
    }
}