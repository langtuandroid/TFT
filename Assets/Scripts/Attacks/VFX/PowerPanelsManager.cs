using Scriptable;
using UnityEngine;
using Utils;
using Attack;
using System;
using UnityEngine.UI;
using System.Collections;

public class PowerPanelsManager : MonoBehaviour
{

    #region SerializeFields

    [SerializeField]
    [Tooltip("Lista con los datos de los distintos paneles")]
    private PowerPanelDataListScriptable _powerPanelList;

    [SerializeField]
    [Tooltip("Panel con el efecto de ataque")]
    private Image _panel;

    [SerializeField]
    private Image _maxPowerIcon;

    #endregion

    #region Public variables

    public static PowerPanelsManager Instance { get; private set; }
    public PowerPanelDataListScriptable PowerPanelList => _powerPanelList;

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
            Initialize();
        }
        else
            Destroy(gameObject);

    }

    private void Start()
    {
        _magicEvents.OnMaxPowerValueChange += OnMaxPowerValueChange;
    }

    private void OnDestroy()
    {
        _magicEvents.OnMaxPowerValueChange -= OnMaxPowerValueChange;
    }

    #endregion

    #region Private methods

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

    private void OnMaxPowerValueChange(MaxPowerValues values)
    {
        _maxPowerIcon.fillAmount = values.FillAmount;
    }

    #endregion


    #region Public methods

    #region Panel values

    public void ChangePanelColor(IAttack attack)
    {
        _panel.color = GetData(attack).Color;
    }

    public float GetAlpha()
    {
        return _panel.color.a;
    }

    public void SetAlpha(float alpha)
    {
        _panel.SetImageAlpha(alpha);
    }

    #region Coroutines



    #endregion

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
    public PowerPanelData GetData(IAttack attack)
    {
        Type type = attack.GetType();

        if (type == typeof(FireAttack))
            return GetData(Constants.PANEL_FIRE);

        return null;
    }

    #endregion

    #endregion

}
