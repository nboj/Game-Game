using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.Saving;

public class Portal : MonoBehaviour, IFHandler {
    [SerializeField] private string sceneToLoad;
    [SerializeField] private PortalID portalID;
    [SerializeField] private Transform spawnpoint;

    public PortalID PID {
        get => portalID;
        set => portalID = value;
    }
    public Transform Spawnpoint => spawnpoint;

    public enum PortalID {
        A, B, C, D, E, F, G
    } 

    protected virtual void Start() {
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player Root")) {
            Teleport();
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other) {
    }

    protected IEnumerator Transition() { 
        DontDestroyOnLoad(gameObject);
        var savingSystem = FindObjectOfType<SavingSystem>();
        savingSystem.Save("Save");
        yield return SceneManager.LoadSceneAsync(sceneToLoad);
        var portal = GetOtherPortal();  
        savingSystem.Load("Save"); 
        UpdatePlayer(portal);
        savingSystem.Save("Save");
        Destroy(gameObject); 
    }

    protected void UpdatePlayer(Portal portal) {
        var player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = portal.Spawnpoint.position;
        Debug.Log("Updated Player pos");
    }

    protected Portal GetOtherPortal() { 
        var portals = FindObjectsOfType<Portal>();
        foreach (Portal portal in portals) {
            if (portal != this && portal.PID == portalID)
                return portal;
        }
        Debug.Log("PortalNotFound");
        return null;
    }

    public void Teleport() {
        transform.parent = null;
        StartCoroutine(Transition());
    }

    public void Fire() {
        Teleport();
    }
}