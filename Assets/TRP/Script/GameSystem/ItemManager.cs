using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{


    static List<int> ItemsIndex => SaveDataManager.CurrengSaveData.ItemIndexs;
    static List<GameItem> showItems => ItemsIndex.Select(index => BagManager.BagItems[index]).ToList();
    public GameObject contentPrefab;
    public GameObject ItemPrefab;
    public static ItemManager Instance;
    public int selectItemIndex = 0;
    public static GameItem SelectItem => BagManager.BagItems[ItemsIndex[Instance.selectItemIndex]];

    private void Awake() => Instance = this;

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
        //创造ui选项
        for (int i = 0; i < 8; i++)
        {
            Instantiate(Instance.ItemPrefab, Instance.contentPrefab.transform);
        }
        //刷新ui
        Refresh();
    }

    public static void Refresh()
    {
        for (int i = 0; i < showItems.Count; i++)
        {
            var targetIcon = Instance.contentPrefab.transform.GetChild(i).GetChild(0).GetComponent<Image>();
            var targetcount = Instance.contentPrefab.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>();
            var targetName = Instance.contentPrefab.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>();
            var selectSign = Instance.contentPrefab.transform.GetChild(i).GetChild(3).gameObject;

            if (showItems[i] != null)
            {
                targetIcon.sprite = BagManager.GetItemIcon(showItems[i].ItemTag);
                targetcount.text = showItems[i].count.ToString();
                targetName.text = showItems[i].showName;
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
