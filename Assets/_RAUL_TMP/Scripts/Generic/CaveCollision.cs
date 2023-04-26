using UnityEngine;

public class CaveCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            Level1Manager.instance.GotoCave();
              
    }
}
