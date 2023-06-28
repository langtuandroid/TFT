using Player;
using UnityEngine;
using Utils;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField]
    private int _damageQuantity = 1;


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag(Constants.TAG_PLAYER))
        {
            col.gameObject.GetComponent<PlayerStatus>().TakeDamage(_damageQuantity);
        }
    }
    
}