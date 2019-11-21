using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growable : MonoBehaviour
{
    //how much it will grow
    public float growHeight = 3.0f;
    //how many projectiles it will take to grow
    public int projectilesToGrow = 5;
    //how many hits it's taken
    public int hits;

    BoxCollider2D growable_collider;

    //bool to track when we are grown
    bool isGrown = false;

    void Start()
    {
        growable_collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //if it's not already grown
        if (!isGrown)
        {
            //if the number of projectiles hit has reached the goal
            if (hits >= projectilesToGrow)
            {
                //increase the collider size to what has been specified in projectilesToGrow
                growable_collider.size = new Vector2(growable_collider.size.x, growable_collider.size.y + growHeight);
                isGrown = true;
                Debug.Log("Growable grown " + this.transform.position + " to have a height of " + growable_collider.size.y);
            }
        }
    }
}
