using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeUI : MonoBehaviour
{
    #region Consts

    private const float SPACING = 16;

    #endregion

    #region SerializeFields

    [Header("Life UI part")]
    [Tooltip("Sección contenedora de los corazones")]
    [SerializeField]
    private GameObject _heartSection;

    [Header("Life scriptable object")]
    [Tooltip("Scriptable object de la vida")]
    [SerializeField]
    private PlayerStatusSaveSO _playerStatusSaveSO;

    [Header("Heart prefab")]
    [Tooltip("Prefab del corazón")]
    [SerializeField]
    private GameObject _heartPrefab;

    [Header("Sprites")]
    [Tooltip("Sprite del corazón vacío")]
    [SerializeField]
    private Sprite _spriteEmptyHeart;
    [Tooltip("Sprite de corazón por la mitad")]
    [SerializeField]
    private Sprite _spriteMediumHeart;
    [Tooltip("Sprite de corazón lleno")]
    [SerializeField]
    private Sprite _fullHeart;


    #endregion

    #region Private variables

    // SERVICES
    private LifeEvents _lifeEvents;

    // COMPONENTS
    // Vertical Layout Group
    private VerticalLayoutGroup _verticalLayout;

    // VARIABLES
    // Lista de corazones
    private List<Image> _heartList;
    private int _currentHealth; // Salud actual

    #endregion


    #region Unity methods

    private void Awake()
    {
        // SERVICIOS
        _lifeEvents = ServiceLocator.GetService<LifeEvents>();
        _lifeEvents.OnHeartsValue += OnAddHeart;
        _lifeEvents.OnCurrentLifeValue += OnChangeLife;


        // COMPONENTES
        _verticalLayout = GetComponentInChildren<VerticalLayoutGroup>();

        // INICIALIZACIÓN DE VARIABLES
        _heartList = new List<Image>();
    }

    private void Start()
    {
        Debug.Log(_playerStatusSaveSO.playerStatusSave.maxHealth);
        for (
            int i = 0;
            i < _playerStatusSaveSO.playerStatusSave.maxHealth / 2;
            i++
            )
            OnAddHeart();

    }

    #endregion


    [ContextMenu("OnAddHeart")]
    private void OnAddHeart()
    {
        GameObject heart = Instantiate(
            _heartPrefab,
            _heartSection.transform
            );

        _heartList.Add(heart.GetComponent<Image>());
        UpdateSpacing();
    }

    private void OnChangeLife(int life)
    {
        StartCoroutine(ChangeLife(life));
    }

    private IEnumerator ChangeLife(int life)
    {
        yield return null;
    }

    private void UpdateSpacing()
    {
        _verticalLayout.spacing = SPACING + SPACING / 2 * ((_heartList.Count - 1) / 8);
    }


}
