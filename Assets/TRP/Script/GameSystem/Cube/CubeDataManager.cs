using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CubeDataManager : MonoBehaviour
{
    static List<CubeData> cubeData = new();
    public static Dictionary<int, CubeData> cubeDataDict = new();
    public static void LoadCubeData()
    {
        cubeData.Clear();
        string filePath = @"Assets\TRP\Resources\CubeData.csv"; // 指定CSV文件的路径
        var datas = File.ReadAllLines(filePath);

        var titles = datas[0].Split(',').ToList();
        for (int i = 1; i < datas.Length; i++)
        {
            string[] values = datas[i].Split(',');
            if (values.Length >= 7)
            {
                CubeData info = new CubeData
                {
                    ID = int.Parse(values[titles.IndexOf("ID")]),
                    Name_CH = values[titles.IndexOf("Name_CH")],
                    Name_EN = values[titles.IndexOf("Name_EN")],
                    Describe = values[titles.IndexOf("Describe")],
                    Durability = int.Parse(values[titles.IndexOf("Durability")]),
                    ItemType = (ItemType)int.Parse(values[titles.IndexOf("Type")]),
                    CSE = values[titles.IndexOf("CSE")],
                    DSE = values[titles.IndexOf("DSE")]
                };
                cubeData.Add(info);
            }
        }
        cubeDataDict = cubeData.ToDictionary(data => data.ID);
    }
    public static CubeData GetCubeData(int ID)
    {
        if (!cubeDataDict.Any())
        {
            LoadCubeData();
        }
        if (cubeDataDict.ContainsKey(ID))
        {
            return cubeDataDict[ID];
        }
        else
        {
            Debug.LogError("无法找到指定方块:" + ID);
            return null;
        }
    }
    public static void UpdatePrefabsTexture()
    {
#if UNITY_EDITOR
        string thumbPath = Directory.GetCurrentDirectory() + $"\\Assets\\TRP\\Resources\\CubePrefab\\Thumb";
        Directory.CreateDirectory(thumbPath);
        var prefabs = Resources.LoadAll<GameObject>("CubePrefab").ToList();
        Debug.Log("预制体数量" + prefabs.Count);
        prefabs.ForEach(prefab =>
        {
            Texture2D Tex = AssetPreview.GetAssetPreview(prefab);
            if (Tex != null)
            {
                byte[] bytes = Tex.EncodeToPNG();
                File.WriteAllBytes($"{thumbPath}\\{prefab.name}.png", bytes);
            }
            else
            {
                Debug.LogError("缩略图生成失败");
            }
        });
#endif
    }

    internal static void CreatNewPrefab()
    {
        throw new NotImplementedException();
    }
}