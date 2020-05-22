using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    Rigidbody2D rb;
    PlayerState player;

    public Vector3 currentCheckpoint;

    [HideInInspector]
    public bool dead = false;

    public int health = 100;
    public static int lives = 4;
    int initialHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<PlayerState>();


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
            lives--;
            DebugHelper.Log("Player died. Lives left: " + lives);
            HealthUI.updateHealthUI = true;

            if (lives > 0)
            {
            Respawn(currentCheckpoint);
            }
            else
            {
                GameOver.gameOver = true;
            }
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
        //respawn player as water for now
        player.pressedWater = true;
        //reset health and move player to respawn location
        ResetHealth();
        //reset speed of the object
        rb.velocity = new Vector2(0, 0);
        //reset position to the respawn position
        transform.position = respawnPosition;

        DebugHelper.Log(this.gameObject + " respawned at " + respawnPosition);
    }

}
