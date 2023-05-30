using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D( Collider2D collision )
    {
        ServiceLocator.GetService<LevelEvents>().KeyObtained();
        Destroy( gameObject );
    }
}
