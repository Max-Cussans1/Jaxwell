using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVolume : MonoBehaviour
{
    PlayerState player;
    Health playerHealth;

    [SerializeField] int damage = 100;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerState>();
        playerHealth = player.GetComponent<Health>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        //check if whatever we are hitting isn't null
        if (other.gameObject != null)
        {
            if (other.gameObject == player.gameObject)
            {                
                playerHealth.TakeDamage(damage);
                DebugHelper.Log("Player took " + damage + " damage from " + this.gameObject);
            }
        }
    }
}
