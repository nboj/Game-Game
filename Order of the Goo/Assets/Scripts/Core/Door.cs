using UnityEngine; 
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; 

public class Door : MonoBehaviour {
    [SerializeField] string sceneToLoad;
    [SerializeField] bool sendBack;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool canTravel;
    private Player player; 

    private void Awake() { 
        spriteRenderer =  GetComponent<SpriteRenderer>();
        player = FindObjectOfType<Player>();
        canTravel = false;
        if (sendBack) {
            player.transform.position = transform.position;
        }
        animator = GetComponent<Animator>(); 
    } 

    private void OnTriggerEnter2D(Collider2D collider) {
        canTravel = true;
        spriteRenderer.color = Color.yellow;
        animator.SetBool("isOpen", true);
    }

    private void OnTriggerExit2D(Collider2D collider) {
        canTravel = false;
        spriteRenderer.color = Color.white;
        animator.SetBool("isOpen", false);
    } 

    public void Travel() {
        if ((sceneToLoad.Length > 0 || sendBack) && canTravel) {  
            if (sendBack) {
                // SceneManager.LoadScene(player.LastScene);
                // player.CanTeleport = true;
            } else { 
                // player.OldPos = player.transform.position;
                SceneManager.LoadScene(sceneToLoad);  
            }
        } else {
            Debug.Log("Error: Exception in Travel(); No scene to load or cannot travel (from Door)");
        }
    }  
}