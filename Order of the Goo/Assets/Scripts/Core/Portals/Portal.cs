using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (other.CompareTag("Player")) {
            Teleport();
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other) {
    }

    protected IEnumerator Transition() { 
        DontDestroyOnLoad(gameObject);
        yield return SceneManager.LoadSceneAsync(sceneToLoad);
        var portal = GetOtherPortal(); 
        UpdatePlayer(portal);
        Destroy(gameObject); 
    }

    protected void UpdatePlayer(Portal portal) {
        var player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = portal.Spawnpoint.position;
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