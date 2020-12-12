using SQLite;

namespace trackMe.Data
{
    public class Route
    {
        [PrimaryKey, AutoIncrement]
        public int route_id { get; set; }
        public int agency_id { get; set; }
        public string start_from { get; set; }
        public string destination { get; set; }
        public string route_desc { get; set; }
        public int route_type { get; set; }
        public string route_color { get; set; }
    }
}