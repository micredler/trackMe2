using Android.Widget;
using System;
using System.Collections.Generic;

public class FavoriteElementId
{
    public ImageButton Btn { get; set; }
    public TextView Txt { get; set; }
    public string Name { get; set; }
    public int SearchType { get; set; }

    public FavoriteElementId (ImageButton btn,TextView txt, string name, int searchType) {
        this.Btn = btn;
        this.Txt = txt;
        this.Name = name;
        this.SearchType = searchType;
        }
}

    public enum SEARCH_TYPE
    {
        train = 1,
        line = 2,
        station = 3
    }

