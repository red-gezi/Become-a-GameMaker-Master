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
    [Header("�����������")]
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

    //////////////////////////////////��ק�������////////////////////////////////////////
    [Header("��ק�������")]
    public GameObject dragItemPrefab;
    public ItemData dropItemData;
    public RectTransform deleteArea;
    public void ChangeState()
    {
        if (IsOpen)
        {
            //�رձ���
            bagUI.SetActive(false);
        }
        else
        {
            //�򿪱���
            bagUI.SetActive(true);
        }
        IsOpen = !IsOpen;
    }
    public static void Init()
    {
        //����ͼ��
        LoadIconTex();
        //�����Ӧ��������������ui
        CreatItemUi();
        //��ʼ������Ԥ����
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
                //��Ϊ��
            }
        }
    }
    [Button("�򱳰���ӵ���")]
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
    //�Ƴ���������
    public static void Remove(int itemID)
    {
        BagItems.Remove(BagItems.FirstOrDefault(item => item.ItemID == itemID));
        BagManager.Refresh();
        QuickItemManager.Refresh();
    }
    //////////////////////////////////��ק��ز���////////////////////////////////////////

    public void ShowDragItem(GameObject itemButton)
    {
        var buttonIndex = BagManager.Instance.ItemButtons.IndexOf(itemButton);
        //��������������������
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
            // �ж��ɿ�λ���Ƿ��ڿ������Χ��
            if (RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition, Camera.main))
            {
                int index = Array.IndexOf(QuickItemManager.QuickItemsIDs.ToArray(), dropItemData.ItemID);

                // ����ҵ����ظ���ItemID
                if (index != -1)
                {
                    // ����λ�õ�ItemID��Ϊ0
                    QuickItemManager.QuickItemsIDs[index] = 0;
                }
                QuickItemManager.QuickItemsIDs[i] = dropItemData.ItemID;

                QuickItemManager.Refresh();
            }
        }
        // �ж��ɿ�λ���Ƿ�������ui��Χ��
        if (RectTransformUtility.RectangleContainsScreenPoint(deleteArea, Input.mousePosition, Camera.main))
        {
            // ������ִ������߼��������ӡ��ǰ�����UI��Ϣ
            Debug.Log("��������");
            BagManager.Remove(dropItemData.ItemID);
        }
    }
}
