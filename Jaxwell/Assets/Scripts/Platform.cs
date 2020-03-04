using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Platform : Elements
{
    BoxCollider2D p_collider;
    GameObject player;
    BoxCollider2D player_collider;
    PlayerState playerstate;
    SpriteRenderer spriteRenderer;
    Color alphaOriginal;
    Color alphaTemp;

    public static float alphaAmount = 0.2f;

#if UNITY_EDITOR
    [Header("Debug")]
    [Tooltip("Enable to draw boxes of where the platform will oscillate")]
    [SerializeField] bool levelDesignDebug = false;
    [Space(25)]

    //variables to store the current coords to get the debug values
    Vector2 topL;
    Vector2 topR;
    Vector2 bottomL;
    Vector2 bottomR;

    //variables for debug boxes
    Vector2 vUpTopR;
    Vector2 vUpTopL;
    Vector2 vUpBottomR;
    Vector2 vUpBottomL;

    Vector2 vDownTopR;
    Vector2 vDownTopL;
    Vector2 vDownBottomR;
    Vector2 vDownBottomL;

    Vector2 hLeftTopR;
    Vector2 hLeftTopL;
    Vector2 hLeftBottomR;
    Vector2 hLeftBottomL;

    Vector2 hRightTopR;
    Vector2 hRightTopL;
    Vector2 hRightBottomR;
    Vector2 hRightBottomL;

#endif

    float rangeToCollide;
    float startX;
    float startY;

    bool canBeSeenByCamera = false;

    bool broken = false;
    float timeToDestroyAfterBreak = 2.5f;
    float heightDashEnded;

    [Header("Element")]
    //change to the element we need in editor
    public elements element = 0;

    [Header("Platform Breaking")]
    [SerializeField] bool breakableOnEarthDash = false;
    [SerializeField] float dashDistanceRequiredToBreak = 6.0f;

    [Header("Oscillation Direction")]
    [Tooltip("Platforms oscillating diagonally at different x and y speeds are not supported")]
    [SerializeField] bool oscillateVertically = false;
    [Tooltip("Platforms oscillating diagonally at different x and y speeds are not supported")]
    [SerializeField] bool oscillateUpwardsFirst = false;
    [Tooltip("Platforms oscillating diagonally at different x and y speeds are not supported")]
    [SerializeField] bool oscillateHorizontally = false;
    [Tooltip("Platforms oscillating diagonally at different x and y speeds are not supported")]
    [SerializeField] bool oscillateRightFirst = false;

    [Header("Oscillation Distance")]
    [SerializeField] float upwardOscillationDistance = 5.0f;
    [SerializeField] float downwardOscillationDistance = 5.0f;
    [SerializeField] float leftOscillationDistance = 5.0f;
    [SerializeField] float rightOscillationDistance = 5.0f;

    [Header("Oscillation Speed")]
    [Tooltip("Speed * Time.DeltaTime to travel")]
    [SerializeField] float verticalOscillationSpeed = 0.05f;
    [Tooltip("Speed * Time.DeltaTime to travel")]
    [SerializeField] float horizontalOscillationSpeed = 0.05f;

    [Header("Oscillation Pause Time")]
    [Tooltip("How long to pause at the oscillation distance in seconds")]
    [SerializeField] float verticalOscillationPauseTime = 3.0f;
    [Tooltip("How long to pause at the oscillation distance in seconds")]
    [SerializeField] float horizontalOscillationPauseTime = 3.0f;

    float tempXOscillationPauseTime;
    float tempYOscillationPauseTime;


    // Start is called before the first frame update
    void Start()
    {
        startX = transform.position.x;
        startY = transform.position.y;

        tempYOscillationPauseTime = verticalOscillationPauseTime;
        tempXOscillationPauseTime = horizontalOscillationPauseTime;

        //get platforms components
        p_collider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        alphaOriginal = spriteRenderer.color;
        alphaOriginal.a = 1.0f;
        alphaTemp = spriteRenderer.color;
        //value between 0-1 of alpha to change platforms to when we cant collide
        alphaTemp.a = alphaAmount;

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

    //called when can be seen by any camera
    void OnBecameVisible()
    {
        canBeSeenByCamera = true;
    }

    //called when no longer seen by any camera
    void OnBecameInvisible()
    {
        canBeSeenByCamera = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canBeSeenByCamera && Application.isPlaying)
        {
            //if element is neutral we never want to disable colliders
            if (element != elements.neutral)
            {
                //check if we're the same element, disable collision if not
                if (ElementCheck(element, playerstate.element) && p_collider.enabled == false)
                {
                    p_collider.enabled = true;
                    spriteRenderer.color = alphaOriginal;
                    DebugHelper.Log(this.gameObject + " had collider enabled because player is the same element and in range");
                }
                else if (!ElementCheck(element, playerstate.element) && p_collider.enabled == true)
                {
                    p_collider.enabled = false;
                    spriteRenderer.color = alphaTemp;
                    DebugHelper.Log(this.gameObject + " had collider disabled because player is a different element and in range");
                }
            }
        }

        if (levelDesignDebug && !Application.isPlaying)
        {
            topL = new Vector2(transform.position.x - (transform.localScale.x) / 2, transform.position.y + (transform.localScale.y) / 2);
            topR = new Vector2(transform.position.x + (transform.localScale.x) / 2, transform.position.y + (transform.localScale.y) / 2);
            bottomL = new Vector2(transform.position.x - (transform.localScale.x) / 2, transform.position.y - (transform.localScale.y) / 2);
            bottomR = new Vector2(transform.position.x + (transform.localScale.x) / 2, transform.position.y - (transform.localScale.y) / 2);

            DrawPlatformOscillationDebug();
        }

        if (!broken && Application.isPlaying)
        {
            if (oscillateVertically)
            {
                oscillateY(ref oscillateUpwardsFirst, startY, verticalOscillationSpeed * Time.deltaTime, ref tempYOscillationPauseTime);
            }
            if (oscillateHorizontally)
            {
                oscillateX(ref oscillateRightFirst, startX, horizontalOscillationSpeed * Time.deltaTime, ref tempXOscillationPauseTime);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            if (!EarthDash.earthDashEnded && breakableOnEarthDash && playerstate.element == Elements.elements.earth)
            {
                heightDashEnded = player.transform.position.y;
                DebugHelper.Log("Travelled " + (EarthDash.heightDashedAt - heightDashEnded) + " units with earth dash trying to break a platform");

                //check we travelled the distance we want and if we did break the platform
                if (EarthDash.heightDashedAt - heightDashEnded >= dashDistanceRequiredToBreak)
                {
                    EarthDash.earthDashEnded = true;
                    p_collider.enabled = false;
                    DebugHelper.Log(this.gameObject + " had collision disabled because it was broken from earthdash");
                    broken = true;
                    DebugHelper.Log(this.gameObject + " is being destroyed because it was broken from earthdash");
                    Destroy(gameObject, timeToDestroyAfterBreak);
                }
            }
            else if(CollisionManager.groundedObject == this.gameObject && (oscillateHorizontally || oscillateVertically))
            {
                collision.transform.parent = this.transform;
                DebugHelper.Log("Player transform is being parented by " + this.gameObject + " because it is oscillating");
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            if (WallClimb.grabbing && (oscillateHorizontally || oscillateVertically))
            {
                collision.transform.parent = this.transform;
                DebugHelper.Log("Player transform is being parented by " + this.gameObject + " because it is oscillating and player is grabbing");
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            if (oscillateHorizontally || oscillateVertically)
            {
                collision.transform.parent = null;
                DebugHelper.Log("Player transform is being unparented by " + this.gameObject + " because the player has left the platform");
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

    void oscillateY(ref bool startDirection, float start, float speed, ref float delay)
    {
        //if the start direction is true (up) and we're not at the max position
        if(startDirection && transform.position.y <= start + upwardOscillationDistance)
        {
            //check if the position change will be above the max distance
            if (!(transform.position.y + speed > start + upwardOscillationDistance))
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + speed);
            }
            else
            {
                transform.position = new Vector2(transform.position.x, start + upwardOscillationDistance);

                delay -= Time.deltaTime;
                if(delay <= 0)
                {
                    startDirection = false;
                    delay = verticalOscillationPauseTime;
                }
            }
        }

        if(!startDirection && transform.position.y >= start - downwardOscillationDistance)
        {
            //check if the position change will be below the negative max distance
            if (!(transform.position.y - speed < start - downwardOscillationDistance))
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - speed);
            }
            else
            {
                transform.position = new Vector2(transform.position.x, start - downwardOscillationDistance);

                delay -= Time.deltaTime;
                if (delay <= 0)
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
        if (startDirection && transform.position.x <= start + rightOscillationDistance)
        {
            //check if the position change will be above the max distance
            if (!(transform.position.x + speed > start + rightOscillationDistance))
            {
                transform.position = new Vector2(transform.position.x + speed, transform.position.y);
            }
            else
            {
                transform.position = new Vector2(start + rightOscillationDistance, transform.position.y);

                delay -= Time.deltaTime;
                if (delay < 0)
                {
                    startDirection = false;
                    delay = horizontalOscillationPauseTime;
                }
            }
        }

        if (!startDirection && transform.position.x >= start - leftOscillationDistance)
        {
            //check if the position change will be below the negative max distance
            if (!(transform.position.x - speed < start - leftOscillationDistance))
            {
                transform.position = new Vector2(transform.position.x - speed, transform.position.y);
            }
            else
            {
                transform.position = new Vector2(start - leftOscillationDistance, transform.position.y);

                delay -= Time.deltaTime;
                if (delay < 0)
                {
                    startDirection = true;
                    delay = horizontalOscillationPauseTime;
                }
            }
        }
    }

    void DrawPlatformOscillationDebug()
    {
        if (oscillateVertically)
        {
            //get the coords for top and bottom boxes (where the platforms will be after oscillating)
            vUpTopL = new Vector2(topL.x, topL.y + upwardOscillationDistance);
            vUpTopR = new Vector2(topR.x, topR.y + upwardOscillationDistance);
            vUpBottomL = new Vector2(bottomL.x, bottomL.y + upwardOscillationDistance);
            vUpBottomR = new Vector2(bottomR.x, bottomR.y + upwardOscillationDistance);

            vDownTopL = new Vector2(topL.x, topL.y - downwardOscillationDistance);
            vDownTopR = new Vector2(topR.x, topR.y - downwardOscillationDistance);
            vDownBottomL = new Vector2(bottomL.x, bottomL.y - downwardOscillationDistance);
            vDownBottomR = new Vector2(bottomR.x, bottomR.y - downwardOscillationDistance);

            //platform at top of its oscillation
            Debug.DrawRay(transform.position, Vector2.up * upwardOscillationDistance, Color.yellow, 0.0f);
            DebugHelper.DrawBox(vUpTopL, vUpTopR, vUpBottomL, vUpBottomR, Color.green);

            //platform at bottom of its oscillation
            Debug.DrawRay(transform.position, -Vector2.up * downwardOscillationDistance, Color.yellow, 0.0f);
            DebugHelper.DrawBox(vDownTopL, vDownTopR, vDownBottomL, vDownBottomR, Color.red);
        }
        if(oscillateHorizontally)
        {
            hLeftTopL = new Vector2(topL.x - leftOscillationDistance, topL.y);
            hLeftTopR = new Vector2(topR.x - leftOscillationDistance, topR.y);
            hLeftBottomL = new Vector2(bottomL.x - leftOscillationDistance, bottomL.y);
            hLeftBottomR = new Vector2(bottomR.x - leftOscillationDistance, bottomR.y);

            hRightTopL = new Vector2(topL.x + rightOscillationDistance, topL.y);
            hRightTopR = new Vector2(topR.x + rightOscillationDistance, topR.y);
            hRightBottomL = new Vector2(bottomL.x + rightOscillationDistance, bottomL.y);
            hRightBottomR = new Vector2(bottomR.x + rightOscillationDistance, bottomR.y);

            //platform at left of its oscillation
            Debug.DrawRay(transform.position, -Vector2.right * leftOscillationDistance, Color.yellow, 0.0f);
            DebugHelper.DrawBox(hLeftTopL, hLeftTopR, hLeftBottomL, hLeftBottomR, Color.red);

            //platform at right of its oscillation
            Debug.DrawRay(transform.position, Vector2.right * rightOscillationDistance, Color.yellow, 0.0f);
            DebugHelper.DrawBox(hRightTopL, hRightTopR, hRightBottomL, hRightBottomR, Color.green);
        }
    }

}
