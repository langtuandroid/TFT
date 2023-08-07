using UnityEngine.SceneManagement;

public class SceneLoader
{
    public string SceneToLoad => _targetScene;

    private string _targetScene;

    public SceneLoader()
    {
        SceneManager.LoadScene( "S04_InstanceScene" , LoadSceneMode.Additive );
    }

    public void Load( string targetSceneName )
    {
        _targetScene = targetSceneName;
        SceneManager.LoadScene( SceneName.S01_LoadingScene.ToString() );
    }

    public void InstaLoad( string targetSceneName )
    {
        SceneManager.LoadScene( targetSceneName );
    }
}