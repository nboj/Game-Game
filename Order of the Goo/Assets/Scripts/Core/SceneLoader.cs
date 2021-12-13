using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Core {
    public class SceneLoader : MonoBehaviour {
        [SerializeField] private bool loadNextOnAwake = false; 
        private void Awake() { 
            if (loadNextOnAwake) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
