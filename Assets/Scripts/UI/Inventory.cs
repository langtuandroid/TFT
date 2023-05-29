using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public int inventorySpace = 10;

    public Dictionary<int, GameObject> uiItems = new Dictionary<int, GameObject>();
    private Dictionary<int, bool> itemUIStates = new Dictionary<int, bool>(); 

    public bool AddItem(Item item)
    {
        if (items.Count < inventorySpace)
        {
            items.Add(item);
            itemUIStates[item.itemID] = true; 
            if (uiItems.ContainsKey(item.itemID))
            {
                uiItems[item.itemID].SetActive(true); 
            }
            return true;
        }
        else
        {
            Debug.Log("Inventario lleno. No se puede añadir " + item.name + ".");
            return false;
        }
    }

    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            itemUIStates.Remove(item.itemID);
            if (uiItems.ContainsKey(item.itemID))
            {
                uiItems[item.itemID].SetActive(false);
            }
        }
        else
        {
            Debug.Log("No se encontró " + item.name + " en el inventario.");
        }
    }

    public void CheckItemInInventory(int itemID)
    {
        if (itemUIStates.ContainsKey(itemID))
        {
            bool isActive = itemUIStates[itemID];
            if (uiItems.ContainsKey(itemID))
            {
                uiItems[itemID].SetActive(isActive);
            }
        }
    }
}