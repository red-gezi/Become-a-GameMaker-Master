﻿using System;
using System.Collections.Generic;

public class SaveData
{
    //解锁进度数据
    //剧情数据
    //背包数据
    public List<ItemData> BagItems { get; set; } = new();
    //快捷栏数据
    public List<int> QuickItemsIDs { get; set; } = new() { };
    //背包数据
    //电路卡牌数据
    //蓝图数据

    //位置数据
    //存档时间数据
    public DateTime SaveTime { get; set; } = new();
    //存档截图数据
    public string ImgPath { get; set; } = "";
}
