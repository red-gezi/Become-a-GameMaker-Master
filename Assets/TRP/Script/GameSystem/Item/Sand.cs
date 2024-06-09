class Sand : GameItem
{
    public Sand(int count)
    {
        itemType = ItemType.Cube;
        itemIcon = ItemIcon.Sand;
        this.count = count;
        showName = "沙子";
    }
}
