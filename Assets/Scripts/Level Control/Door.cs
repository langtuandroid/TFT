using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField][Range(1, 5)] 
    private int numberOfActivationsNeeded = 1;
    private int activationsCount;

    public void OpenDoor()
    {
        activationsCount++;
        if ( numberOfActivationsNeeded == activationsCount )
            gameObject.SetActive( false );
    }
    
    public void CloseDoor()
    {
        activationsCount--;
        gameObject.SetActive( true );
    }
}
