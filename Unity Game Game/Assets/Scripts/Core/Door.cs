using System;
using UnityEngine; 
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using RPG.Control;

public class Door : MonoBehaviour {
    private SpriteRenderer spriteRenderer;
    [SerializeField] string sceneToLoad;
    private bool canTravel;
    private PlayerController player;

    private void Start() {
        spriteRenderer =  GetComponent<SpriteRenderer>();
        player = GetComponent<PlayerController>();
        canTravel = false;
    }  

    private void OnTriggerEnter2D(Collider2D collider) {
        canTravel = true;
        spriteRenderer.color = Color.yellow;
    }

    private void OnTriggerExit2D(Collider2D collider) {
        canTravel = false;
        spriteRenderer.color = Color.white;
    } 

    public void Travel() {
        if (sceneToLoad.Length > 0 && canTravel) {  
            SceneManager.LoadScene(sceneToLoad);  
        } else {
            Debug.Log("Error: Exception in Travel(); No scene to load or cannot travel (from Door)");
        }
    } 
}