using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    Rigidbody2D rigidbody;

    public Vector3 currentCheckpoint;

    public bool dead = false;

    public int health = 100;
    int initialHealth;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        initialHealth = health;
        //add a safety checkpoint where the object starts
        currentCheckpoint = transform.position;
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
            Respawn(currentCheckpoint);
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
        //reset speed of the object
        rigidbody.velocity = new Vector2(0, 0);
        //reset position to the respawn position
        transform.position = respawnPosition;

        DebugHelper.Log(this.gameObject + " respawned at " + respawnPosition);
    }
}
