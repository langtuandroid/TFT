using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/ItemSO")]
[System.Serializable]
public class Item : ScriptableObject
{
    public string Name;
    public int itemID;
    public Sprite Sprite;
    public string Description;
    public bool Unlocked;

    public Item(string itemName, Sprite itemSprite, string itemDescription)
    {
        Name = itemName;
        Sprite = itemSprite;
        Description = itemDescription;
    }

}