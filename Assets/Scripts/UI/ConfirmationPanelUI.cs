using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ConfirmationPanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;

    private void Awake()
    {
        Hide();
        _noButton.onClick.AddListener( () => Hide() );
    }

    public void Show( Action onConfirmation )
    {
        gameObject.SetActive( true );
        ServiceLocator.GetService<GameInputs>().OnCancelPerformed += GameInputs_OnCancelPerformed;
        _yesButton.onClick.AddListener( () => onConfirmation?.Invoke() );
    }

    private void Hide()
    {
        gameObject.SetActive( false );
        ServiceLocator.GetService<GameInputs>().OnCancelPerformed -= GameInputs_OnCancelPerformed;
        _yesButton.onClick.RemoveAllListeners();
    }

    private void GameInputs_OnCancelPerformed()
    {
        if ( gameObject.activeSelf )
            Hide();
    }
}
