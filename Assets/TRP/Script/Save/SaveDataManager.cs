using System;
using System.IO;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    public static SaveData CurrengSaveData { get; set; } = new();
    public static void Save(int index)
    {
        CurrengSaveData.SaveTime = DateTime.Now;
        File.WriteAllText(Application.persistentDataPath + $"//Save{index}.json", CurrengSaveData.ToJson());
    }

    public static void Load(int index)
    {
        CurrengSaveData = File.ReadAllText(Application.persistentDataPath + $"//Save{index}.json").ToObject<SaveData>();
    }

}