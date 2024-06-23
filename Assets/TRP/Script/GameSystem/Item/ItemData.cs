using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    //������Ϣ
    public CubeData CubeData { get; set; }
    public int Count { get; set; }

    //��������
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
