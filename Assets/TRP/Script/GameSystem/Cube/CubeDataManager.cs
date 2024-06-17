using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CubeDataManager:MonoBehaviour
{
    static List<CubeData> cubeData = new();
    public static Dictionary<string, CubeData> cubeDataDict = new();
    public static void LoadCubeData()
    {
        cubeData.Clear();
        string filePath = @"\Assets\TRP\Resources\CubeData.csv"; // 指定CSV文件的路径
        var datas = File.ReadAllLines(filePath);

        for (int i = 1; i < datas.Length; i++)
        {
            string[] values = datas[i].Split(',');
            if (values.Length >= 7)
            {
                CubeData info = new CubeData
                {
                    Tag = values[0],
                    NameCH = values[1],
                    Describe = values[2],
                    Durability = int.Parse(values[3]),
                    Type = (ItemType)int.Parse(values[4]),
                    CSE = values[5],
                    DSE = values[6]
                };
                cubeData.Add(info);
            }
        }
        cubeDataDict = cubeData.ToDictionary(data => data.Tag);
    }
    public static void UpdatePrefabsTexture()
    {
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
    }

    internal static void CreatNewPrefab()
    {
        throw new NotImplementedException();
    }
}