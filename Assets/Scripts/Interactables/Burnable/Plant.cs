using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour, IBurnable, IInteractable
{
    public void Burn()
    {
        throw new System.NotImplementedException();
    }

    public void Interact(Vector2 lookDirection)
    {
        throw new System.NotImplementedException();
    }

    public void ShowCanInteract(bool show)
    {
        throw new System.NotImplementedException();
    }
}
