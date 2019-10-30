using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    //time before we clean up projectiles (in seconds)
    public float projectileCleanupTime = 3.0f;

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
    }
}
