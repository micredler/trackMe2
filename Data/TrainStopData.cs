using SQLite;

namespace trackMe.Data
{
    public class TrainStopData
    {
        [PrimaryKey, AutoIncrement]
        public int stop_code { get; set; }
        public string stop_name { get; set; }
        public string stop_city { get; set; }
        public string stop_merged { get; set; }
    }
}