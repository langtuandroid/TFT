using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{
    public List<Item> items = new List<Item>();  //TODO tengo dudas de si viene de una clase
    public int inventorySpace = 10;  //TODO no se si hace falta
    
    public bool AddItem(Item item)
    {
        if (items.Count < inventorySpace) //TODO añadir comprobacion de si ya lo tiene?
        {
            items.Add(item); // TODO hay que hacer un setActive true
            return true;
        }
        else
        {
            Debug.Log("Inventario lleno. No se puede añadir " + item.name + "."); //TODO Quitar
            return false;
        }
    }

    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item); // TODO hay que hacer un setActive false
        }
        else
        {
            Debug.Log("No se encontró " + item.name + " en el inventario.");//TODO Quitar
        }
    }
}

//TODO añado el item para pruebas
[System.Serializable]
public class Item
{
    public string name; 

    public Item(string itemName)
    {
        name = itemName;
    }
}