using SQLite;

namespace trackMe.Data
{
    public class Agency
    {
        [PrimaryKey, AutoIncrement]
        public int agency_id { get; set; }
        public string agency_name { get; set; }
        public string agency_url { get; set; }
    }
}