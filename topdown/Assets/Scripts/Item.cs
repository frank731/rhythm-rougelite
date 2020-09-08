using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    public int itemId;
    public string displayName;
    public string description;
    public Action function;
    public Item(int itemId, string displayName, string description, Action function)
    {
        this.itemId = itemId;
        this.displayName = displayName;
        this.description = description;
        this.function = function;
    }
}
