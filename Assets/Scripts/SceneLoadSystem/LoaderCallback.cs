using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneLoadSystem
{
    public class LoaderCallback : MonoBehaviour
    {
        private void Start() => StartCoroutine(AsyncSceneLoading());

        private IEnumerator AsyncSceneLoading()
        {
            AsyncOperation asyncLoadScene = SceneManager.LoadSceneAsync(SceneLoader.SceneToLoad);
            asyncLoadScene.allowSceneActivation = false;

            while (!asyncLoadScene.isDone)
            {
                if (asyncLoadScene.progress >= 0.9f)
                    asyncLoadScene.allowSceneActivation = true;

                yield return null;
            }
        }
    }
}