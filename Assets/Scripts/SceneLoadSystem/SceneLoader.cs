using UnityEngine.SceneManagement;

public class SceneLoader
{
    public string SceneToLoad => _targetScene;

    private string _targetScene;

    public void Load(string targetSceneName)
    {
        _targetScene = targetSceneName;
        SceneManager.LoadScene( SceneName.S01_LOADING );
    }
}