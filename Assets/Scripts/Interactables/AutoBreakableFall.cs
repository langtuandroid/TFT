using UnityEngine;

public class AutoBreakableFall : MonoBehaviour
{
    private void OnTriggerEnter2D( Collider2D collision )
    {
        collision.GetComponent<Player.PlayerController>().Fall();
    }
}
