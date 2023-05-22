using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ConfirmationPanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;

    private Action OnCancel;

    private void Awake()
    {
        Hide();
        _noButton.onClick.AddListener( () => Hide() );
    }

    public void Show( Action onConfirmation , Action onCancel )
    {
        gameObject.SetActive( true );
        _noButton.Select();
        ServiceLocator.GetService<GameInputs>().OnCancelPerformed += GameInputs_OnCancelPerformed;
        _yesButton.onClick.AddListener( () => onConfirmation?.Invoke() );
        OnCancel = onCancel;
    }

    private void Hide()
    {
        gameObject.SetActive( false );
        ServiceLocator.GetService<GameInputs>().OnCancelPerformed -= GameInputs_OnCancelPerformed;
        _yesButton.onClick.RemoveAllListeners();
        OnCancel?.Invoke();
    }

    private void GameInputs_OnCancelPerformed()
    {
        if ( gameObject.activeSelf )
            Hide();
    }
}
