using SQLite;

namespace trackMe.Data
{
    public class FavoriteData
    {
        [PrimaryKey, AutoIncrement]
        public string name { get; set; }
        public string url { get; set; }
    }
}