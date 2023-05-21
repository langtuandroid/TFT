using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ConfirmationPanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;

    private Action OnConfirmation;

    private void Awake()
    {
        Hide();
        _noButton.onClick.AddListener( () => Hide() );
    }

    public void Show( Action onConfirmation )
    {
        gameObject.SetActive( true );
        _yesButton.onClick.AddListener( () => onConfirmation?.Invoke() );
    }

    private void Hide()
    {
        gameObject.SetActive( false );
        _yesButton.onClick.RemoveAllListeners();
    }
}
