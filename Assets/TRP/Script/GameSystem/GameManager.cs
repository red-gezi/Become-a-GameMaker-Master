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
    //��ʼ����Ϸ
    private static void Init()
    {
        //�������ݼ���
        CubeDataManager.Init();
        //������ʼ��
        BagManager.Init();
        //�������ʼ��
        QuickItemManager.Init();
        //��ͼ��ʼ��
        WorldManager.Init();
        //�����ʼ��
        //�������ʼ��
        //UI��ʼ��
    }
    [Button("�½��浵")]
    public void Creat()
    {
        SaveDataManager.CreatNewData();
        GameManager.Init();
    }
    [Button("����浵")]
    public void Save(int index)
    {
        SaveDataManager.Save(index);
    }
    [Button("���ش浵")]
    public void Load(int index)
    {
        SaveDataManager.Load(index);
        GameManager.Init();
    }
}
