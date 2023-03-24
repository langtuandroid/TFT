using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneLoadSystem
{
    public class LoaderCallback : MonoBehaviour
    {
        private float _secondsLoading = 1f;

        private void Start() => StartCoroutine(AsyncSceneLoading());

        private IEnumerator AsyncSceneLoading()
        {
            AsyncOperation asyncLoadScene = SceneManager.LoadSceneAsync(SceneLoader.SceneToLoad);
            asyncLoadScene.allowSceneActivation = false;
            bool isLoaded = false;

            while ( !isLoaded )
            {
                while ( _secondsLoading > 0 )
                {
                    _secondsLoading -= Time.deltaTime;

                    if ( asyncLoadScene.progress >= 0.9f )
                        isLoaded = true;

                    yield return null;
                }
            }
            asyncLoadScene.allowSceneActivation = true;
        }
    }
}