using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    PlayerScript player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerScript>();        
    }

    
    void OnTriggerEnter2D(Collider2D other)
    {
        //check if whatever we are hitting isn't null
        if (other.gameObject != null)
        {
            if (other.gameObject == player.gameObject)
            {                
                player.currentCheckpoint = transform.position;
                Debug.Log("New checkpoint at " + transform.position);
            }
        }
    }
}
