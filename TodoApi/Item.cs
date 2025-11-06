using System;
using System.Collections.Generic;

namespace TodoApi;

public partial class Item
{
    // Constructor ריק בשביל Entity Framework
    public Item()
    {
    }

    // Constructor עם שם (אופציונלי)
    public Item(string name)
    {
        NameI = name;
        IsComplete = false; // ברירת מחדל
    }

    public int Id { get; set; }

    public string? NameI { get; set; }

    public bool? IsComplete { get; set; }
}