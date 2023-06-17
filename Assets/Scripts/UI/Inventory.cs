using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utils;

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

    public Sprite exampleSprite;
    public List<Item> items = new List<Item>();
    public List<Item> nonEquipableList;
    public List<Image> nonEquipableButtons;
    public List<Item> powerList;
    public List<ConsumableItem> consumableList;
    public int inventorySpace = 10;

    #endregion

    #region Private variables
    private Dictionary<int, Item> uiItem = new Dictionary<int, Item>();
    private Dictionary<int, GameObject> uiItems = new Dictionary<int, GameObject>();
    private readonly Dictionary<int, bool> itemUIStates = new Dictionary<int, bool>();

    private List<int> itemsNoEquipables = new List<int>();

    // SERVICES
    private InputAction submitAction;
    public InputActionAsset inputActions;

    #endregion

    #region Unity methods

    private void Awake() //TODO quitar solo es de prueba
    {
        submitAction = inputActions.FindActionMap("UI").FindAction("Navigate");
        Item yellowFlower = new Item("Flor amarillenta", null, "Me encanta esta floresita que es de color amarilla");
        yellowFlower.itemID = 1;
        //Item florAmarillenta = new Item("Flor Amarillenta", 1);
        AddItem(yellowFlower);
    }

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        submitAction.performed += OnMovement;
        submitAction.Enable();
    }

    private void OnDestroy()
    {
        submitAction.performed -= OnMovement;
        submitAction.Disable();
    }

    #endregion

    #region Private methods

    private void Init()
    {
        foreach (var item in nonEquipableList)
            uiItem.Add(item.itemID, item);

        for (int i = 0; i < nonEquipableButtons.Count; i++)
        {
            if (i < nonEquipableList.Count)
                nonEquipableButtons[i].sprite = nonEquipableList[i].Sprite;
            else
                nonEquipableButtons[i].sprite = _empty.Sprite;
        }


        EventSystem ev = EventSystem.current;
        ev.SetSelectedGameObject(nonEquipableButtons[0].transform.parent.gameObject);
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

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        Debug.Log("Cambio selección");
        EventSystem ev = EventSystem.current;
        int n = int.Parse(ev.currentSelectedGameObject.name);
        if (uiItem.ContainsKey(n))
        {
            _textInfo.text = uiItem[n].Description;
        }
        else
        {
            _textInfo.text = _empty.Description;
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