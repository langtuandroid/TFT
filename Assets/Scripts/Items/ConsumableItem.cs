using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ConsumableItem", menuName = "Item/ConsumableItemSO")]
[Serializable]
public class ConsumableItem : Item
{
    public int Quantity;

    public ConsumableItem(string itemName, Sprite itemSprite, string itemDescription) : base(itemName, itemSprite, itemDescription)
    {
        Quantity = 0;
    }

    public ConsumableItem(string itemName, Sprite itemSprite, string itemDescription, int itemQuantity) : base(itemName, itemSprite, itemDescription)
    {
        Quantity = itemQuantity;
    }

}


