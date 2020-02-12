using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    PlayerState player;

    public bool dead = false;

    public int health = 100;
    int initialHealth;

    void Start()
    {
        player = FindObjectOfType<PlayerState>();
        initialHealth = health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        DebugHelper.Log(this.gameObject + " took damage and is at " + health + " health");
        if (health <= 0)
        {
            dead = true;
            DebugHelper.Log(this.gameObject + " died at " + transform.position + "!");
            //DEAD STUFF
            Respawn(player.currentCheckpoint);
        }
    }

    void ResetHealth()
    {        
        health = initialHealth;
        dead = false;

        DebugHelper.Log(this.gameObject + " reset health");
    }

    void Respawn(Vector3 respawnPosition)
    {
        //TODO: ADD ELEMENT WE WANT TO RESPAWN AS

        //reset health and move player to respawn location
        ResetHealth();
        player.transform.position = respawnPosition;

        DebugHelper.Log(this.gameObject + " respawned at " + respawnPosition);
    }
}
