using Android.Widget;
using System;
using System.Collections.Generic;

public class FavoriteElementId
{
    public ImageButton Btn { get; set; }
    public TextView Txt { get; set; }
    public string Name { get; set; }

    public FavoriteElementId (ImageButton btn,TextView txt, string name) {
        this.Btn = btn;
        this.Txt = txt;
        this.Name = name;
        }
}
