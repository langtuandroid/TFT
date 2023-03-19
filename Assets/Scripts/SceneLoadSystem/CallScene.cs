using UnityEngine;

namespace SceneLoadSystem
{
    public class CallScene : MonoBehaviour
    {
        [SerializeField] private string _sceneToLoadName;

        /// <summary>
        /// Load the Scene selected in the inspector
        /// </summary>
        public void LoadScene()
        {
            SceneLoader.Load(_sceneToLoadName);
        }
        
        /// <summary>
        /// Load the Scene pass as argument
        /// </summary>
        public void LoadScene( string sceneToLoad )
        {
            SceneLoader.Load( sceneToLoad );
        }
    }
}