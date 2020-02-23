using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    PlayerState player;
    Health health;    

    public Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerState>();
        health = player.GetComponent<Health>();
        position = transform.position;
    }

    
    void OnTriggerEnter2D(Collider2D other)
    {
        //check if whatever we are hitting isn't null
        if (other.gameObject != null)
        {
            if (other.gameObject == player.gameObject)
            {                
                //use variable in health script to handle checkpoints because that's where we're handling respawning
                health.currentCheckpoint = position;
                player.currentCheckpointSave = position;
                DebugHelper.Log("New checkpoint at " + position);
                if (SaveManager.currentSavePath != null)
                {
                    player.Save();
                }
            }
        }
    }
}
