using System;
using System.IO;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    public static SaveData CurrengSaveData { get; set; }
    //创造一个初始存档
    public static void CreatNewData()
    {
        CurrengSaveData = new();
        CurrengSaveData.QuickItemsIDs = new() {0,0,0,0,0,0,0,0,0 };
        CurrengSaveData.BagItems.Add(new ItemData(1, 6));
        CurrengSaveData.BagItems.Add(new ItemData(2, 3));
        CurrengSaveData.BagItems.Add(new ItemData(3, 3));
        CurrengSaveData.BagItems.Add(new ItemData(4, 3));
    }
    public static void Save(int index)
    {
        CurrengSaveData.SaveTime = DateTime.Now;
        File.WriteAllText(Application.dataPath + $"//Save{index}.json", CurrengSaveData.ToJson());
    }

    public static void Load(int index)
    {
        CurrengSaveData = File.ReadAllText(Application.dataPath + $"//Save{index}.json").ToObject<SaveData>();
    }

}