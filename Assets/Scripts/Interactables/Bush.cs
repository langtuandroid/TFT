using UnityEngine;

public class Bush : MonoBehaviour, IBurnable
{
    public void Burn(int damage)
    {
        gameObject.SetActive(false);
    }
}
