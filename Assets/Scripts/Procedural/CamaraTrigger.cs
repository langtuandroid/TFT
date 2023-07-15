using UnityEngine;
using DG.Tweening;

public class CamaraTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D( Collider2D collision )
    {
        Vector3 roomCenter = new Vector3( transform.position.x, transform.position.y, Camera.main.transform.position.z );
        Camera.main.transform.DOMove( roomCenter , 0.5f ).Play();
    }
}
