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
    [SerializeField] float timeToUnstick = 0.5f;
    [SerializeField] float grabbingFallSpeed = -0.1f;
    [SerializeField] float wallJumpHeight = 15.0f;
    [SerializeField] float wallJumpHorizontalForce = 3.0f;
    [SerializeField] float timeToIgnoreDecelerationForWallJump = 0.5f;

    public static bool grabbing = false;
    public bool pressedWallJump = false;
    

    float tempTimeToWaitBeforeSliding;
    float temptimeToIgnoreDecelerationForWallJump;
    float temptimeToUnstick;
    public static bool ignoreDecelerationForWallJump = false;

    //animator
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        p_rigidbody = GetComponent<Rigidbody2D>();
        playerstate = GetComponent<PlayerState>();
        dashScript = GetComponent<DashScript>();
        initialGravityScale = p_rigidbody.gravityScale;

        tempTimeToWaitBeforeSliding = timeToWaitBeforeSliding;
        temptimeToIgnoreDecelerationForWallJump = timeToIgnoreDecelerationForWallJump;
        temptimeToUnstick = timeToUnstick;
    }

    void Update()
    {
        //if we're against a wall, not grounded, water type
        if((CollisionManager.isAgainstWallRight || CollisionManager.isAgainstWallLeft) && !CollisionManager.isGrounded && playerstate.element == Elements.elements.water)
        {
            grabbing = true;
            if (CollisionManager.isAgainstWallLeft && tempTimeToWaitBeforeSliding > 0)
            {
                animator.SetTrigger("grabLeft");
                animator.SetBool("grabbing", true);
            }
            if (CollisionManager.isAgainstWallRight && tempTimeToWaitBeforeSliding > 0)
            {
                animator.SetTrigger("grabRight");
                animator.SetBool("grabbing", true);
            }

            animator.SetBool("wallJump", false);

            if (tempTimeToWaitBeforeSliding > 0)
            {
                //count down to slide
                tempTimeToWaitBeforeSliding -= Time.deltaTime;
            }
            else
            {
                animator.ResetTrigger("grabLeft");
                animator.ResetTrigger("grabRight");
                animator.SetBool("grabbing", false);
                animator.SetBool("sliding", true);                
            }

            if ((Input.GetKey(KeyCode.D) || Input.GetAxis("Horizontal") > 0) && CollisionManager.isAgainstWallLeft)
            {
                if (temptimeToUnstick > 0)
                {
                    temptimeToUnstick -= Time.deltaTime;
                    if (temptimeToUnstick <= 0)
                    {
                        grabbing = false;
                        animator.ResetTrigger("grabLeft");
                        animator.SetBool("grabbing", false);
                        animator.SetBool("sliding", false);
                    }
                }
                DebugHelper.Log("Unstick time remaining: " + temptimeToUnstick);
            }
            if ((Input.GetKeyUp(KeyCode.D) || Input.GetAxis("Horizontal") <= 0) && CollisionManager.isAgainstWallLeft)
            {
                if (temptimeToUnstick != timeToUnstick)
                {
                    temptimeToUnstick = timeToUnstick;
                    DebugHelper.Log("Unstick time reset due to releasing right input");
                }
            }

            if ((Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal") < 0) && CollisionManager.isAgainstWallRight)
            {
                if (temptimeToUnstick > 0)
                {
                    temptimeToUnstick -= Time.deltaTime;
                    if (temptimeToUnstick <= 0)
                    {
                        grabbing = false;
                        animator.ResetTrigger("grabRight");
                        animator.SetBool("grabbing", false);
                        animator.SetBool("sliding", false);
                    }
                }
                DebugHelper.Log("Unstick time remaining: " + temptimeToUnstick);
            }
            if ((Input.GetKeyUp(KeyCode.A) || Input.GetAxis("Horizontal") >= 0) && CollisionManager.isAgainstWallRight)
            {
                if (temptimeToUnstick != timeToUnstick)
                {
                    temptimeToUnstick = timeToUnstick;
                    DebugHelper.Log("Unstick time reset due to releasing left input");
                }
            }
        }
        else
        {
            grabbing = false;
            animator.ResetTrigger("grabLeft");
            animator.ResetTrigger("grabRight");
            animator.SetBool("grabbing", false);
            animator.SetBool("sliding", false);
            //reset the timer to start sliding
            if (tempTimeToWaitBeforeSliding != timeToWaitBeforeSliding)
            {                
                tempTimeToWaitBeforeSliding = timeToWaitBeforeSliding;
                DebugHelper.Log("Wall grab hang time reset");
            }
            if (temptimeToUnstick != timeToUnstick)
            {
                temptimeToUnstick = timeToUnstick;
                DebugHelper.Log("Unstick time reset due to grabbing being false");
            }

        }          

        if(ignoreDecelerationForWallJump)
        {
            DebugHelper.Log("Ignore deceleration for wall jump");
            temptimeToIgnoreDecelerationForWallJump -= Time.deltaTime;
            if(temptimeToIgnoreDecelerationForWallJump <= 0)
            {
                DebugHelper.Log("Stopped ignoring deceleration for wall jump");
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
        if(!EarthDash.pressedDashToEarth && !grabbing && !dashScript.dashing && p_rigidbody.gravityScale != initialGravityScale)
        {
            DebugHelper.Log("Setting initial gravity scale in wallclimb script");
            //set initial gravity scale back since we removed it to have a smooth fall speed
            p_rigidbody.gravityScale = initialGravityScale;
        }

        if(pressedWallJump)
        {
            if (CollisionManager.isAgainstWallRight)
            {
                //jump in left direction if we're against a wall to the right
                WallJump(p_rigidbody, -1);
                MoveScript.movingRight = false;
                animator.SetBool("moveRight", MoveScript.movingRight);
            }

            if (CollisionManager.isAgainstWallLeft)
            {
                //jump in right direction if we're against a wall to the left
                WallJump(p_rigidbody, 1);
                MoveScript.movingRight = true;
                animator.SetBool("moveRight", MoveScript.movingRight);
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
            DebugHelper.Log(rigidbody + " stopped Y movement for water wall grabbing");
            rigidbody.gravityScale = 0;
            DebugHelper.Log(rigidbody + " disabled gravity for water wall grabbing");
        }        
        if (tempTimeToWaitBeforeSliding <= 0)
        {            
            //add a fall speed for when we're grabbing
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, grabbingFallSpeed);
            DebugHelper.Log(rigidbody + " wall grab hang time has ended, falling with grabbing fall speed " + grabbingFallSpeed);
        }
    }

    void WallJump(Rigidbody2D rigidbody, int direction)
    {
        animator.SetBool("wallJump", true);
        rigidbody.velocity = new Vector2(0, 0);
        //add a force in x and y direction to jump off the wall
        rigidbody.AddForce(new Vector2(wallJumpHorizontalForce * direction, wallJumpHeight), ForceMode2D.Impulse);
        DebugHelper.Log(rigidbody + " wall jumped " + direction + "with a horizontal force of " + wallJumpHorizontalForce + "and vertical force of " + wallJumpHeight);

        //reset the timer for ignoring deceleration if it's not yet over
        temptimeToIgnoreDecelerationForWallJump = timeToIgnoreDecelerationForWallJump;
        ignoreDecelerationForWallJump = true;
    }
}
