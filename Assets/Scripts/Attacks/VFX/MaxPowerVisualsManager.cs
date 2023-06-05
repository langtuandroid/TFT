using Scriptable;
using UnityEngine;
using Utils;
using Attack;
using System;
using UnityEngine.UI;

public class MaxPowerVisualsManager : MonoBehaviour
{

    #region SerializeFields

    [SerializeField]
    [Tooltip("Lista con los datos de los distintos paneles")]
    private PowerPanelDataListScriptable _powerPanelList;

    [SerializeField]
    [Tooltip("Panel con el efecto de ataque")]
    private Image _panel;

    [SerializeField]
    [Tooltip("Cantidad de carga del poder máximo")]
    private Image _maxPowerIcon;
    [SerializeField]
    [Tooltip("Fondo de la carga del poder máximo")]
    private Image _maxPowerBackground;

    #endregion

    #region Public variables

    public static MaxPowerVisualsManager Instance { get; private set; }

    #endregion

    #region Private variables

    private MagicEvents _magicEvents;

    #endregion

    #region Unity methods

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        Initialize();

        _magicEvents.OnAttackTypeValue += OnAttackTypeValueChange;
        _magicEvents.OnFillAmountValue += OnFillAmountValueChange;
        _magicEvents.OnPanelAlphaValue += OnPanelAlphaValueChange;
    }

    private void OnDestroy()
    {
        _magicEvents.OnAttackTypeValue -= OnAttackTypeValueChange;
        _magicEvents.OnFillAmountValue -= OnFillAmountValueChange;
        _magicEvents.OnPanelAlphaValue -= OnPanelAlphaValueChange;
    }

    #endregion

    #region Private methods

    #region Initialization

    /// <summary>
    /// Introduce los datos para los 3 tipos de paneles
    /// </summary>
    private void Initialize()
    {
        _magicEvents = ServiceLocator.GetService<MagicEvents>();

        if (_powerPanelList.PowerPanelDataList.Count > 0)
            return;

        PowerPanelData panel1 =
            new PowerPanelData(
                Constants.PANEL_FIRE,
                new Color(
                    161 / 255f,
                    97 / 255f,
                    89 / 255f
                    )
            );
        PowerPanelData panel2 =
            new PowerPanelData(
                Constants.PANEL_LEAF,
                new Color(
                    120 / 255f,
                    161 / 255f,
                    88 / 255f
                    )
                );
        PowerPanelData panel3 =
            new PowerPanelData(
                Constants.PANEL_WATER,
                new Color(
                    87 / 255f,
                    114 / 255f,
                    151 / 255f
                    )
                );

        _powerPanelList.PowerPanelDataList.Clear();
        _powerPanelList.PowerPanelDataList.Add(panel1);
        _powerPanelList.PowerPanelDataList.Add(panel2);
        _powerPanelList.PowerPanelDataList.Add(panel3);
    }

    #endregion

    #region Events

    private void OnAttackTypeValueChange(MagicAttack attack)
    {
        PowerPanelData data = GetData(attack);
        _panel.color = data.Color;
        _maxPowerIcon.color = data.Color;
        _maxPowerBackground.color = data.Color;
        //_maxPowerBackground.SetImageAlpha(0);
        //_maxPowerBackground.SetImageAlpha(100f / 255f);
    }

    private void OnFillAmountValueChange(float fillAmount)
    {
        _maxPowerIcon.fillAmount = fillAmount;
    }

    private void OnPanelAlphaValueChange(float alpha)
    {
        _panel.SetImageAlpha(alpha);
    }

    #endregion

    #region Getting data

    /// <summary>
    /// Obtiene un panel según el ID del poder
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public PowerPanelData GetData(string ID)
    {
        return _powerPanelList.PowerPanelDataList.Find(data => data.PowerName == ID);
    }

    /// <summary>
    /// Obtiene un panel según el poder usado
    /// </summary>
    /// <param name="attack"></param>
    /// <returns></returns>
    public PowerPanelData GetData(MagicAttack attack)
    {
        Type type = attack.GetType();

        if (type == typeof(FireAttack))
            return GetData(Constants.PANEL_FIRE);

        return null;
    }

    #endregion

    #endregion

}
