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
    float startX;
    float startY;

    [SerializeField] bool oscillateVertically = false;
    [SerializeField] bool oscillateUpwardsFirst = false;
    [SerializeField] bool oscillateHorizontally = false;
    [SerializeField] bool oscillateRightFirst = false;
    [SerializeField] float verticalOscillationDistance = 5.0f;
    [SerializeField] float verticalOscillationSpeed = 0.05f;
    [SerializeField] float verticalOscillationPauseTime = 3.0f;
    [SerializeField] float horizontalOscillationDistance = 5.0f;
    [SerializeField] float horizontalOscillationSpeed = 0.05f;
    [SerializeField] float horizontalOscillationPauseTime = 3.0f;

    float tempXOscillationPauseTime;
    float tempYOscillationPauseTime;

    //change to the element we need in editor
    public elements element = 0;

    // Start is called before the first frame update
    void Start()
    {
        startX = transform.position.x;
        startY = transform.position.y;

        tempYOscillationPauseTime = verticalOscillationPauseTime;
        tempXOscillationPauseTime = horizontalOscillationPauseTime;

        //get platforms collider
        p_collider = GetComponent<BoxCollider2D>();

        //get player components
        player = GameObject.Find("Player");
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
                DebugHelper.Log(this.gameObject + " had collider enabled because player is the same element and in range");
            }
            else
            {
                p_collider.enabled = false;
                DebugHelper.Log(this.gameObject + " had collider disabled because player is a different element and in range");
            }
        }
    }

    void FixedUpdate()
    {
        if(oscillateVertically)
        {
            oscillateY(ref oscillateUpwardsFirst, startY, verticalOscillationSpeed, ref tempYOscillationPauseTime);
        }
        if(oscillateHorizontally)
        {
            oscillateX(ref oscillateRightFirst, startX, horizontalOscillationSpeed, ref tempXOscillationPauseTime);
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

    void oscillateY(ref bool startDirection, float start, float speed, ref float delay)
    {
        //if the start direction is true (up) and we're not at the max position
        if(startDirection && transform.position.y < start + verticalOscillationDistance)
        {
            //check if the position change will be above the max distance
            if (!(transform.position.y + speed > start + verticalOscillationDistance))
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + speed);
            }
            else
            {
                delay -= Time.deltaTime;
                if(delay < 0)
                {
                    startDirection = false;
                    delay = verticalOscillationPauseTime;
                }
            }
        }

        if(!startDirection && transform.position.y > start - verticalOscillationDistance)
        {
            //check if the position change will be below the negative max distance
            if (!(transform.position.y - speed < start - verticalOscillationDistance))
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - speed);
            }
            else
            {
                delay -= Time.deltaTime;
                if (delay < 0)
                {
                    startDirection = true;
                    delay = verticalOscillationPauseTime;
                }
            }
        }
    }

    void oscillateX(ref bool startDirection, float start, float speed, ref float delay)
    {
        //if the start direction is true (right) and we're not at the max position
        if (startDirection && transform.position.x < start + horizontalOscillationDistance)
        {
            //check if the position change will be above the max distance
            if (!(transform.position.x + speed > start + horizontalOscillationDistance))
            {
                transform.position = new Vector2(transform.position.x + speed, transform.position.y);
            }
            else
            {
                delay -= Time.deltaTime;
                if (delay < 0)
                {
                    startDirection = false;
                    delay = horizontalOscillationPauseTime;
                    DebugHelper.Log("x position = " + transform.position.x + " distance = " + (start - horizontalOscillationDistance));
                }
            }
        }

        if (!startDirection && transform.position.x > start - horizontalOscillationDistance)
        {
            //check if the position change will be below the negative max distance
            if (!(transform.position.x - speed < start - horizontalOscillationDistance))
            {
                transform.position = new Vector2(transform.position.x - speed, transform.position.y);
            }
            else
            {
                delay -= Time.deltaTime;
                if (delay < 0)
                {
                    startDirection = true;
                    delay = horizontalOscillationPauseTime;
                    DebugHelper.Log("x position = " + transform.position.x + " distance = " + (start + horizontalOscillationDistance));
                }
            }
        }
    }

}
