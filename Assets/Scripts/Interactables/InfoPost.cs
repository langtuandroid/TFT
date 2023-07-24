// ************ @autor: �lvaro Repiso Romero *************

using System;
using Honeti;
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
            MyDialogueManager.Instance.Text(I18N.instance.getValue("^infopost_S10_Z0_0"));
        }
    }

    public void ShowCanInteract( bool show )
    {
        _exclamationIcon.SetActive( show );
    }
}