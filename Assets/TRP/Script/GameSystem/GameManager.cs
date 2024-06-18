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
        //UI��ʼ��
        
        //������ʼ��
        BagManager.Init();
        //�������ʼ��
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
        //��ͼ��ʼ��
        WorldManager.Init();
        //�����ʼ��
    }
    [Button("����浵")]
    public void Save(int index)
    {
        SaveDataManager.Save(index);
    }
    public void Load(int index)
    {
        SaveDataManager.Load(index);
        Init();
        //��ʼ����Ϸ
    }
}
