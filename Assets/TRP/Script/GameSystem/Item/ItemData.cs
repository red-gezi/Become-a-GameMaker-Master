using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    //基本信息
    public CubeData CubeData { get; set; }
    public int Count { get; set; }

    //快速索引
    public int ItemID => CubeData.ID;
    public string ItemTag=> CubeData.Name_EN;
    public ItemType ItemType=> CubeData.ItemType;
    public string ShowName=> CubeData.Name_CH;
    public ItemData(int ItemID, int count)
    {
        var cubeData = CubeDataManager.GetCubeData(ItemID);
        this.CubeData = cubeData;
        this.Count = count;
    }
}
