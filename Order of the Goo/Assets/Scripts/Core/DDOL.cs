using UnityEngine; 

public class DDOL : MonoBehaviour {
    private static DDOL instance;
    public void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}