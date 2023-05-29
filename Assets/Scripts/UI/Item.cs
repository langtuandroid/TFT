[System.Serializable]
public class Item
{
    public string name;
    public int itemID;

    public Item(string itemName, int id)
    {
        name = itemName;
        itemID = id;
    }
}