using Android.Widget;
using System;
using System.Collections.Generic;

public class FavoriteElementId
{
    public ImageButton Btn { get; set; }
    public TextView Txt { get; set; }
    public string Name { get; set; }
    public int SearchType { get; set; }

    public FavoriteElementId(ImageButton btn, TextView txt, string name, int searchType)
    {
        Btn = btn;
        Txt = txt;
        Name = name;
        SearchType = searchType;
    }
}
public class FavoriteLineElementId : FavoriteElementId
{
    public string Direction { get; set; }

    public FavoriteLineElementId(ImageButton btn, TextView txt, string name, int searchType, string direction)
        : base(btn, txt, name, searchType)
    {
        Btn = btn;
        Txt = txt;
        Name = name;
        SearchType = searchType;
        Direction = direction;
    }
}

public enum SEARCH_TYPE
{
    train = 1,
    line = 2,
    station = 3
}

