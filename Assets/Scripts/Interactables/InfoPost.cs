// ************ @autor: �lvaro Repiso Romero *************

using System;
using UnityEngine;

public class InfoPost : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _exclamationIcon;
    [SerializeField] private string _infoPostMessage;
    private Action onStopInteracting;

    public void Interact( Vector2 lookDirection )
    {
        if (lookDirection.y > 0)
        {
            MyDialogueManager.Instance.Text(_infoPostMessage);
        }
    }

    public void ShowCanInteract( bool show )
    {
        _exclamationIcon.SetActive( show );
    }
}