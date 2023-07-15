using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineConfiner2D _confiner;
    [SerializeField] private Collider2D _colliderToSwitch;

    private void OnTriggerEnter2D( Collider2D collision )
    {
        if ( _confiner.m_BoundingShape2D != _colliderToSwitch )
        {
            _confiner.m_BoundingShape2D = _colliderToSwitch;
            _confiner.InvalidateCache();
        }
    }
}
