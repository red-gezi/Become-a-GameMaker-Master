using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BagManager : MonoBehaviour
{
    public List<Sprite> itemIcon;
    public static BagManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public static Sprite GetItemIcon(ItemIcon itemIcon)
    {
        return Instance.itemIcon.FirstOrDefault(icon => icon.name == itemIcon.ToString());
    }
}
