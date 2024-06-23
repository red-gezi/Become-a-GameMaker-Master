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
public class GameManager : MonoBehaviour
{
    void Start()
    {
        Creat();
        //Load(0);
    }
    //初始化游戏
    private static void Init()
    {
        //方块数据加载
        CubeDataManager.Init();
        //背包初始化
        BagManager.Init();
        //快捷栏初始化
        QuickItemManager.Init();
        //地图初始化
        WorldManager.Init();
        //人物初始化
        //摄像机初始化
        //UI初始化
    }
    [Button("新建存档")]
    public void Creat()
    {
        SaveDataManager.CreatNewData();
        GameManager.Init();
    }
    [Button("储存存档")]
    public void Save(int index)
    {
        SaveDataManager.Save(index);
    }
    [Button("加载存档")]
    public void Load(int index)
    {
        SaveDataManager.Load(index);
        GameManager.Init();
    }
}
