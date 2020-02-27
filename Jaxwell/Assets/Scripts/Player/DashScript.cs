﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashScript : MonoBehaviour
{
    Rigidbody2D p_rigidbody;

    MoveScript movescript;
    JumpScript jumpScript;
    PlayerState playerState;

    [SerializeField] float dashSpeed = 30.0f;
    public float dashCooldown = 2.0f;
    [SerializeField] float dashDuration = 0.1f;

    float initialGravityScale;
    public float tempDashCooldown;
    private float tempDashDuration;

    bool pressedDashLeft = false;
    bool pressedDashRight = false;

    public bool dashing = false;
    public bool canDash = true;
    bool dashJustEnded = false;

    public bool pressedDash = false;


    // Start is called before the first frame update
    void Start()
    {
        playerState = GetComponent<PlayerState>();

        p_rigidbody = GetComponent<Rigidbody2D>();
        initialGravityScale = p_rigidbody.gravityScale;
        movescript = GetComponent<MoveScript>();
        jumpScript = GetComponent<JumpScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //input
        if (pressedDash)
        {
            //check which direction we're going from the movescript
            switch (MoveScript.movingRight)
            {
                case false:
                    pressedDashLeft = true;
                    break;

                case true:
                    pressedDashRight = true;
                    break;

                default:
                    pressedDashRight = true;
                    break;
            }            
        }

        if (!canDash)
        {
            //since we're calling in update, time.deltatime is time since last frame, so -= will give us a countdown
            tempDashCooldown -= Time.deltaTime;
            //if cooldown is finished we can dash again
            if (tempDashCooldown <= 0)
            {
                canDash = true;
            }
        }

        if (dashing)
        {
            tempDashDuration -= Time.deltaTime;
            //check if duration has completed
            if (tempDashDuration <= 0 || playerState.element != Elements.elements.fire || jumpScript.pressedJump)
            {
                dashing = false;
                //set this bool to handle dash end physics in fixedupdate
                dashJustEnded = true;
            }
        }
    }

    void FixedUpdate()
    {
        if(pressedDashLeft == true)
        {
            DashLeft(p_rigidbody, dashSpeed);
            pressedDashLeft = false;
            pressedDash = false;
        }

        if (pressedDashRight == true)
        {
            DashRight(p_rigidbody, dashSpeed);
            pressedDashRight = false;
            pressedDash = false;
        }

        if (dashJustEnded)
        {
            HandleDashEnd(p_rigidbody);
            dashJustEnded = false;
        }
    }

    void DashRight(Rigidbody2D rigidbody, float speed)
    {
        DebugHelper.Log("Dashed Right");
        WallClimb.grabbing = false;
        //set x and y velocity to 0 to make our dash more reliable
        rigidbody.velocity = new Vector2(0.0f, 0.0f);
        dashing = true;

        //add a force in the positive X direction to dash right
        rigidbody.AddForce(new Vector2(speed, 0), ForceMode2D.Impulse);

        //Set on cooldown
        canDash = false;

        //set gravity to 0 so short sharp dash in air
        rigidbody.gravityScale = 0;

        tempDashCooldown = dashCooldown;
        tempDashDuration = dashDuration;
    }

    void DashLeft(Rigidbody2D rigidbody, float speed)
    {
        DebugHelper.Log("Dashed Left");
        WallClimb.grabbing = false;
        //set x and y velocity to 0 to make our dash more reliable
        rigidbody.velocity = new Vector2(0.0f, 0.0f);
        dashing = true;

        //add a force in the positive X direction to dash right
        rigidbody.AddForce(new Vector2(-speed, 0), ForceMode2D.Impulse); 

        //Set on cooldown
        canDash = false;

        //set gravity to 0 so short sharp dash in air
        rigidbody.gravityScale = 0;

        tempDashCooldown = dashCooldown;
        tempDashDuration = dashDuration;
    }

    //function to handle physics at the end of dashing
    void HandleDashEnd(Rigidbody2D rigidbody)
    {
        //zero out our x velocity to make us stop
        rigidbody.velocity = new Vector2(0.0f, rigidbody.velocity.y);
        //change the gravity scale back to the one we have in editor
        rigidbody.gravityScale = initialGravityScale;
        DebugHelper.Log("Reset gravity after dash ended");
    }
}
