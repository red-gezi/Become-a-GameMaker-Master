using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Load(0);
    }
    private static void Init()
    {
        //UI初始化
        
        //背包初始化
        BagManager.Init();
        //快捷栏初始化
        ItemManager.Init();
        //ItemManager.Init(new List<GameItem>()
        //{
        //    new GameItem(1,18),
        //    new GameItem(2,18),
        //    new GameItem(3,18),
        //    new GameItem(4,18),
        //    null,
        //    null,
        //    null,
        //    null,
        //    null,
        //});
        //地图初始化
        WorldManager.Init();
        //人物初始化
    }
    [Button("创造存档")]
    public void Save(int index)
    {
        SaveDataManager.Save(index);
    }
    public void Load(int index)
    {
        SaveDataManager.Load(index);
        Init();
        //初始化游戏
    }
}
