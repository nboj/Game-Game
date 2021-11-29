using UnityEngine; 

public class DontDestroyOnLoad : MonoBehaviour {
    private void Awake() {
        DontDestroyOnLoad[] objects = FindObjectsOfType<DontDestroyOnLoad>();
        if (objects.Length > 1) {
            Destroy(gameObject);
        } else { 
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }
}