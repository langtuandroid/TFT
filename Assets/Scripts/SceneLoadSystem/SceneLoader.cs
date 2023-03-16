using UnityEngine.SceneManagement;

namespace SceneLoadSystem
{
    public static class SceneLoader
    {
        public const string LOADING_SCENE_NAME = "LoadingScene";
        public static string SceneToLoad => _targetScene;

        private static string _targetScene;

        public static void Load(string targetSceneName)
        {
            _targetScene = targetSceneName;
            SceneManager.LoadScene(LOADING_SCENE_NAME);
        }
    }
}