using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    public SkeletonBaseState CurrentState;

    private void Awake()
    {
        var skeletonStateFactory = new SkeletonStateFactory( this );
        CurrentState = skeletonStateFactory.SelectState( SkeletonStateFactory.States.Idle );
    }

    private void Start()
    {
        CurrentState.EnterState();
    }

    private void Update()
    {
        CurrentState.UpdateState();
    }
}
