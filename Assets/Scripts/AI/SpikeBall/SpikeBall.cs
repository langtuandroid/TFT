using UnityEngine;

public class SpikeBall : MonoBehaviour
{
    private FsmSpikeBall _actualState;
    public FsmSpikeBall ActualState
    {
        get => _actualState;
        set => _actualState = value;
    }

    public float AttackVelocity;
    public float ReturnVelocity;
    public Vector2 InitialPosition;
    public float DetectionRadius;
    public float PatrolDetectionRadius;
    
    private void Start()
    {
        ActualState = new SpikeBallAlertState();
        InitialPosition = transform.position;
    }
    
    private void Update()
    {
        ActualState.Execute(this);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, DetectionRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, PatrolDetectionRadius);
    }
}
