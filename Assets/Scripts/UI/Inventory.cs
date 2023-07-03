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
    private InputAction navigateAction;

    #endregion

    #region Unity methods

    private void Awake() //TODO quitar solo es de prueba
    {
        navigateAction = _inputActions.FindActionMap("UI").FindAction("Navigate");

        _descriptions = new List<string>();

        // OBJETOS NO EQUIPABLES
        _descriptions.Add("VARA MÁGICA\nLa vara que te regaló papá. Se puede usar para golpear e invocar poderes mágicos.");
        _descriptions.Add("BOTAS DE SALTO\nUnas botas mágicas con las que podrás saltar muy alto.");
        _descriptions.Add("ALETAS\nCon ellas podrás meterte en el agua y nadar sin miedo a ahogarte.");
        _descriptions.Add("DASH\nTe permite realizar un ligero teletransporte.");

        // MAGIAS PRIMARIAS
        // Fuego
        _descriptions.Add("MAGIA DE FUEGO\nUsa el poder del fuego para quemar a tus enemigos");
        _descriptions.Add("BOLA DE FUEGO\nLanza una bola de fuego en línea recta");
        _descriptions.Add("LANZALLAMAS\nUtiliza el poder del lanzallamas para quemar a los enemigos ante ti");
        _descriptions.Add("RÁFAGA DE FUEGO\nInvoca una ráfaga de fuego que devasta todo a su alrededor");
        // Planta
        _descriptions.Add("MAGIA DE PLANTA\n");
        _descriptions.Add("MAGIA DÉBIL DE PLANTA\n");
        _descriptions.Add("MAGIA MEDIA DE PLANTA\n");
        _descriptions.Add("MAGIA FUERTE DE PLANTA\n");
        // Agua
        _descriptions.Add("MAGIA DE AGUA\n");
        _descriptions.Add("MAGIA DÉBIL DE AGUA\n");
        _descriptions.Add("MAGIA MEDIA DE AGUA\n");
        _descriptions.Add("MAGIA FUERTE DE AGUA\n");

        // MAGIAS SECUNDARIAS
        _descriptions.Add("BOLA DE LUZ\nLanza una bola de luz que te permite ver a oscuras.");
        _descriptions.Add("MAGIA DE AIRE\nTe permite lanzar ráfagas de aire para empujar a tus enemigos");

        // OBJETOS CONSUMIBLES
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

        foreach (Button button in _primaryMagicButtons)
            button.onClick.AddListener(() => Debug.Log(button.gameObject.name));

        if (_playerStatusSO.playerStatusSave.isPhysicAttackUnlocked)
            SetText(_descriptions[0]);
    }

    private void OnDestroy()
    {
        navigateAction.performed -= OnMovement;
        navigateAction.Disable();
    }

    #endregion

    #region Private methods

    private void Init()
    {
        //_gameInputs = ServiceLocator.GetService<GameInputs>();

        // Mostramos los iconos
        ShowIcons();

        // Activamos eventos
        navigateAction.performed += OnMovement;
        navigateAction.Enable();

        // Y activamos selección actual
        _currentSelected = _nonEquipableButtons[0];
        _currentSelected.Select();
    }

    private void ShowIcons()
    {
        // Vamos viendo si ha desbloqueado los distintos elementos
        PlayerStatusSave ps = _playerStatusSO.playerStatusSave;

        // OBJETOS NO EQUIPABLES
        // Vara:
        if (ps.isPhysicAttackUnlocked)
            _nonEquipableButtons[0].GetComponentsInChildren<Image>()[1].enabled = true;

        // Botas
        if (ps.isJumpUnlocked)
            _nonEquipableButtons[1].GetComponentsInChildren<Image>()[1].enabled = true;

        // Nadar
        if (ps.isSwimUnlocked)
            _nonEquipableButtons[2].GetComponentsInChildren<Image>()[1].enabled = true;

        // Dash
        if (ps.isDashUnlocked)
            _nonEquipableButtons[3].GetComponentsInChildren<Image>()[1].enabled = true;

        // MAGIAS PRIMARIAS
        // FUEGO
        // Poder débil
        if (ps.isFireWeakUnlocked)
        {
            _primaryMagicButtons[0].GetComponentsInChildren<Image>()[1].enabled = true;
            _primaryMagicButtons[1].GetComponentsInChildren<Image>()[1].enabled = true;
        }

        // Poder medio
        if (ps.isFireMediumUnlocked)
            _primaryMagicButtons[2].GetComponentsInChildren<Image>()[1].enabled = true;

        // Poder fuerte
        if (ps.isFireStrongUnlocked)
            _primaryMagicButtons[3].GetComponentsInChildren<Image>()[1].enabled = true;

        // PLANTA
        // Poder débil
        if (ps.isPlantWeakUnlocked)
        {
            _primaryMagicButtons[4].GetComponentsInChildren<Image>()[1].enabled = true;
            _primaryMagicButtons[5].GetComponentsInChildren<Image>()[1].enabled = true;
        }

        // Poder medio
        if (ps.isPlantMediumUnlocked)
            _primaryMagicButtons[6].GetComponentsInChildren<Image>()[1].enabled = true;

        // Poder fuerte
        if (ps.isPlantStrongUnlocked)
            _primaryMagicButtons[7].GetComponentsInChildren<Image>()[1].enabled = true;

        // AGUA
        // Poder débil
        if (ps.isWaterWeakUnlocked)
        {
            _primaryMagicButtons[8].GetComponentsInChildren<Image>()[1].enabled = true;
            _primaryMagicButtons[9].GetComponentsInChildren<Image>()[1].enabled = true;
        }

        // Poder medio
        if (ps.isWaterMediumUnlocked)
            _primaryMagicButtons[10].GetComponentsInChildren<Image>()[1].enabled = true;

        // Poder fuerte
        if (ps.isWaterStrongUnlocked)
            _primaryMagicButtons[11].GetComponentsInChildren<Image>()[1].enabled = true;

    }

    /// <summary>
    /// Cambia el texto según el objeto seleccionado
    /// </summary>
    private void ChangeText()
    {
        EventSystem current = EventSystem.current;

        _currentSelected = current.currentSelectedGameObject.GetComponent<Button>();

        if (_nonEquipableButtons.Contains(_currentSelected))
            SetTextToNonEquipableObject();
        else if (_primaryMagicButtons.Contains(_currentSelected))
            SetTextToPrimaryMagic();
        else if (_secondaryMagicButtons.Contains(_currentSelected))
            SetTextToSecondaryMagic();
        else
            SetTextToConsumableMagic();
    }

    private void SetTextToNonEquipableObject()
    {
        PlayerStatusSave ps = _playerStatusSO.playerStatusSave;

        switch (_nonEquipableButtons.IndexOf(_currentSelected))
        {
            // Objetos no equipables
            case 0: // Cetro
                if (ps.isPhysicAttackUnlocked)
                    SetText(_descriptions[0]);
                else
                    SetText("");
                break;
            case 1: // Botas
                if (ps.isJumpUnlocked)
                    SetText(_descriptions[1]);
                else
                    SetText("");
                break;
            case 2: // Nadar
                if (ps.isSwimUnlocked)
                    SetText(_descriptions[2]);
                else
                    SetText("");
                break;
            case 3: // Dash
                if (ps.isDashUnlocked)
                    SetText(_descriptions[3]);
                else
                    SetText("");
                break;
            default: // Elemento no definido
                SetText("");
                break;
        }
    }

    private void SetTextToPrimaryMagic()
    {
        PlayerStatusSave ps = _playerStatusSO.playerStatusSave;
        switch (_primaryMagicButtons.IndexOf(_currentSelected))
        {
            case 0: // Magia de fuego
                if (ps.isFireWeakUnlocked)
                    SetText(_descriptions[_nonEquipableButtons.Count]);
                else
                    SetText("");
                break;
            case 1: // Magia de fuego (débil)
                if (ps.isFireWeakUnlocked)
                    SetText(_descriptions[_nonEquipableButtons.Count + 1]);
                else
                    SetText("");
                break;
            case 2: // Magia de fuego (media)
                if (ps.isFireMediumUnlocked)
                    SetText(_descriptions[_nonEquipableButtons.Count + 2]);
                else
                    SetText("");
                break;
            case 3: // Magia de fuego (fuerte)
                if (ps.isFireStrongUnlocked)
                    SetText(_descriptions[_nonEquipableButtons.Count + 3]);
                else
                    SetText("");
                break;
            case 4: // Magia de planta
                if (ps.isPlantWeakUnlocked)
                    SetText(_descriptions[_nonEquipableButtons.Count + 4]);
                else
                    SetText("");
                break;
            case 5: // Magia de planta (débil)
                if (ps.isPlantWeakUnlocked)
                    SetText(_descriptions[_nonEquipableButtons.Count + 5]);
                else
                    SetText("");
                break;
            case 6: // Magia de planta (media)
                if (ps.isPlantMediumUnlocked)
                    SetText(_descriptions[_nonEquipableButtons.Count + 6]);
                else
                    SetText("");
                break;
            case 7: // Magia de planta (fuerte)
                if (ps.isPlantStrongUnlocked)
                    SetText(_descriptions[_nonEquipableButtons.Count + 7]);
                else
                    SetText("");
                break;
            case 8: // Magia de agua
                if (ps.isWaterWeakUnlocked)
                    SetText(_descriptions[_nonEquipableButtons.Count + 8]);
                else
                    SetText("");
                break;
            case 9: // Magia de agua (débil)
                if (ps.isWaterWeakUnlocked)
                    SetText(_descriptions[_nonEquipableButtons.Count + 9]);
                else
                    SetText("");
                break;
            case 10: // Magia de agua (media)
                if (ps.isWaterMediumUnlocked)
                    SetText(_descriptions[_nonEquipableButtons.Count + 10]);
                else
                    SetText("");
                break;
            case 11: // Magia de agua (fuerte)
                if (ps.isWaterStrongUnlocked)
                    SetText(_descriptions[_nonEquipableButtons.Count + 11]);
                else
                    SetText("");
                break;
            default: // Elemento no definido
                SetText("");
                break;
        }
    }

    private void SetTextToSecondaryMagic()
    {
        switch (_secondaryMagicButtons.IndexOf(_currentSelected))
        {
            case 0: // Luz
                break;
            case 1: // Aire
            default: // Elemento no definido
                SetText("");
                break;
        }
    }

    private void SetTextToConsumableMagic()
    {
        switch (_consumableButtons.IndexOf(_currentSelected))
        {
            case 0: // Baya de vida
                break;
            default: // Elemento no definido
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