using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    List<GameItem> gameItems = new List<GameItem>();
    public GameObject contentPrefab;
    public GameObject ItemPrefab;
    public static ItemManager Instance;
    public int selectItemIndex = 0;
    public static GameItem SelectItem => Instance.gameItems[Instance.selectItemIndex];
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Keypad1 + (i - 1)) || Input.GetKeyDown(KeyCode.Alpha1 + (i - 1)))
            {
                selectItemIndex = i - 1;
                Refresh();
                break; // 一旦找到匹配的按键，就跳出循环
            }
        }
    }
    public static void Init(List<GameItem> Items)
    {
        for (int i = 0; i < 8; i++)
        {
            Instantiate(Instance.ItemPrefab, Instance.contentPrefab.transform);
        }
        Instance.gameItems = Items;
        Refresh();
    }
    public static void Refresh()
    {
        for (int i = 0; i < Instance.gameItems.Count; i++)
        {
            var targetIcon = Instance.contentPrefab.transform.GetChild(i).GetChild(0).GetComponent<Image>();
            var targetcount = Instance.contentPrefab.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>();
            var targetName = Instance.contentPrefab.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>();
            var selectSign = Instance.contentPrefab.transform.GetChild(i).GetChild(3).gameObject;

            if (Instance.gameItems[i] != null)
            {
                targetIcon.sprite = BagManager.GetItemIcon(Instance.gameItems[i].itemIcon);
                targetcount.text = Instance.gameItems[i].count.ToString();
                targetName.text = Instance.gameItems[i].showName;
            }
            else
            {
                targetIcon.sprite = null;
                targetcount.text = "";
                targetName.text = "";
            }
            selectSign.SetActive(Instance.selectItemIndex == i);
        }
    }
}
