using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    //health for destructible
    public int health = 50;
    //time for the object to be destroyed once it hits 0 health
    public float timeToDestroy = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            timeToDestroy -= Time.deltaTime;
            if (timeToDestroy <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

}
