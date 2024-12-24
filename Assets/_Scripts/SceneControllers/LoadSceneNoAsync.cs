using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneControllers
{
    public class LoadSceneNoAsync : MonoBehaviour
    {
        [SerializeField] private string _sceneNameToLoad;

        public void _LoadScene()
        {
            SceneManager.LoadScene(_sceneNameToLoad);
        }
    }
}
