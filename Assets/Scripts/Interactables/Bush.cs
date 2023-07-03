using UnityEngine;

public class Bush : MonoBehaviour, IBurnable
{
    public void Burn()
    {
        gameObject.SetActive( false );
    }
}
