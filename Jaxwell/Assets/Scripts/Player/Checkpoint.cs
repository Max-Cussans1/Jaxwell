using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    PlayerState player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerState>();        
    }

    
    void OnTriggerEnter2D(Collider2D other)
    {
        //check if whatever we are hitting isn't null
        if (other.gameObject != null)
        {
            if (other.gameObject == player.gameObject)
            {                
                player.currentCheckpoint = transform.position;
                DebugHelper.Log("New save at " + transform.position);
                player.Save();
            }
        }
    }
}
