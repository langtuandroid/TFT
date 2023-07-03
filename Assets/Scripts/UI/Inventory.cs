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
    [Tooltip("Parte en la que se ve el nombre del objeto seleccionado")]
    private TMP_Text _titleInfo;
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
    private List<string> _titles;
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

        _titles = new List<string>();

        _descriptions = new List<string>();

        // OBJETOS NO EQUIPABLES
        _titles.Add("VARA MÁGICA");
        _descriptions.Add("La vara que te regaló papá. Se puede usar para golpear e invocar poderes mágicos.");
        _titles.Add("BOTAS DE SALTO");
        _descriptions.Add("Unas botas mágicas con las que podrás saltar muy alto.");
        _titles.Add("ALETAS");
        _descriptions.Add("Con ellas podrás meterte en el agua y nadar sin miedo a ahogarte.");
        _titles.Add("DASH");
        _descriptions.Add("Te permite realizar un ligero teletransporte.");

        // MAGIAS PRIMARIAS
        // Fuego
        _titles.Add("MAGIA DE FUEGO");
        _descriptions.Add("Usa el poder del fuego para quemar a tus enemigos.");
        _titles.Add("BOLA DE FUEGO");
        _descriptions.Add("Lanza una bola de fuego en línea recta.");
        _titles.Add("LANZALLAMAS");
        _descriptions.Add("Utiliza el poder del lanzallamas para quemar a los enemigos ante ti.");
        _titles.Add("RÁFAGA DE FUEGO");
        _descriptions.Add("Invoca una ráfaga de fuego que devasta todo a su alrededor.");
        // Planta
        _titles.Add("MAGIA DE PLANTA");
        _descriptions.Add("Información sobre la magia de planta.");
        _titles.Add("MAGIA DÉBIL DE PLANTA");
        _descriptions.Add("Información sobre la magia débil de planta.");
        _titles.Add("MAGIA MEDIA DE PLANTA");
        _descriptions.Add("Información sobre la magia media de planta.");
        _titles.Add("MAGIA FUERTE DE PLANTA");
        _descriptions.Add("Información sobre la magia fuerte de planta.");
        // Agua
        _titles.Add("MAGIA DE AGUA");
        _descriptions.Add("Información sobre la magia de agua.");
        _titles.Add("MAGIA DÉBIL DE AGUA");
        _descriptions.Add("Información sobre la magia débil de agua.");
        _titles.Add("MAGIA MEDIA DE AGUA");
        _descriptions.Add("Información sobre la magia media de agua.");
        _titles.Add("MAGIA FUERTE DE AGUA");
        _descriptions.Add("Información sobre la magia fuerte de agua.");

        // MAGIAS SECUNDARIAS
        _titles.Add("BOLA DE LUZ");
        _descriptions.Add("Lanza una bola de luz que te permite ver a oscuras.");
        _titles.Add("MAGIA DE AIRE");
        _descriptions.Add("Te permite lanzar ráfagas de aire para empujar a tus enemigos.");

        // OBJETOS CONSUMIBLES
        _titles.Add("COGER OBJETOS PESADOS");
        _descriptions.Add("Con ello podrás alzar objetos muy pesados.");
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
            SetText(_titles[0], _descriptions[0]);
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
                    SetText(_titles[0], _descriptions[0]);
                else
                    SetText("", "");
                break;
            case 1: // Botas
                if (ps.isJumpUnlocked)
                    SetText(_titles[1], _descriptions[1]);
                else
                    SetText("", "");
                break;
            case 2: // Nadar
                if (ps.isSwimUnlocked)
                    SetText(_titles[2], _descriptions[2]);
                else
                    SetText("", "");
                break;
            case 3: // Dash
                if (ps.isDashUnlocked)
                    SetText(_titles[3], _descriptions[3]);
                else
                    SetText("", "");
                break;
            default: // Elemento no definido
                SetText("", "");
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
                    SetText(_titles[_nonEquipableButtons.Count], 
                        _descriptions[_nonEquipableButtons.Count]);
                else
                    SetText("", "");
                break;
            case 1: // Magia de fuego (débil)
                if (ps.isFireWeakUnlocked)
                    SetText(_titles[_nonEquipableButtons.Count + 1], 
                        _descriptions[_nonEquipableButtons.Count + 1]);
                else
                    SetText("", "");
                break;
            case 2: // Magia de fuego (media)
                if (ps.isFireMediumUnlocked)
                    SetText(_titles[_nonEquipableButtons.Count + 2], 
                        _descriptions[_nonEquipableButtons.Count + 2]);
                else
                    SetText("", "");
                break;
            case 3: // Magia de fuego (fuerte)
                if (ps.isFireStrongUnlocked)
                    SetText(_titles[_nonEquipableButtons.Count + 3], 
                        _descriptions[_nonEquipableButtons.Count + 3]);
                else
                    SetText("", "");
                break;
            case 4: // Magia de planta
                if (ps.isPlantWeakUnlocked)
                    SetText(_titles[_nonEquipableButtons.Count + 4], 
                        _descriptions[_nonEquipableButtons.Count + 4]);
                else
                    SetText("", "");
                break;
            case 5: // Magia de planta (débil)
                if (ps.isPlantWeakUnlocked)
                    SetText(_titles[_nonEquipableButtons.Count + 5],
                        _descriptions[_nonEquipableButtons.Count + 5]);
                else
                    SetText("", "");
                break;
            case 6: // Magia de planta (media)
                if (ps.isPlantMediumUnlocked)
                    SetText(_titles[_nonEquipableButtons.Count + 6],
                        _descriptions[_nonEquipableButtons.Count + 6]);
                else
                    SetText("", "");
                break;
            case 7: // Magia de planta (fuerte)
                if (ps.isPlantStrongUnlocked)
                    SetText(_titles[_nonEquipableButtons.Count + 7],
                        _descriptions[_nonEquipableButtons.Count + 7]);
                else
                    SetText("", "");
                break;
            case 8: // Magia de agua
                if (ps.isWaterWeakUnlocked)
                    SetText(_titles[_nonEquipableButtons.Count + 8],
                        _descriptions[_nonEquipableButtons.Count + 8]);
                else
                    SetText("", "");
                break;
            case 9: // Magia de agua (débil)
                if (ps.isWaterWeakUnlocked)
                    SetText(_titles[_nonEquipableButtons.Count + 9],
                        _descriptions[_nonEquipableButtons.Count + 9]);
                else
                    SetText("", "");
                break;
            case 10: // Magia de agua (media)
                if (ps.isWaterMediumUnlocked)
                    SetText(_titles[_nonEquipableButtons.Count + 10],
                        _descriptions[_nonEquipableButtons.Count + 10]);
                else
                    SetText("", "");
                break;
            case 11: // Magia de agua (fuerte)
                if (ps.isWaterStrongUnlocked)
                    SetText(_titles[_nonEquipableButtons.Count + 11],
                        _descriptions[_nonEquipableButtons.Count + 11]
                        );
                else
                    SetText("", "");
                break;
            default: // Elemento no definido
                SetText("", "");
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
                SetText("", "");
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
                SetText("", "");
                break;
        }
    }

    /// <summary>
    /// Cambia el texto descriptivo según el texto enviado
    /// </summary>
    /// <param name="text"></param>
    private void SetText(string name, string description)
    {
        _titleInfo.text = name;
        _textInfo.text = description;
    }

    #endregion

    #region Public methods

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        Invoke(nameof(ChangeText), .1f);
    }


    #endregion

}