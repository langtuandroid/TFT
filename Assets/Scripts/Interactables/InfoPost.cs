using UnityEngine;

public class InfoPost : MonoBehaviour, IInteractable
{
    [SerializeField] private string InfoPostMessage;

    public void Interact()
    {
        Debug.Log( InfoPostMessage );
    }
}
