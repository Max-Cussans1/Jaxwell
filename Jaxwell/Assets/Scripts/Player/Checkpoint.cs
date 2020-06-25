using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Checkpoint : MonoBehaviour
{
    PlayerState player;

    [SerializeField] AudioClip checkpointPickupSFX;


    public Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(checkpointPickupSFX, "Checkpoint Pickup SFX was null, ensure a sound is assigned to the checkpoint script");
        player = FindObjectOfType<PlayerState>();
        position = transform.position;
    }

    
    void OnTriggerEnter2D(Collider2D other)
    {
        //check if whatever we are hitting isn't null
        if (other.gameObject != null)
        {
            if (other.gameObject == player.gameObject)
            {
                if (Health.currentCheckpoint != position)
                {
                    AudioManager.instance.PlaySFX(checkpointPickupSFX);
                    //use variable in health script to handle checkpoints because that's where we're handling respawning
                    Health.currentCheckpoint = position;
                    //player.currentCheckpointSave = position;
                    DebugHelper.Log("New checkpoint at " + position);
                }
            }
        }
    }
}
