using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickItemManager : MonoBehaviour
{
    public static List<int> QuickItemsIDs => SaveDataManager.CurrengSaveData.QuickItemsIDs;
    //ѡ�п�����ʱ���ؿ���
    public static List<ItemData> QuickItemsData =>
        QuickItemsIDs.Select(ID => BagManager.BagItems.FirstOrDefault(item => item.ItemID == ID) ?? new ItemData(0, 0)).ToList();
    [Header("��ݵ��������")]
    public List<GameObject> QuickItem = new() { };
    public GameObject contentPrefab;
    public GameObject ItemPrefab;
    public static QuickItemManager Instance;
    public static int SelectItemIndex { get; set; } = 0;
    //ѡ�п�����ʱ��������
    public static ItemData SelectItem => QuickItemsData[SelectItemIndex];

    //////////////////////////////////��ק�������////////////////////////////////////////
    [Header("��ק�������")]
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
                break; // һ���ҵ�ƥ��İ�����������ѭ��
            }
        }
    }
    public static void Init()
    {
        //����uiѡ��
        Instance.QuickItem.Add(Instance.ItemPrefab);
        for (int i = 0; i < 8; i++)
        {
            var item = Instantiate(Instance.ItemPrefab, Instance.contentPrefab.transform);
            Instance.QuickItem.Add(item);
        }
        //ˢ��ui
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
    //////////////////////////////////��ק��ز���////////////////////////////////////////

    public void ShowDragItem(GameObject item)
    {
        var itemIndex = QuickItem.IndexOf(item);
        //��������������������
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

        // �ж��ɿ�λ���Ƿ�������ui��Χ��
        if (RectTransformUtility.RectangleContainsScreenPoint(quickItemArea, Input.mousePosition, Camera.main))
        {
            //�ж��ͷ����ĸ����λ�ã����н�����Ȼ��ˢ��
            for (int i = 0; i < QuickItemManager.Instance.QuickItem.Count; i++)
            {
                var rect = QuickItemManager.Instance.QuickItem[i].GetComponent<RectTransform>();
                // �ж��ɿ�λ���Ƿ��ڿ������Χ��
                if (RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition, Camera.main))
                {
                    var temp = QuickItemsIDs[i];
                    QuickItemsIDs[i] = QuickItemsIDs[dropitemIndex];
                    QuickItemsIDs[dropitemIndex] = temp;
                    QuickItemManager.Refresh();
                }
            }
            // ������ִ������߼��������ӡ��ǰ�����UI��Ϣ
            Debug.Log("��������");
            //BagManager.Remove(dropItemData.ItemID);
        }
        else
        {
            //����Ӧλ����Ϊ0
            QuickItemsIDs[dropitemIndex] = 0;
            QuickItemManager.Refresh();
        }
    }
}
