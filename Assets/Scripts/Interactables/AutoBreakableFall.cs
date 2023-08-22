using UnityEngine;

public class AutoBreakableFall : MonoBehaviour
{
    private void OnTriggerEnter2D( Collider2D collision )
    {
        float yOffset = 0.5f + 1f / 16 * 4;
        Vector2 centerPosition = new Vector2( transform.position.x , transform.position.y - yOffset );
        collision.GetComponent<Player.PlayerController>().Fall( centerPosition );
        //collision.transform.position = centerPosition;
    }
}
