using UnityEngine;

public class BridgeAppear : MonoBehaviour
{
    [SerializeField][Range(1, 10)] private int numOfActivations;
    private int activationCount = 0;

    public void Appear()
    {
        activationCount++;
        if ( activationCount == numOfActivations )
            gameObject.SetActive( true );
    }
    
    public void Disappear()
    {
        activationCount--;
        if ( activationCount != numOfActivations )
            gameObject.SetActive( false );
    }
}
