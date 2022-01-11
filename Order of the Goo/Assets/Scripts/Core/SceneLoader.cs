using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.Saving;

namespace RPG.Core {
    public class SceneLoader : MonoBehaviour {
        [SerializeField] private bool loadNextOnAwake = false; 
        private void Awake() { 
            if (loadNextOnAwake) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        public void LoadNextScene() {
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex+1);
        }

        public void LoadScene(string sceneName) {
            SceneManager.LoadScene(sceneName);
        }

        public void LoadScene(int sceneIndex) {
            SceneManager.LoadScene(sceneIndex);
        } 
    }
}
