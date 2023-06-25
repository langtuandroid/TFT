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

    [Header("Game information")]

    [SerializeField]
    [Tooltip("Información del juego")]
    private PlayerStatusSaveSO _playerStatusSO;

    [SerializeField]
    [Tooltip("Lista con las descripciones de los objetos")]
    private List<string> _descriptions;

    [Header("UI elements")]

    [SerializeField]
    [Tooltip("Lista de botones de objetos no equipables")]
    private List<Button> _nonEquipableButtons;

    [SerializeField]
    [Tooltip("Parte que muestra la descripción del objeto seleccionado")]
    private TMP_Text _textInfo;

    [Header("Input Action Asset")]

    [SerializeField]
    [Tooltip("Input action asset")]
    private InputActionAsset _inputActions;

    #endregion


    #region Public variables

    //public List<Item> items = new List<Item>();
    //public List<Item> nonEquipableList;

    //public List<Item> powerList;
    //public List<ConsumableItem> consumableList;

    #endregion

    #region Private variables

    // SERVICES
    //private GameInputs _gameInputs;

    // ELEMENTS
    private Button _currentSelected;
    //private Dictionary<int, Item> uiItem = new Dictionary<int, Item>();
    //private Dictionary<int, GameObject> uiItems = new Dictionary<int, GameObject>();
    //private readonly Dictionary<int, bool> itemUIStates = new Dictionary<int, bool>();

    //private List<int> itemsNoEquipables = new List<int>();

    // SERVICES
    private InputAction submitAction;


    #endregion

    #region Unity methods

    private void Awake() //TODO quitar solo es de prueba
    {
        submitAction = _inputActions.FindActionMap("UI").FindAction("Navigate");
        //Item yellowFlower = new Item("Flor amarillenta", null, "Me encanta esta floresita que es de color amarilla");
        //yellowFlower.itemID = 1;
        ////Item florAmarillenta = new Item("Flor Amarillenta", 1);
        //AddItem(yellowFlower);
    }

    private void OnEnable()
    {
        Init();

        foreach (Button button in _nonEquipableButtons)
            button.onClick.AddListener(() => Debug.Log(button.gameObject.name));

        submitAction.performed += OnMovement;
        submitAction.Enable();

        _currentSelected = _nonEquipableButtons[0];
        _currentSelected.Select();

        if (_playerStatusSO.playerStatusSave.isPhysicAttackUnlocked)
            SetText(_descriptions[0]);
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
        //_gameInputs = ServiceLocator.GetService<GameInputs>();

        // Mostramos los iconos
        ShowIcons();



        //foreach (var item in nonEquipableList)
        //    uiItem.Add(item.itemID, item);

        //for (int i = 0; i < nonEquipableButtons.Count; i++)
        //{
        //    if (i < nonEquipableList.Count)
        //        nonEquipableButtons[i].sprite = nonEquipableList[i].Sprite;
        //    else
        //        nonEquipableButtons[i].sprite = _empty.Sprite;
        //}



        //_textInfo.text = nonEquipableList[0].Description;


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

    private void ShowIcons()
    {
        // Vamos viendo si ha desbloqueado los distintos elementos
        PlayerStatusSave ps = _playerStatusSO.playerStatusSave;

        // Vara:
        if (ps.isPhysicAttackUnlocked)
            _nonEquipableButtons[0].GetComponentInChildren<Image>().enabled = true;

        // Botas
        if (ps.isJumpUnlocked)
            _nonEquipableButtons[1].GetComponentInChildren<Image>().enabled = true;

        // Dash
        if (ps.isDashUnlocked)
            _nonEquipableButtons[2].GetComponentInChildren<Image>().enabled = true;
    }

    //private void CheckItemInInventory(int itemID)
    //{
    //    if (itemUIStates.ContainsKey(itemID))
    //    {
    //        bool isActive = itemUIStates[itemID];

    //        if (uiItems.ContainsKey(itemID))
    //        {
    //            uiItems[itemID].SetActive(isActive);
    //        }
    //    }
    //}

    #endregion

    #region Public methods

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        Invoke(nameof(ChangeText), .1f);
    }

    private void ChangeText()
    {
        EventSystem current = EventSystem.current;

        _currentSelected = current.currentSelectedGameObject.GetComponent<Button>();

        PlayerStatusSave ps = _playerStatusSO.playerStatusSave;


        int i = 0;

        if (_nonEquipableButtons.Contains(_currentSelected))
            for (i = 0; i < _nonEquipableButtons.Count && _currentSelected != _nonEquipableButtons[i]; i++) ;

        switch (i)
        {
            case 0:
                if (ps.isPhysicAttackUnlocked)
                    SetText(_descriptions[0]);
                else
                    SetText("");
                break;
            case 1:
                if (ps.isJumpUnlocked)
                    SetText(_descriptions[1]);
                else
                    SetText("");
                break;
            case 2:
                if (ps.isDashUnlocked)
                    SetText(_descriptions[2]);
                else
                    SetText("");
                break;
            default:
                SetText("");
                break;
        }
    }

    private void SetText(string text)
    {
        _textInfo.text = text;
    }


    //public bool AddItem(Item item)
    //{
    //    if (items.Count < inventorySpace)
    //    {
    //        items.Add(item);
    //        itemUIStates[item.itemID] = true;
    //        if (uiItems.ContainsKey(item.itemID))
    //        {
    //            uiItems[item.itemID].SetActive(true);
    //        }
    //        return true;
    //    }
    //    else
    //    {
    //        Debug.Log("Inventario lleno. No se puede añadir " + item.Name + ".");
    //        return false;
    //    }
    //}

    //public void RemoveItem(Item item)
    //{
    //    if (items.Contains(item))
    //    {
    //        items.Remove(item);
    //        itemUIStates.Remove(item.itemID);
    //        if (uiItems.ContainsKey(item.itemID))
    //        {
    //            uiItems[item.itemID].SetActive(false);
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("No se encontró " + item.Name + " en el inventario.");
    //    }
    //}

    #endregion

}