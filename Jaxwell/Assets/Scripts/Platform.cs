using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : Elements
{
    BoxCollider2D p_collider;
    GameObject player;
    PlayerState playerstate;

    float rangeToCollide = 1.5f;

    //change to the element we need in editor
    public elements element = 0;

    // Start is called before the first frame update
    void Start()
    {
        p_collider = GetComponent<BoxCollider2D>();
        player = GameObject.Find("TestPlayer");
        playerstate = player.GetComponent<PlayerState>();
    }

    // Update is called once per frame
    void Update()
    {
        //check if the player is close
        if(DistanceCheck(player.transform.position, transform.position, rangeToCollide))
        {
            //check if we're the same element, disable collision if not
            if (ElementCheck(element, playerstate.element))
            {
                p_collider.enabled = true;
                Debug.Log(this.gameObject + " had collider enabled because player is the same element and in range");
            }
            else
            {
                p_collider.enabled = false;
                Debug.Log(this.gameObject + " had collider disabled because player is a different element and in range");
            }
        }

    }

    //returns true if in range
    bool DistanceCheck(Vector2 position, Vector2 otherPosition, float distance)
    {
        //saves a square root call which can be expensive
        if ((otherPosition - position).sqrMagnitude < distance * distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
