using UnityEngine;

public class ChangeSceneTest : MonoBehaviour
{
    private void OnTriggerEnter2D( Collider2D collision )
    {
        if ( collision.transform.tag.Equals( "Player" ) )
        {
            ServiceLocator.GetService<SceneLoader>().Load( "ProceduralTest" );
        }
    }
}
