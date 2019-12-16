using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimb : MonoBehaviour
{
    Rigidbody2D p_rigidbody;
    PlayerState playerstate;
    float initialGravityScale;
    DashScript dashScript;

    [SerializeField] float grabbingFallSpeed = -0.1f;
    [SerializeField] float wallJumpHeight = 15.0f;
    [SerializeField] float wallJumpHorizontalForce = 3.0f;

    public static bool grabbing = false;
    public bool pressedWallJump = false;

    void Start()
    {
        p_rigidbody = GetComponent<Rigidbody2D>();
        playerstate = GetComponent<PlayerState>();
        dashScript = GetComponent<DashScript>();
        initialGravityScale = p_rigidbody.gravityScale;
    }

    void Update()
    {
        //if we're against a wall, not grounded, water type and we're not moving upwards
        if((CollisionManager.isAgainstWallRight || CollisionManager.isAgainstWallLeft) && !CollisionManager.isGrounded && playerstate.element == Elements.elements.water && p_rigidbody.velocity.y < 0)
        {
            grabbing = true;
        }
        else
        {
            grabbing = false;
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
        //disable gravity
        rigidbody.gravityScale = 0;
        //add a fall speed for when we're grabbing
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, grabbingFallSpeed);
    }

    void WallJump(Rigidbody2D rigidbody, int direction)
    {
        //add a force in x and y direction to jump off the wall
        rigidbody.AddForce(new Vector2(wallJumpHorizontalForce * direction, wallJumpHeight), ForceMode2D.Impulse);
    }
}
