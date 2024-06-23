using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BagManager : MonoBehaviour
{
    public static BagManager Instance { get; set; }
    [Header("背包物体相关")]
    public GameObject bagUI;
    [ShowInInspector]
    public static List<Sprite> itemIcons = new();
    public static List<ItemData> BagItems => SaveDataManager.CurrengSaveData.BagItems;
    public List<GameObject> ItemButtons = new List<GameObject>();
    public GameObject contentPrefab;
    public GameObject ItemButtonPrefab;
    private void Awake() => Instance = this;
    public static bool IsOpen = false;
    public static Sprite GetItemIcon(string itemTag) => itemIcons.FirstOrDefault(icon => icon.name == itemTag);

    //////////////////////////////////拖拽操作相关////////////////////////////////////////
    [Header("拖拽操作相关")]
    public GameObject dragItemPrefab;
    public ItemData dropItemData;
    public RectTransform deleteArea;
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
        LoadIconTex();
        //创造对应背包物体数量的ui
        CreatItemUi();
        //初始化背包预制体
        Refresh();
    }
    static void LoadIconTex()
    {
        itemIcons.Clear();
        var icons = Resources
            .LoadAll<Texture2D>("CubePrefabs\\Thumb")
            .Select(tex => tex.ToSprite())
            .ToList();
        itemIcons.AddRange(icons);
    }
    private static void CreatItemUi()
    {
        int targetCount = Math.Max(60, ((BagItems.Count / 10) + 1) * 10);
        for (int i = Instance.ItemButtons.Count(); i < targetCount; i++)
        {
            var newItem = Instantiate(Instance.ItemButtonPrefab, Instance.contentPrefab.transform);
            Instance.ItemButtons.Add(newItem);
        }
    }
    public static void Refresh()
    {
        for (int i = 0; i < Instance.ItemButtons.Count; i++)
        {
            var targetIcon = Instance.contentPrefab.transform.GetChild(i).GetChild(0).GetComponent<Image>();
            var targetcount = Instance.contentPrefab.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>();
            var targetName = Instance.contentPrefab.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>();
            if (i < BagItems.Count)
            {
                targetIcon.sprite = GetItemIcon(BagItems[i].ItemTag);
                targetcount.text = BagItems[i].Count.ToString();
                targetName.text = BagItems[i].ShowName;
            }
            else
            {
                targetIcon.sprite = null;
                targetcount.text = "";
                targetName.text = "";
                //设为空
            }
        }
    }
    [Button("向背包添加道具")]
    public static void Add(int itemID, int count = 1)
    {
        var targetItem = BagItems.FirstOrDefault(item => item.ItemID == itemID);
        if (targetItem == null)
        {
            BagItems.Add(new ItemData(itemID, count));
        }
        else
        {
            targetItem.Count += count;
        }
        BagManager.Refresh();
        QuickItemManager.Refresh();
    }
    //移除背包道具
    public static void Remove(int itemID)
    {
        BagItems.Remove(BagItems.FirstOrDefault(item => item.ItemID == itemID));
        BagManager.Refresh();
        QuickItemManager.Refresh();
    }
    //////////////////////////////////拖拽相关操作////////////////////////////////////////

    public void ShowDragItem(GameObject itemButton)
    {
        var buttonIndex = BagManager.Instance.ItemButtons.IndexOf(itemButton);
        //点的是无物体格子则跳过
        if (buttonIndex > BagManager.BagItems.Count) { return; }
        dropItemData = BagManager.BagItems[buttonIndex];
        dragItemPrefab.transform.GetChild(0).GetComponent<Image>().sprite = BagManager.GetItemIcon(dropItemData.ItemTag);
        dragItemPrefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = dropItemData.ShowName;
        dragItemPrefab.SetActive(true);
        StartDrag();
    }
    public void StartDrag()
    {
        //Debug.Log("OnDrag");
        Vector3 mousePosition = Input.mousePosition;
        Vector3 targetPos = Camera.main.ScreenToWorldPoint((new Vector3(mousePosition.x, mousePosition.y, 1)));
        dragItemPrefab.transform.position = targetPos;
    }
    public void EndDrag()
    {
        dragItemPrefab.SetActive(false);
        for (int i = 0; i < QuickItemManager.Instance.QuickItem.Count; i++)
        {
            var rect = QuickItemManager.Instance.QuickItem[i].GetComponent<RectTransform>();
            // 判断松开位置是否在快捷栏范围内
            if (RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition, Camera.main))
            {
                int index = Array.IndexOf(QuickItemManager.QuickItemsIDs.ToArray(), dropItemData.ItemID);

                // 如果找到了重复的ItemID
                if (index != -1)
                {
                    // 将该位置的ItemID置为0
                    QuickItemManager.QuickItemsIDs[index] = 0;
                }
                QuickItemManager.QuickItemsIDs[i] = dropItemData.ItemID;

                QuickItemManager.Refresh();
            }
        }
        // 判断松开位置是否在销毁ui范围内
        if (RectTransformUtility.RectangleContainsScreenPoint(deleteArea, Input.mousePosition, Camera.main))
        {
            // 在这里执行你的逻辑，比如打印当前点击的UI信息
            Debug.Log("销毁物体");
            BagManager.Remove(dropItemData.ItemID);
        }
    }
}
