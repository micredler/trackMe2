using SQLite;

namespace trackMe.Data
{
    public class FavoriteData
    {
        [PrimaryKey, AutoIncrement]
        public string name { get; set; }
        public string url { get; set; }
        public int searchType { get; set; }
        public string direction { get; set; }
    }
}