class StoneBrick : GameItem
{
    public StoneBrick(int count)
    {
        itemType = ItemType.Cube;
        itemIcon = ItemIcon.StoneBrick;
        this.count = count;
        showName = "石砖";

    }
}