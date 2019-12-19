using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimb : MonoBehaviour
{
    Rigidbody2D p_rigidbody;
    PlayerState playerstate;
    float initialGravityScale;
    DashScript dashScript;

    [SerializeField] float timeToWaitBeforeSliding = 0.5f;
    [SerializeField] float grabbingFallSpeed = -0.1f;
    [SerializeField] float wallJumpHeight = 15.0f;
    [SerializeField] float wallJumpHorizontalForce = 3.0f;
    [SerializeField] float timeToIgnoreDecelerationForWallJump = 0.5f;

    public static bool grabbing = false;
    public bool pressedWallJump = false;
    

    float tempTimeToWaitBeforeSliding;
    float temptimeToIgnoreDecelerationForWallJump;
    public static bool ignoreDecelerationForWallJump = false;

    void Start()
    {
        p_rigidbody = GetComponent<Rigidbody2D>();
        playerstate = GetComponent<PlayerState>();
        dashScript = GetComponent<DashScript>();
        initialGravityScale = p_rigidbody.gravityScale;
        tempTimeToWaitBeforeSliding = timeToWaitBeforeSliding;
        temptimeToIgnoreDecelerationForWallJump = timeToIgnoreDecelerationForWallJump;
    }

    void Update()
    {
        //if we're against a wall, not grounded, water type
        if((CollisionManager.isAgainstWallRight || CollisionManager.isAgainstWallLeft) && !CollisionManager.isGrounded && playerstate.element == Elements.elements.water)
        {
            grabbing = true;
            if (tempTimeToWaitBeforeSliding > 0)
            {
                //count down to slide
                tempTimeToWaitBeforeSliding -= Time.deltaTime;
            }
        }
        else
        {
            grabbing = false;
            //reset the timer to start sliding
            if (tempTimeToWaitBeforeSliding != timeToWaitBeforeSliding)
            {                
                tempTimeToWaitBeforeSliding = timeToWaitBeforeSliding;
                Debug.Log("Wall grab hang time reset");
            }
        }          

        if(ignoreDecelerationForWallJump)
        {
            Debug.Log("Ignore deceleration for wall jump");
            temptimeToIgnoreDecelerationForWallJump -= Time.deltaTime;
            if(temptimeToIgnoreDecelerationForWallJump <= 0)
            {
                Debug.Log("Stopped ignoring deceleration for wall jump");
                ignoreDecelerationForWallJump = false;
                temptimeToIgnoreDecelerationForWallJump = timeToIgnoreDecelerationForWallJump;
            }
        }
    }

    void FixedUpdate()
    {
        if(grabbing)
        {
            Grab(p_rigidbody);
        }
        if(!grabbing && !EarthDash.pressedDashToEarth && !dashScript.dashing && p_rigidbody.gravityScale != initialGravityScale)
        {
            //set initial gravity scale back since we removed it to have a smooth fall speed
            p_rigidbody.gravityScale = initialGravityScale;
        }

        if(pressedWallJump)
        {
            if (CollisionManager.isAgainstWallRight)
            {
                //jump in left direction if we're against a wall to the right
                WallJump(p_rigidbody, -1);
            }

            if (CollisionManager.isAgainstWallLeft)
            {
                //jump in right direction if we're against a wall to the left
                WallJump(p_rigidbody, 1);
            }
            pressedWallJump = false;
        }
    }

    void Grab(Rigidbody2D rigidbody)
    {
        //stop the y movement to stick to the wall and disable gravity so we don't fall - only do this if we're moving downwards so we can jump against walls still
        if (rigidbody.velocity.y < 0)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
            Debug.Log(rigidbody + " stopped Y movement for water wall grabbing");
            rigidbody.gravityScale = 0;
            Debug.Log(rigidbody + " disabled gravity for water wall grabbing");
        }        
        if (tempTimeToWaitBeforeSliding <= 0)
        {            
            //add a fall speed for when we're grabbing
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, grabbingFallSpeed);
            Debug.Log(rigidbody + " wall grab hang time has ended, falling with grabbing fall speed " + grabbingFallSpeed);
        }
    }

    void WallJump(Rigidbody2D rigidbody, int direction)
    {
        rigidbody.velocity = new Vector2(0, 0);
        //add a force in x and y direction to jump off the wall
        rigidbody.AddForce(new Vector2(wallJumpHorizontalForce * direction, wallJumpHeight), ForceMode2D.Impulse);
        Debug.Log(rigidbody + " wall jumped " + direction + "with a horizontal force of " + wallJumpHorizontalForce + "and vertical force of " + wallJumpHeight);

        //reset the timer for ignoring deceleration if it's not yet over
        temptimeToIgnoreDecelerationForWallJump = timeToIgnoreDecelerationForWallJump;
        ignoreDecelerationForWallJump = true;
    }
}
