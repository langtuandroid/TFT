using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using Honeti;

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

    // ELEMENTS
    private Button _currentSelected;

    // SERVICES
    // QUITAR EN UN FUTURO CUANDO SE DEFINA EL PERFORMED EN GAMEINPUTS
    private InputAction _navigateAction;

    // VARIABLES
    private int _primarySkillSelected;
    private int _secondarySkillSelected;

    #endregion

    #region Unity methods

    private void Awake() //TODO quitar solo es de prueba
    {
        _primarySkillSelected = -1;
        _secondarySkillSelected = -1;

    }

    private void Start()
    {
        // EVENTS
        // Inventory
        _inventoryEvent = ServiceLocator.GetService<InventoryEvents>();
        _inventoryEvent.OnPrimarySkillChange += OnChangePrimarySkill;
        _inventoryEvent.OnSecondarySkillChange += OnChangeSecondarySkill;

        // GameInputs
        _gameInputs = ServiceLocator.GetService<GameInputs>();
        _gameInputs.OnCancelPerformed += GameInputs_OnCancel;
        _gameInputs.OnNextMenuPerformed += GameInputs_OnNextMenu;
        _gameInputs.OnPrevMenuPerformed += GameInputs_OnPrevMenu;

        _navigateAction = _gameInputs.NavigateAction;
        _navigateAction.performed += GameInputs_OnNavigate;


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
    }

    private void OnDestroy()
    {
        // Quitamos listeners
        RemoveListeners();

        // Desactivamos eventos

        if (_gameInputs != null)
        {
            _navigateAction.performed -= GameInputs_OnNavigate;
            _gameInputs.OnCancelPerformed -= GameInputs_OnCancel;
            _gameInputs.OnNextMenuPerformed -= GameInputs_OnNextMenu;
            _gameInputs.OnPrevMenuPerformed -= GameInputs_OnPrevMenu;
        }

        if (_inventoryEvent != null)
        {
            _inventoryEvent.OnPrimarySkillChange -= OnChangePrimarySkill;
            _inventoryEvent.OnSecondarySkillChange -= OnChangeSecondarySkill;
        }

    }

    #endregion

    #region Private methods

    private void Init()
    {
        // Mostramos los iconos
        ShowIcons();

        // Activamos selección actual
        _currentSelected = _nonEquipableButtons[0];
        _currentSelected.Select();

        // Añadimos los listeners
        AddListeners();

        // Y mostramos los botones del color que correspondan
        OnChangePrimarySkill(_playerStatusSO.playerStatusSave.primarySkillIndex);
        OnChangeSecondarySkill(_playerStatusSO.playerStatusSave.secondarySkillIndex);

        // Y finalmente, cambiamos el texto
        ChangeText();
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
            // O quizá una pantalla con un mensaje diciendo que no está disponible
            _primaryMagicButtons[0].onClick.AddListener(() => Debug.Log("No se puede asignar poder de fuego"));

        if (ps.isPlantWeakUnlocked)
            _primaryMagicButtons[4].onClick.AddListener(() => _inventoryEvent.ChangePrimarySkill(1));
        else
            // TODO: Añadir sonido de error, ya que no se puede asignar
            // O quizá una pantalla con un mensaje diciendo que no está disponible
            _primaryMagicButtons[4].onClick.AddListener(() => Debug.Log("No se puede asignar poder de planta"));

        if (ps.isWaterWeakUnlocked)
            _primaryMagicButtons[8].onClick.AddListener(() => _inventoryEvent.ChangePrimarySkill(2));
        else
            // TODO: Añadir sonido de error, ya que no se puede asignar
            // O quizá una pantalla con un mensaje diciendo que no está disponible
            _primaryMagicButtons[8].onClick.AddListener(() => Debug.Log("No se puede asignar poder de agua"));


        // SECONDARY SKILL ASSIGNMENT
        // TODO (para últimas 2 partes): Añadir cambio de secondarySkillIndex

        if (ps.isLightUnlocked)
            _secondaryMagicButtons[0].onClick.AddListener(() => _inventoryEvent.ChangeSecondarySkill(0));
        else
            _secondaryMagicButtons[0].onClick.AddListener(() => Debug.Log("No se puede asignar bola de luz"));

        if (ps.isAirUnlocked)
            _secondaryMagicButtons[1].onClick.AddListener(() => _inventoryEvent.ChangeSecondarySkill(1));
        else
            _secondaryMagicButtons[1].onClick.AddListener(() => Debug.Log("No se puede asignar magia de aire"));

        if (ps.isHeavyMovementUnlocked)
            _secondaryMagicButtons[2].onClick.AddListener(() => _inventoryEvent.ChangeSecondarySkill(2));
        else
            _secondaryMagicButtons[2].onClick.AddListener(() => Debug.Log("No se puede asignar magia de mover objetos pesados"));

        if (ps.lifeBerryUnlocked)
            _berryButtons[0].onClick.AddListener(() => _inventoryEvent.ChangeSecondarySkill(3));
        else
            _berryButtons[0].onClick.AddListener(() => Debug.Log("No se puede asignar baya curativa"));

        if (ps.magicBerryUnlocked)
            _berryButtons[1].onClick.AddListener(() => _inventoryEvent.ChangeSecondarySkill(4));
        else
            _berryButtons[1].onClick.AddListener(() => Debug.Log("No se puede asignar baya mágica"));

        if (ps.bombBerryUnlocked)
            _berryButtons[2].onClick.AddListener(() => _inventoryEvent.ChangeSecondarySkill(5));
        else
            _berryButtons[2].onClick.AddListener(() => Debug.Log("No se puede asignar baya bomba"));
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

    private void OnChangePrimarySkill(int skill)
    {
        if (skill == _primarySkillSelected)
            return;

        if (_primarySkillSelected >= 0)
            _primaryMagicButtons[_primarySkillSelected * 4].GetComponent<Image>().color = Color.white;

        _primaryMagicButtons[skill * 4].GetComponent<Image>().color = Color.yellow;
        _primarySkillSelected = skill;
    }

    private void OnChangeSecondarySkill(int skill)
    {
        if (skill == _secondarySkillSelected)
            return;

        if (_secondarySkillSelected > 2)
            _berryButtons[_secondarySkillSelected - 3].GetComponent<Image>().color = Color.white;
        else if (_secondarySkillSelected >= 0)
            _secondaryMagicButtons[_secondarySkillSelected].GetComponent<Image>().color = Color.white;

        if (skill > 2)
            _berryButtons[skill - 3].GetComponent<Image>().color = Color.yellow;
        else
            _secondaryMagicButtons[skill].GetComponent<Image>().color = Color.yellow;

        _secondarySkillSelected = skill;
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

        int n = _nonEquipableButtons.IndexOf(_currentSelected);
        string tit = "^inventory_tit_" + n;
        string desc = "^inventory_desc_" + n;

        switch (_nonEquipableButtons.IndexOf(_currentSelected))
        {
            // Objetos no equipables
            case 0: // Cetro
                if (ps.isPhysicAttackUnlocked)
                    SetText(I18N.instance.getValue(tit), I18N.instance.getValue(desc));
                else
                    SetText("", "");
                break;
            case 1: // Botas
                if (ps.isJumpUnlocked)
                    SetText(I18N.instance.getValue(tit), I18N.instance.getValue(desc));
                else
                    SetText("", "");
                break;
            case 2: // Nadar
                if (ps.isSwimUnlocked)
                    SetText(I18N.instance.getValue(tit), I18N.instance.getValue(desc));
                else
                    SetText("", "");
                break;
            case 3: // Dash
                if (ps.isDashUnlocked)
                    SetText(I18N.instance.getValue(tit), I18N.instance.getValue(desc));
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
        int n = _primaryMagicButtons.IndexOf(_currentSelected);

        string tit = "^inventory_tit_" +
            (n + _nonEquipableButtons.Count);
        string desc = "^inventory_desc_" +
            (n + _nonEquipableButtons.Count);


        switch (n)
        {
            case 0: // Magia de fuego
                if (ps.isFireWeakUnlocked)
                    SetText(I18N.instance.getValue(tit), I18N.instance.getValue(desc));
                else
                    SetText("", "");
                break;
            case 1: // Magia de fuego (débil)
                if (ps.isFireWeakUnlocked)
                    SetText(I18N.instance.getValue(tit), I18N.instance.getValue(desc));
                else
                    SetText("", "");
                break;
            case 2: // Magia de fuego (media)
                if (ps.isFireMediumUnlocked)
                    SetText(I18N.instance.getValue(tit), I18N.instance.getValue(desc));
                else
                    SetText("", "");
                break;
            case 3: // Magia de fuego (fuerte)
                if (ps.isFireStrongUnlocked)
                    SetText(I18N.instance.getValue(tit), I18N.instance.getValue(desc));
                else
                    SetText("", "");
                break;
            case 4: // Magia de planta
                if (ps.isPlantWeakUnlocked)
                    SetText(I18N.instance.getValue(tit), I18N.instance.getValue(desc));
                else
                    SetText("", "");
                break;
            case 5: // Magia de planta (débil)
                if (ps.isPlantWeakUnlocked)
                    SetText(I18N.instance.getValue(tit), I18N.instance.getValue(desc));
                else
                    SetText("", "");
                break;
            case 6: // Magia de planta (media)
                if (ps.isPlantMediumUnlocked)
                    SetText(I18N.instance.getValue(tit), I18N.instance.getValue(desc));
                else
                    SetText("", "");
                break;
            case 7: // Magia de planta (fuerte)
                if (ps.isPlantStrongUnlocked)
                    SetText(I18N.instance.getValue(tit), I18N.instance.getValue(desc));
                else
                    SetText("", "");
                break;
            case 8: // Magia de agua
                if (ps.isWaterWeakUnlocked)
                    SetText(I18N.instance.getValue(tit), I18N.instance.getValue(desc));
                else
                    SetText("", "");
                break;
            case 9: // Magia de agua (débil)
                if (ps.isWaterWeakUnlocked)
                    SetText(I18N.instance.getValue(tit), I18N.instance.getValue(desc));
                else
                    SetText("", "");
                break;
            case 10: // Magia de agua (media)
                if (ps.isWaterMediumUnlocked)
                    SetText(I18N.instance.getValue(tit), I18N.instance.getValue(desc));
                else
                    SetText("", "");
                break;
            case 11: // Magia de agua (fuerte)
                if (ps.isWaterStrongUnlocked)
                    SetText(I18N.instance.getValue(tit), I18N.instance.getValue(desc));
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

        int n = _secondaryMagicButtons.IndexOf(_currentSelected);

        string tit = "^inventory_tit_" +
            (n + _nonEquipableButtons.Count + _primaryMagicButtons.Count);
        string desc = "^inventory_desc_" +
            (n + _nonEquipableButtons.Count + _primaryMagicButtons.Count);

        switch (n)
        {
            case 0: // Luz
                if (ps.isLightUnlocked)
                    SetText(I18N.instance.getValue(tit), I18N.instance.getValue(desc));
                break;
            case 1: // Aire
                if (ps.isAirUnlocked)
                    SetText(I18N.instance.getValue(tit), I18N.instance.getValue(desc));
                break;
            case 2: // Mover objetos pesados
                if (ps.isHeavyMovementUnlocked)
                    SetText(I18N.instance.getValue(tit), I18N.instance.getValue(desc));
                break;
            default: // Elemento no definido
                SetText("", "");
                break;
        }
    }

    private void SetTextToBerry()
    {
        PlayerStatusSave ps = _playerStatusSO.playerStatusSave;

        int n = _berryButtons.IndexOf(_currentSelected);

        string tit = "^inventory_tit_" +
            (n + _nonEquipableButtons.Count + _primaryMagicButtons.Count + _secondaryMagicButtons.Count);
        string desc = "^inventory_desc_" +
            (n + _nonEquipableButtons.Count + _primaryMagicButtons.Count + _secondaryMagicButtons.Count);

        switch (n)
        {
            case 0: // Baya de vida
                if (ps.lifeBerryUnlocked)
                    SetText(I18N.instance.getValue(tit), I18N.instance.getValue(desc));
                break;
            case 1: // Baya de magia
                if (ps.magicBerryUnlocked)
                    SetText(I18N.instance.getValue(tit), I18N.instance.getValue(desc));
                break;
            case 2: // Baya bomba
                if (ps.bombBerryUnlocked)
                    SetText(I18N.instance.getValue(tit), I18N.instance.getValue(desc));
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

    private void GameInputs_OnCancel()
    {
        // TODO: Reproducir sonido
        gameObject.SetActive(false);
    }

    private void GameInputs_OnNextMenu()
    {
        // TODO: Pasar a siguiente panel (configuración)
        Debug.Log("Abrimos panel sistema");
    }

    private void GameInputs_OnPrevMenu()
    {
        // TODO: Pasar a panel anterior (mapa)
        Debug.Log("Abrimos panel mapa");
    }


    #endregion


    #endregion

    #region Public methods

    #endregion

}