using UnityEngine;

public class Door : ActivableSceneObject
{
    [SerializeField][Range(1, 5)] 
    private int numberOfActivationsNeeded = 1;
    private int activationsCount;

    public void OpenDoor()
    {
        activationsCount++;
        if ( numberOfActivationsNeeded == activationsCount )
        {
            gameObject.SetActive( false );
            _hasBeenActivated = true;
        }
    }
    
    public void CloseDoor()
    {
        activationsCount--;
        gameObject.SetActive( true );
        _hasBeenActivated = false;
    }

    public override void TriggerActivation()
    {
        base.TriggerActivation();
        activationsCount = numberOfActivationsNeeded;
        gameObject.SetActive( false );
    }
}
