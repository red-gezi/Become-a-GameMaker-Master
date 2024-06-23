using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickItemManager : MonoBehaviour
{
    public static List<int> QuickItemsIDs => SaveDataManager.CurrengSaveData.QuickItemsIDs;
    //选中空物体时返回空气
    public static List<ItemData> QuickItemsData =>
        QuickItemsIDs.Select(ID => BagManager.BagItems.FirstOrDefault(item => item.ItemID == ID) ?? new ItemData(0, 0)).ToList();
    [Header("快捷道具栏相关")]
    public List<GameObject> QuickItem = new() { };
    public GameObject contentPrefab;
    public GameObject ItemPrefab;
    public static QuickItemManager Instance;
    public static int SelectItemIndex { get; set; } = 0;
    //选中空物体时返回虚无
    public static ItemData SelectItem => QuickItemsData[SelectItemIndex];

    //////////////////////////////////拖拽操作相关////////////////////////////////////////
    [Header("拖拽操作相关")]
    public GameObject dragItemPrefab;
    public int dropitemIndex;
    public RectTransform quickItemArea;

    private void Awake() => Instance = this;

    private void Update()
    {
        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Keypad1 + (i - 1)) || Input.GetKeyDown(KeyCode.Alpha1 + (i - 1)))
            {
                SelectItemIndex = i - 1;
                Refresh();
                break; // 一旦找到匹配的按键，就跳出循环
            }
        }
    }
    public static void Init()
    {
        //创造ui选项
        Instance.QuickItem.Add(Instance.ItemPrefab);
        for (int i = 0; i < 8; i++)
        {
            var item = Instantiate(Instance.ItemPrefab, Instance.contentPrefab.transform);
            Instance.QuickItem.Add(item);
        }
        //刷新ui
        Refresh();
    }

    public static void Refresh()
    {
        for (int i = 0; i < QuickItemsData.Count; i++)
        {
            var targetIcon = Instance.contentPrefab.transform.GetChild(i).GetChild(0).GetComponent<Image>();
            var targetcount = Instance.contentPrefab.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>();
            var targetName = Instance.contentPrefab.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>();
            var selectSign = Instance.contentPrefab.transform.GetChild(i).GetChild(3).gameObject;

            if (QuickItemsData[i] != null)
            {
                targetIcon.sprite = BagManager.GetItemIcon(QuickItemsData[i].ItemTag);
                targetcount.text = QuickItemsData[i].Count.ToString();
                targetName.text = QuickItemsData[i].ShowName;
            }
            else
            {
                targetIcon.sprite = null;
                targetcount.text = "";
                targetName.text = "";
            }
            selectSign.SetActive(SelectItemIndex == i);
        }
    }
    public void ClickItem(GameObject item)
    {
        var itemIndex = QuickItem.IndexOf(item);
        SelectItemIndex = itemIndex;
        Refresh();
    }
    //////////////////////////////////拖拽相关操作////////////////////////////////////////

    public void ShowDragItem(GameObject item)
    {
        var itemIndex = QuickItem.IndexOf(item);
        //点的是无物体格子则跳过
        if (QuickItemsIDs[itemIndex] == 0) { return; }
        dropitemIndex = itemIndex;
        dragItemPrefab.transform.GetChild(0).GetComponent<Image>().sprite = BagManager.GetItemIcon(QuickItemsData[itemIndex].ItemTag);
        dragItemPrefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = QuickItemsData[itemIndex].ShowName;
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

        // 判断松开位置是否在销毁ui范围内
        if (RectTransformUtility.RectangleContainsScreenPoint(quickItemArea, Input.mousePosition, Camera.main))
        {
            //判断释放在哪个快捷位置，进行交换，然后刷新
            for (int i = 0; i < QuickItemManager.Instance.QuickItem.Count; i++)
            {
                var rect = QuickItemManager.Instance.QuickItem[i].GetComponent<RectTransform>();
                // 判断松开位置是否在快捷栏范围内
                if (RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition, Camera.main))
                {
                    var temp = QuickItemsIDs[i];
                    QuickItemsIDs[i] = QuickItemsIDs[dropitemIndex];
                    QuickItemsIDs[dropitemIndex] = temp;
                    QuickItemManager.Refresh();
                }
            }
            // 在这里执行你的逻辑，比如打印当前点击的UI信息
            Debug.Log("销毁物体");
            //BagManager.Remove(dropItemData.ItemID);
        }
        else
        {
            //将对应位置设为0
            QuickItemsIDs[dropitemIndex] = 0;
            QuickItemManager.Refresh();
        }
    }
}
