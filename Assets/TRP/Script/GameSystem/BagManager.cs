using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class BagManager : MonoBehaviour
{
    public static BagManager Instance;
    public GameObject bagUI;
    [ShowInInspector]
    public static List<Sprite> itemIcons = new();
    public static List<GameItem> BagItems=> SaveDataManager.CurrengSaveData.BagItems;
    private void Awake() => Instance = this;
    public static bool IsOpen = false;
    public static Sprite GetItemIcon(string itemTag) => itemIcons.FirstOrDefault(icon => icon.name == itemTag);

    public void ChangeState()
    {
        if (IsOpen)
        {
            //关闭背包
            bagUI.SetActive(false);
        }
        else
        {
            //打开背包
            bagUI.SetActive(true);
        }
        IsOpen = !IsOpen;
    }
    public static void Init()
    {
        //载入图标
        LoadIcon();
        Refresh();
    }
    static void LoadIcon()
    {
        itemIcons.Clear();
        var icons = Resources
            .LoadAll<Texture2D>("CubePrefabs\\Thumb")
            .Select(tex => tex.ToSprite())
            .ToList();
        itemIcons.AddRange(icons);
    }
    public static void Add()
    {

    }
    public static void Remove()
    {

    }
    public static void Sort()
    {

    }
    //刷新背包物体
    public static void Refresh()
    {
        //for (int i = 0; i < Instance.gameItems.Count; i++)
        //{
        //    var targetIcon = Instance.contentPrefab.transform.GetChild(i).GetChild(0).GetComponent<Image>();
        //    var targetcount = Instance.contentPrefab.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>();
        //    var targetName = Instance.contentPrefab.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>();
        //    var selectSign = Instance.contentPrefab.transform.GetChild(i).GetChild(3).gameObject;

        //    if (Instance.gameItems[i] != null)
        //    {
        //        targetIcon.sprite = GetItemIcon(Instance.gameItems[i].ItemTag);
        //        targetcount.text = Instance.gameItems[i].count.ToString();
        //        targetName.text = Instance.gameItems[i].showName;
        //    }
        //    else
        //    {
        //        targetIcon.sprite = null;
        //        targetcount.text = "";
        //        targetName.text = "";
        //    }
        //    selectSign.SetActive(Instance.selectItemIndex == i);
        //}
    }
}
