using UnityEngine.SceneManagement;

namespace SceneLoadSystem
{
    public static class SceneLoader
    {
        public static string SceneToLoad => _targetScene;

        private static string _targetScene;

        public static void Load(string targetSceneName)
        {
            _targetScene = targetSceneName;
            SceneManager.LoadScene( SceneName.S01_LOADING );
        }
    }
}