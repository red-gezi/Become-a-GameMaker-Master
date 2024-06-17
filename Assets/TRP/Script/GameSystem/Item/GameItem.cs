using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem
{
    public string ItemTag;
    public ItemType itemType;
    public int count;
    public string showName;
    public GameItem(string itemName ,int count)
    {
        itemName = itemName;
        this.count = count;
        itemType = ItemType.Cube;
       
        showName = "´«ËÍÃÅ";
    }
}
