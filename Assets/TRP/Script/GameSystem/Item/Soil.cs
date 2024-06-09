class Soil : GameItem
{
    public Soil(int count)
    {
        itemType = ItemType.Cube; 
        itemIcon= ItemIcon.Soil;
        this.count = count;
        showName = "泥土";

    }
}
