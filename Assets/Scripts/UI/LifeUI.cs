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
    private Sprite _spriteFullHeart;


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
    // Valor de la salud actual
    private int _currentHealth;

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
        for (
            int i = 0;
            i < _playerStatusSaveSO.playerStatusSave.maxHealth / 2;
            i++
            )
            OnAddHeart();

        OnChangeLifeInstantly(_playerStatusSaveSO.playerStatusSave.currentHealth);
    }

    #endregion

    private void OnAddHeart()
    {
        GameObject heart = Instantiate(
            _heartPrefab,
            _heartSection.transform
            );

        _heartList.Add(heart.GetComponent<Image>());
        UpdateSpacing();
    }

    /// <summary>
    /// Actualiza la salud (visualmente) de forma instantánea
    /// </summary>
    /// <param name="life"></param>
    private void OnChangeLifeInstantly(int life)
    {
        int div = life / 2;

        for (int i = 0; i < div; i++)
            _heartList[i].sprite = _spriteFullHeart;

        if (life % 2 == 1)
            _heartList[div].sprite = _spriteMediumHeart;

        _currentHealth = life;
    }

    /// <summary>
    /// Actualiza la salud
    /// </summary>
    /// <param name="life"></param>
    private void OnChangeLife(int life)
    {
        // Detenemos corrutinas (en caso de haberlas)
        StopAllCoroutines();
        // Comenzamos la corrutina
        StartCoroutine(ChangeLife(life));
    }

    /// <summary>
    /// Cambia el valor de la vida poco a poco
    /// </summary>
    /// <param name="life"></param>
    /// <returns></returns>
    private IEnumerator ChangeLife(int life)
    {
        if (life < _currentHealth)
            for (int i = _currentHealth; i >= life; i--)
                yield return UpdateLife(i);
        else
            for (int i = _currentHealth; i <= life; i++)
                yield return UpdateLife(i);

        // Si la vida llega a 0
        if (_currentHealth == 0)
            // Morimos
            _lifeEvents.OnDeath();
    }

    private IEnumerator UpdateLife(int life)
    {
        int div = life / 2;
        int res = life % 2;

        // Si tenemos algo de vida
        if (div + res > 0)
        {
            // Si tenemos un número impar
            if (res == 1)
                // Ponemos medio corazón
                _heartList[div].sprite = _spriteMediumHeart;
            // En otro caso
            else
            {
                if (div < _playerStatusSaveSO.playerStatusSave.maxHealth / 2)
                    _heartList[div].sprite = _spriteEmptyHeart;

                _heartList[div - 1].sprite = _spriteFullHeart;
            }
        }
        // En caso de no tener vida
        else
            // Vaciamos el corazón
            _heartList[0].sprite = _spriteEmptyHeart;

        // Actualizamos la última vida
        _currentHealth = life;

        yield return new WaitForSeconds(0.2f);
    }

    /// <summary>
    /// Establece el espacio correspondiente según la cantidad de corazones
    /// </summary>
    private void UpdateSpacing()
    {
        _verticalLayout.spacing = SPACING + SPACING / 2 * ((_heartList.Count - 1) / 8);
    }


}
