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

    [Header("UI elements")]

    [SerializeField]
    [Tooltip("Lista de botones de objetos no equipables")]
    private List<Button> _nonEquipableButtons;
    [SerializeField]
    [Tooltip("Lista de magias primarias (incluyendo cada subtipo)")]
    private List<Button> _primaryMagicButtons;
    [SerializeField]
    [Tooltip("Lista de magias secundarias")]
    private List<Button> _secondaryMagicButtons;
    [SerializeField]
    [Tooltip("Lista de objetos consumibles")]
    private List<Button> _consumableButtons;

    [SerializeField]
    [Tooltip("Parte que muestra la descripción del objeto seleccionado")]
    private TMP_Text _textInfo;

    [Header("Input Action Asset")]

    [SerializeField]
    [Tooltip("Input action asset")]
    private InputActionAsset _inputActions;

    #endregion


    #region Public variables

    #endregion

    #region Private variables

    // SERVICES
    //private GameInputs _gameInputs;

    // INFORMATION
    private List<string> _descriptions;

    // ELEMENTS
    private Button _currentSelected;

    // SERVICES
    private InputAction submitAction;


    #endregion

    #region Unity methods

    private void Awake() //TODO quitar solo es de prueba
    {
        submitAction = _inputActions.FindActionMap("UI").FindAction("Navigate");

        _descriptions = new List<string>();
        _descriptions.Add("VARA MÁGICA\nLa vara que te regaló papá.Te permite atacar y usar poderes mágicos.");
        _descriptions.Add("BOTAS DE SALTO\nUnas botas mágicas con las que podrás saltar muy alto.");
        _descriptions.Add("DASH\nTe permite realizar un ligero teletransporte.");
        _descriptions.Add("COGER OBJETOS PESADOS\nCon ello podrás alzar objetos muy pesados.");
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
            _currentSelected.Select();
    }

    private void OnEnable()
    {
        // Inicializamos
        Init();

        // Damos un evento al click de botón
        foreach (Button button in _nonEquipableButtons)
            button.onClick.AddListener(() => Debug.Log(button.gameObject.name));

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

        // Activamos eventos
        submitAction.performed += OnMovement;
        submitAction.Enable();

        // Y activamos selección actual
        _currentSelected = _nonEquipableButtons[0];
        _currentSelected.Select();
    }

    private void ShowIcons()
    {
        // Vamos viendo si ha desbloqueado los distintos elementos
        PlayerStatusSave ps = _playerStatusSO.playerStatusSave;

        // Vara:
        if (ps.isPhysicAttackUnlocked)
            _nonEquipableButtons[0].GetComponentsInChildren<Image>()[1].enabled = true;


        // Botas
        if (ps.isJumpUnlocked)
            _nonEquipableButtons[1].GetComponentsInChildren<Image>()[1].enabled = true;


        // Dash
        if (ps.isDashUnlocked)
            _nonEquipableButtons[2].GetComponentsInChildren<Image>()[1].enabled = true;

    }

    /// <summary>
    /// Cambia el texto según el objeto seleccionado
    /// </summary>
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

    /// <summary>
    /// Cambia el texto descriptivo según el texto enviado
    /// </summary>
    /// <param name="text"></param>
    private void SetText(string text)
    {
        _textInfo.text = text;
    }

    #endregion

    #region Public methods

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        Invoke(nameof(ChangeText), .1f);
    }


    #endregion

}