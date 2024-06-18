using System;
using System.Collections.Generic;

public class SaveData
{
    //解锁进度数据
    //剧情数据
    //背包数据
    public List<GameItem> BagItems { get; set; } = new();
    //快捷栏数据
    //背包数据
    //电路卡牌数据
    //蓝图数据
    public List<int> ItemIndexs { get; set; } = new() { -1,-1,-1,-1,-1,-1,-1,-1,-1};
    //位置数据
    //存档时间数据
    public DateTime SaveTime { get; set; } = new();
    //存档截图数据
    public string ImgPath { get; set; } = "";
}
