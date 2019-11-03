using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    //time before we clean up projectiles (in seconds)
    public float projectileCleanupTime = 3.0f;

    public bool fire = false;
    public bool water = false;
    public bool earth = false;
    public bool air = false;

    Growable growable;

    // Use this for initialization
    void Awake()
    {
        //destroy projectile after a certain amount of time
        Destroy(this.gameObject, projectileCleanupTime);
    }

    //do stuff if the projectile passes into something else's collision
    void OnTriggerEnter2D(Collider2D other)
    {
        //check if whatever we are hitting does have a parent otherwise we get null reference exceptions when they don't
        if (other.gameObject.transform.parent != null)
        {
            //check if they're an enemy
            if (other.gameObject.transform.parent.CompareTag("Enemy"))
            {
                //Destroy enemy if we hit them
                Debug.Log("Enemy destroyed: " + other + " at " + other.transform.position);
                Destroy(other.transform.parent.gameObject);                
                //Destroy the projectile if it hits an enemy
                Destroy(gameObject);
            }
        }

        //if we're water
        if (water)
        {
            //check if projectile has hit a growable
            if (other.gameObject.GetComponent<Growable>() != null)
            {
                growable = other.gameObject.GetComponent<Growable>();
                growable.hits++;
                Debug.Log("Watered a growable, now at " + growable.hits + " out of " + growable.projectilesToGrow + " needed to grow");
            }
        }
    }
}
