using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    [Tooltip("Lista de bayas")]
    private List<Button> _berryButtons;
    [SerializeField]
    [Tooltip("Textos con cantidades de bayas")]
    private List<TMP_Text> _berryQuantities;

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
    private InventoryEvents _inventoryEvent;
    private GameInputs _gameInputs;

    // INFORMATION
    private List<string> _titles;
    private List<string> _descriptions;

    // ELEMENTS
    private Button _currentSelected;

    // SERVICES
    // QUITAR EN UN FUTURO CUANDO SE DEFINA EL PERFORMED EN GAMEINPUTS
    private InputAction _navigateAction;
    private InputAction _cancelAction;
    private InputAction _nextMenuAction;
    private InputAction _prevMenuAction;

    #endregion

    #region Unity methods

    private void Awake() //TODO quitar solo es de prueba
    {
        _navigateAction = _inputActions.FindActionMap("UI").FindAction("Navigate");
        _cancelAction = _inputActions.FindActionMap("UI").FindAction("Cancel");
        _prevMenuAction = _inputActions.FindActionMap("UI").FindAction("PrevMenu");
        _nextMenuAction = _inputActions.FindActionMap("UI").FindAction("NextMenu");

        // Inicializamos variables
        _titles = new List<string>();
        _descriptions = new List<string>();

        // TEXTOS
        // OBJETOS NO EQUIPABLES
        _titles.Add("VARA MÁGICA");
        _descriptions.Add("La vara que te regaló papá. Se puede usar para golpear o invocar poderes mágicos.");
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
        _titles.Add("MAGIA PESOPLUMA");
        _descriptions.Add("Un hechizo que te permite alzar objetos muy pesados.");

        // OBJETOS CONSUMIBLES (BAYAS)
        _titles.Add("BAYA CURATIVA");
        _descriptions.Add("Una baya que te ayudará a restaurar parte de tu salud.");
        _titles.Add("BAYA MÁGICA");
        _descriptions.Add("Con esta baya nunca te faltará magia para lanzar tus hechizos.");
        _titles.Add("BAYA BOMBA");
        _descriptions.Add("Una baya que explota y produce mucho daño.");

    }

    private void Start()
    {
        // EVENTOS
        _inventoryEvent = ServiceLocator.GetService<InventoryEvents>();
        _gameInputs = ServiceLocator.GetService<GameInputs>();

        PlayerStatusSave ps = _playerStatusSO.playerStatusSave;
        // Damos un evento al click de botón
        foreach (Button button in _nonEquipableButtons)
            // TODO: Añadir sonido de "error" al pulsar, ya que no se pueden equipar
            button.onClick.AddListener(() => Debug.Log(button.gameObject.name));

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

    }

    private void OnDisable()
    {
        // Quitamos listeners
        RemoveListeners();

        // Desactivamos eventos
        _navigateAction.performed -= GameInputs_OnNavigate;
        _cancelAction.performed -= GameInputs_OnCancel;
        _nextMenuAction.performed -= GameInputs_OnNextMenu;
        _prevMenuAction.performed -= GameInputs_OnPrevMenu;

        // Desactivamos inputs
        _nextMenuAction.Disable();
        _prevMenuAction.Disable();
    }

    private void OnDestroy()
    {
        // Quitamos listeners
        RemoveListeners();

        // Desactivamos eventos
        _navigateAction.performed -= GameInputs_OnNavigate;
        _cancelAction.performed -= GameInputs_OnCancel;
        _nextMenuAction.performed -= GameInputs_OnNextMenu;
        _prevMenuAction.performed -= GameInputs_OnPrevMenu;

        // Desactivamos inputs
        _nextMenuAction.Disable();
        _prevMenuAction.Disable();
    }

    #endregion

    #region Private methods

    private void Init()
    {
        // Mostramos los iconos
        ShowIcons();

        // Añadimos los listeners
        AddListeners();

        // Activamos inputs
        _nextMenuAction.Enable();
        _prevMenuAction.Enable();

        // Activamos eventos
        _navigateAction.performed += GameInputs_OnNavigate;
        _cancelAction.performed += GameInputs_OnCancel;
        _nextMenuAction.performed += GameInputs_OnNextMenu;
        _prevMenuAction.performed += GameInputs_OnPrevMenu;

        // Y activamos selección actual
        _currentSelected = _nonEquipableButtons[0];
        _currentSelected.Select();

        if (_playerStatusSO.playerStatusSave.isPhysicAttackUnlocked)
            SetText(_titles[0], _descriptions[0]);
    }

    #region Icons

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

        // MAGIAS SECUNDARIAS
        // LUZ
        if (ps.isLightUnlocked)
            _secondaryMagicButtons[0].GetComponentsInChildren<Image>()[1].enabled = true;
        // AIRE
        if (ps.isAirUnlocked)
            _secondaryMagicButtons[1].GetComponentsInChildren<Image>()[1].enabled = true;
        // MOVER OBJETOS
        if (ps.isHeavyMovementUnlocked)
            _secondaryMagicButtons[2].GetComponentsInChildren<Image>()[1].enabled = true;

        // OBJETOS CONSUMIBLES (BAYAS)
        // BAYA CURATIVA
        if (ps.lifeBerryUnlocked)
        {
            // Activamos imágenes
            CanvasGroup canvas = _berryButtons[0].GetComponentInChildren<CanvasGroup>();
            // Añadimos alpha a los iconos
            if (ps.lifeBerryQuantity == 0)
                canvas.alpha = 100f / 255f;
            else
                canvas.alpha = 1f;
            // Y añadimos el texto con la cantidad
            _berryQuantities[0].text = ps.lifeBerryQuantity.ToString();

        }
        // BAYA MÁGICA
        if (ps.magicBerryUnlocked)
        {
            // Activamos imágenes
            CanvasGroup canvas = _berryButtons[1].GetComponentInChildren<CanvasGroup>();
            // Añadimos alpha a los iconos
            if (ps.magicBerryQuantity == 0)
                canvas.alpha = 100f / 255f;
            else
                canvas.alpha = 1f;
            // Y añadimos el texto con la cantidad
            _berryQuantities[1].text = ps.magicBerryQuantity.ToString();
        }
        // BAYA BOMBA
        if (ps.bombBerryUnlocked)
        {
            // Activamos imágenes
            CanvasGroup canvas = _berryButtons[2].GetComponentInChildren<CanvasGroup>();
            // Añadimos alpha a los iconos
            if (ps.bombBerryQuantity == 0)
                canvas.alpha = 100f / 255f;
            else
                canvas.alpha = 1f;
            // Y añadimos el texto con la cantidad
            _berryQuantities[2].text = ps.bombBerryQuantity.ToString();
        }

    }

    #endregion

    #region Button listeners

    private void AddListeners()
    {
        PlayerStatusSave ps = _playerStatusSO.playerStatusSave;

        // PRIMARY SKILL ASSIGNMENT
        if (ps.isFireWeakUnlocked)
            _primaryMagicButtons[0].onClick.AddListener(() => _inventoryEvent.ChangePrimarySkill(0));
        else
            // TODO: Añadir sonido de error, ya que no se puede asignar
            _primaryMagicButtons[0].onClick.AddListener(() => Debug.Log("No se puede asignar poder de fuego"));

        if (ps.isPlantWeakUnlocked)
            _primaryMagicButtons[4].onClick.AddListener(() => _inventoryEvent.ChangePrimarySkill(1));
        else
            // TODO: Añadir sonido de error, ya que no se puede asignar
            _primaryMagicButtons[4].onClick.AddListener(() => Debug.Log("No se puede asignar poder de planta"));

        if (ps.isWaterWeakUnlocked)
            _primaryMagicButtons[8].onClick.AddListener(() => _inventoryEvent.ChangePrimarySkill(2));
        else
            // TODO: Añadir sonido de error, ya que no se puede asignar
            _primaryMagicButtons[8].onClick.AddListener(() => Debug.Log("No se puede asignar poder de agua"));


        // SECONDARY SKILL ASSIGNMENT
        // TODO (para últimas 2 partes): Añadir cambio de secondarySkillIndex
        foreach (Button button in _secondaryMagicButtons)
            button.onClick.AddListener(() => Debug.Log(button.gameObject.name));

        foreach (Button button in _berryButtons)
            button.onClick.AddListener(() => Debug.Log(button.gameObject.name));
    }

    private void RemoveListeners()
    {
        // Quitamos todos los listeners
        // PRIMARY SKILLS
        _primaryMagicButtons[0].onClick.RemoveAllListeners();
        _primaryMagicButtons[4].onClick.RemoveAllListeners();
        _primaryMagicButtons[8].onClick.RemoveAllListeners();

        // SECONDARY SKILLS
        foreach (Button button in _secondaryMagicButtons)
            button.onClick.RemoveAllListeners();
        foreach (Button button in _berryButtons)
            button.onClick.RemoveAllListeners();
    }

    #endregion


    #region Description

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
            SetTextToBerry();
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
        PlayerStatusSave ps = _playerStatusSO.playerStatusSave;
        switch (_secondaryMagicButtons.IndexOf(_currentSelected))
        {
            case 0: // Luz
                if (ps.isLightUnlocked)
                    SetText(_titles[_nonEquipableButtons.Count + _primaryMagicButtons.Count],
                        _descriptions[_nonEquipableButtons.Count + _primaryMagicButtons.Count]);
                break;
            case 1: // Aire
                if (ps.isAirUnlocked)
                    SetText(_titles[_nonEquipableButtons.Count + _primaryMagicButtons.Count + 1],
                        _descriptions[_nonEquipableButtons.Count + _primaryMagicButtons.Count + 1]);
                break;
            case 2: // Mover objetos pesados
                if (ps.isHeavyMovementUnlocked)
                    SetText(_titles[_nonEquipableButtons.Count + _primaryMagicButtons.Count + 2],
                        _descriptions[_nonEquipableButtons.Count + _primaryMagicButtons.Count + 2]);
                break;
            default: // Elemento no definido
                SetText("", "");
                break;
        }
    }

    private void SetTextToBerry()
    {
        PlayerStatusSave ps = _playerStatusSO.playerStatusSave;
        switch (_berryButtons.IndexOf(_currentSelected))
        {
            case 0: // Baya de vida
                if (ps.lifeBerryUnlocked)
                    SetText(_titles[_nonEquipableButtons.Count + _primaryMagicButtons.Count + _secondaryMagicButtons.Count],
                        _descriptions[_nonEquipableButtons.Count + _primaryMagicButtons.Count + _secondaryMagicButtons.Count]);
                break;
            case 1: // Baya de magia
                if (ps.magicBerryUnlocked)
                    SetText(_titles[_nonEquipableButtons.Count + _primaryMagicButtons.Count + _secondaryMagicButtons.Count + 1],
                        _descriptions[_nonEquipableButtons.Count + _primaryMagicButtons.Count + _secondaryMagicButtons.Count + 1]);
                break;
            case 2: // Baya bomba
                if (ps.bombBerryUnlocked)
                    SetText(_titles[_nonEquipableButtons.Count + _primaryMagicButtons.Count + _secondaryMagicButtons.Count + 2],
                        _descriptions[_nonEquipableButtons.Count + _primaryMagicButtons.Count + _secondaryMagicButtons.Count + 2]);
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

    #region GameInputs

    private void GameInputs_OnNavigate(InputAction.CallbackContext ctx)
    {
        Invoke(nameof(ChangeText), .01f);
    }

    private void GameInputs_OnCancel(InputAction.CallbackContext ctx)
    {
        // TODO: Reproducir sonido
        gameObject.SetActive(false);
    }

    private void GameInputs_OnNextMenu(
        InputAction.CallbackContext ctx
        )
    {
        // TODO: Pasar a siguiente panel (configuración)
        Debug.Log("Abrimos panel sistema");
    }

    private void GameInputs_OnPrevMenu(
        InputAction.CallbackContext ctx
        )
    {
        // TODO: Pasar a panel anterior (mapa)
        Debug.Log("Abrimos panel mapa");
    }


    #endregion


    #endregion

    #region Public methods

    #endregion

}