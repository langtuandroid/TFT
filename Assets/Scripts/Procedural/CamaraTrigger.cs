using UnityEngine;
using DG.Tweening;

public class CamaraTrigger : MonoBehaviour
{
    private Cinemachine.CinemachineVirtualCamera _camera;

    public void AssignCamera( Cinemachine.CinemachineVirtualCamera virtualCamera )
    {
        _camera = virtualCamera;
    }

    private void OnTriggerEnter2D( Collider2D collision )
    {
        Vector3 roomCenter = new Vector3( transform.position.x, transform.position.y, _camera.transform.position.z );
        _camera.transform.DOMove( roomCenter , 0.5f ).Play();
    }
}
