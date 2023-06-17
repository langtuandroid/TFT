using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    #region SerializeFields

    [SerializeField]
    [Tooltip("Información del objeto seleccionado")]
    private TMP_Text _textInfo;

    [SerializeField]
    [Tooltip("Objecto \"vacío\"")]
    private Item _empty;

    #endregion


    #region Public variables

    public List<Item> items = new List<Item>();
    public List<Item> nonEquipableList;
    public List<Image> nonEquipableButtons;
    public List<Item> powerList;
    public List<ConsumableItem> consumableList;
    public int inventorySpace = 10;

    #endregion

    #region Private variables




    private Dictionary<int, GameObject> uiItems = new Dictionary<int, GameObject>();
    private readonly Dictionary<int, bool> itemUIStates = new Dictionary<int, bool>();

    private List<int> itemsNoEquipables = new List<int>();

    #endregion

    #region Unity methods

    private void Awake() //TODO quitar solo es de prueba
    {
        Item yellowFlower = new Item("Flor amarillenta", null, "Me encanta esta floresita que es de color amarilla");
        yellowFlower.itemID = 1;
        //Item florAmarillenta = new Item("Flor Amarillenta", 1);
        AddItem(yellowFlower);
    }

    private void Start()
    {
        Init();
    }

    #endregion

    #region Private methods

    private void Init()
    {


        for (int i = 0; i < nonEquipableButtons.Count; i++)
        {
            if (i < nonEquipableList.Count)
                nonEquipableButtons[i].sprite = nonEquipableList[i].Sprite;
            else
                nonEquipableButtons[i].sprite = _empty.Sprite;
        }

        EventSystem.current.SetSelectedGameObject(nonEquipableButtons[0].transform.parent.gameObject);
        _textInfo.text = nonEquipableList[0].Description;

        //foreach (Item item in items) // Busqueda de items no equipables en la ui para añadirlas al diccionario
        //{
        //    // TODO preguntar por este prefijo u otra forma de hacerlo
        //    Transform uiItemTransform = transform.Find("UI_ITEM_" + item.itemID);

        //    if (uiItemTransform != null)
        //    {
        //        GameObject uiItem = uiItemTransform.gameObject;

        //        if (uiItem != null)
        //        {
        //            uiItems[item.itemID] = uiItem; // ID DE LA UI
        //            itemUIStates[item.itemID] = true;
        //            itemsNoEquipables.Add(item.itemID);
        //            CheckItemInInventory(item.itemID);
        //        }
        //    }
        //}
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

    #endregion

    #region Public methods

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
            Debug.Log("Inventario lleno. No se puede añadir " + item.Name + ".");
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
            Debug.Log("No se encontró " + item.Name + " en el inventario.");
        }
    }

    #endregion

}