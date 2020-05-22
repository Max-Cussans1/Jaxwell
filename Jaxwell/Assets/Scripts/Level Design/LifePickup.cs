using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(BoxCollider2D))]
public class LifePickup : MonoBehaviour
{
    public PlayerState player;
    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(player, "Player was null, ensure the Player GameObject is assigned in the life pickup options");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //check if whatever we are hitting isn't null
        if (other.gameObject != null)
        {
            //if lives are below maximum
            if (other.gameObject == player.gameObject && Health.lives < 4)
            {
                Health.lives++;
                DebugHelper.Log("Picked up a life. Lives: " + Health.lives);
                HealthUI.updateHealthUI = true;
                HealthUI.lifeGained = true;
                Destroy(this.gameObject);
            }
        }
    }

}
