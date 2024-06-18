using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem
{
    public CubeData cubeData;

    public int ItemID => cubeData.ID;
    public string ItemTag=> cubeData.Name_EN;
    public ItemType itemType=> cubeData.ItemType;
    public int count;
    public string showName=> cubeData.Name_CH;
    public GameItem(int ItemID, int count)
    {
        var cubeData = CubeDataManager.GetCubeData(ItemID);
        this.cubeData = cubeData;
        this.count = count;
    }
}
