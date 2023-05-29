using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public int inventorySpace = 10;

    private Dictionary<int, GameObject> uiItems = new Dictionary<int, GameObject>();
    private readonly Dictionary<int, bool> itemUIStates = new Dictionary<int, bool>();

    private List<int> itemsNoEquipables = new List<int>();

    private void Awake() //TODO quitar solo es de prueba
    {
        Item florAmarillenta = new Item("Flor Amarillenta", 1);
        AddItem(florAmarillenta);
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        foreach (Item item in items) // Busqueda de items no equipables en la ui para añadirlas al diccionario
        {
            // TODO preguntar por este prefijo u otra forma de hacerlo
            Transform uiItemTransform = transform.Find("UI_ITEM_" + item.itemID);
            
            if (uiItemTransform != null)
            {
                GameObject uiItem = uiItemTransform.gameObject;
                
                if (uiItem != null)
                {
                    uiItems[item.itemID] = uiItem; // ID DE LA UI
                    itemUIStates[item.itemID] = true;
                    itemsNoEquipables.Add(item.itemID);
                    CheckItemInInventory(item.itemID);
                }
            }
        }
    }

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

    private void CheckItemInInventory(int itemID)
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