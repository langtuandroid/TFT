using UnityEngine;
using DG.Tweening;
using TMPro;

public class SoulsUI : MonoBehaviour
{
    #region SerializeFields

    [Header("Scriptable Objects")]
    [SerializeField]
    [Tooltip("Datos del jugador")]
    private PlayerStatusSaveSO _playerStatusSaveSO;

    [Header("Canvas")]
    [SerializeField]
    [Tooltip("CanvasGroup de las almas")]
    private CanvasGroup _soulsCanvas;
    [SerializeField]
    [Tooltip("Texto con la cantidad de almas")]
    private TMP_Text _soulsText;

    [Header("Settings")]
    [SerializeField]
    private float _changeDuration = 1f;

    #endregion

    #region Private variables

    // EVENTS
    private SoulEvents _soulEvent; // Evento de almas

    // VARIABLES
    private Tween _tween;

    #endregion

    #region Unity methods

    private void Start()
    {
        // Events
        _soulEvent = ServiceLocator.GetService<SoulEvents>();
        _soulEvent.OnGotSoulsValue += OnGotSouls;

        // Initialization
        OnGotSouls(_playerStatusSaveSO.playerStatusSave.currentSouls);
    }

    private void OnDestroy()
    {
        _soulEvent.OnGotSoulsValue -= OnGotSouls;
    }

    #endregion

    #region Private methods

    private void OnGotSouls(int quantity)
    {
        if (_tween != null)
            _tween.Kill();

        // Actualizamos el texto
        UpdateSoulText(quantity);

        Sequence seq = DOTween.Sequence();

        seq.Append(ChangeCanvasAlpha(1f));
        seq.AppendInterval(2f);
        seq.OnComplete(() => ChangeCanvasAlpha(0f).Play());

        seq.Play();

        _tween = seq;
    }

    /// <summary>
    /// Cambia el alpha del CanvasGroup
    /// </summary>
    /// <param name="alpha"></param>
    /// <returns></returns>
    private Tween ChangeCanvasAlpha(float alpha)
    {
        return DOTween.To(
            () => _soulsCanvas.alpha,
            x => _soulsCanvas.alpha = x,
            alpha,
            _changeDuration)
            .SetEase(Ease.Linear);
    }

    /// <summary>
    /// Actualiza el texto de las almas
    /// </summary>
    /// <param name="quantity"></param>
    private void UpdateSoulText(int quantity)
    {
        string value = _soulsText.text.Substring(2);
        int val = int.Parse(value) + quantity;

        val = Mathf.Min(_playerStatusSaveSO.playerStatusSave.maxSouls, val);

        _soulsText.text = $"x {val}";
    }


    #endregion

}
