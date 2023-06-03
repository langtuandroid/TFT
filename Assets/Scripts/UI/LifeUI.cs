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
    [Tooltip("Secci�n contenedora de los corazones")]
    [SerializeField]
    private GameObject _heartSection;

    [Header("Life scriptable object")]
    [Tooltip("Scriptable object de la vida")]
    [SerializeField]
    private PlayerStatusSaveSO _playerStatusSaveSO;

    [Header("Heart prefab")]
    [Tooltip("Prefab del coraz�n")]
    [SerializeField]
    private GameObject _heartPrefab;

    [Header("Sprites")]
    [Tooltip("Sprite del coraz�n vac�o")]
    [SerializeField]
    private Sprite _spriteEmptyHeart;
    [Tooltip("Sprite de coraz�n por la mitad")]
    [SerializeField]
    private Sprite _spriteMediumHeart;
    [Tooltip("Sprite de coraz�n lleno")]
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

        // INICIALIZACI�N DE VARIABLES
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
    /// Actualiza la salud (visualmente) de forma instant�nea
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
        StartCoroutine(ChangeLife(life));
        _currentHealth = life;
    }


    private IEnumerator ChangeLife(int life)
    {
        if (life < _currentHealth)
            for (int i = _currentHealth; i >= life; i--)
                yield return UpdateLife(i);
        else
            for (int i = _currentHealth; i <= life; i++)
                yield return UpdateLife(i);
    }

    private IEnumerator UpdateLife(int life)
    {
        int div = life / 2;
        int res = life % 2;

        // Si tenemos algo de vida
        if (div + res > 0)
        {
            // Si tenemos un n�mero impar
            if (res == 1)
                // Ponemos medio coraz�n
                _heartList[div].sprite = _spriteMediumHeart;
            // En otro caso
            else
            {
                if (div < _playerStatusSaveSO.playerStatusSave.maxHealth / 2)
                {
                    _heartList[div].sprite = _spriteEmptyHeart;
                }

                _heartList[div - 1].sprite = _spriteFullHeart;
            }
        }
        // En caso de no tener vida
        else
            // Vaciamos el coraz�n
            _heartList[0].sprite = _spriteEmptyHeart;

        yield return new WaitForSeconds(0.2f);
    }

    private void UpdateSpacing()
    {
        _verticalLayout.spacing = SPACING + SPACING / 2 * ((_heartList.Count - 1) / 8);
    }


}
