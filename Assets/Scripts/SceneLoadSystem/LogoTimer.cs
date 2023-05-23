using UnityEngine;

public class LogoTimer : MonoBehaviour
{
    private void Start()
    {
        Invoke( nameof( ChangeScene ) , 3f );
    }

    private void ChangeScene()
    {
        ServiceLocator.GetService<SceneLoader>().InstaLoad( SceneName.S00_MainMenuScene.ToString() );
    }
}
