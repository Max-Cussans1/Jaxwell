using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : Elements
{
    BoxCollider2D p_collider;
    GameObject player;
    BoxCollider2D player_collider;
    PlayerState playerstate;

    float rangeToCollide;

    //change to the element we need in editor
    public elements element = 0;

    // Start is called before the first frame update
    void Start()
    {
        //get platforms collider
        p_collider = GetComponent<BoxCollider2D>();

        //get player components
        player = GameObject.Find("TestPlayer");
        playerstate = player.GetComponent<PlayerState>();
        player_collider = player.GetComponent<BoxCollider2D>();

        //check if our platform is bigger in x or y direction
        //might want to change this in future to use a function that checks x and y positions separately
        if (p_collider.size.x > p_collider.size.y)
        {
            rangeToCollide = p_collider.size.x + 2 * player_collider.size.x;
        }
        else
        {
            rangeToCollide = p_collider.size.y + 2 * player_collider.size.y;
        }
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
        //using sqrmagnitude saves a square root call which can be expensive
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
