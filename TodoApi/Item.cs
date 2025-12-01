using System;
using System.Collections.Generic;

namespace TodoApi;

public partial class Item
{
    
    public Item()
    {
    }

    
    public Item(string name)
    {
        NameI = name;
        IsComplete = false; 
    }

    public int Id { get; set; }

    public string? NameI { get; set; }

    public bool? IsComplete { get; set; }
}