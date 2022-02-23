using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControl : MonoBehaviour
{
    private Player player;
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    public void TeleportPlayerTo(string vector2) {
        string[] floatStrings = vector2.Split(",");
        float[] floats = new float[floatStrings.Length];
        for (var i = 0; i < floatStrings.Length; i++) {
            floats[i] = float.Parse(floatStrings[i]);
        }
        if (floats.Length == 2) {
            player.transform.position = new Vector2(floats[0], floats[1]); 
        } else {
            Debug.Log("Invalid format in scene control TeleportPlayerTo");
        }
    }

    public void EnablePlayer() {
        player.Enable(true);
    }

    public void DisablePlayer() {
        player.Disable();
    }

    public void DisablePlayerRenderer() {
        player.GetComponent<SpriteRenderer>().enabled = false;
    }
    
    public void EnablePlayerRenderer() {
        player.GetComponentInChildren<SpriteRenderer>().enabled = true;
    }

    public void AddItemToPlayerInventory(Entity entity) {

    }
}
