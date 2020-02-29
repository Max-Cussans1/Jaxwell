using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    public float maxSpeed = 3.0f;
    float tempMaxSpeed;
    public float acceleration = 0.3f;
    public float deceleration = 0.3f;

    //bool for which direction we're going, true if going right, false if going left
    public static bool movingRight = true;

    bool acceleratingRight = false;
    bool acceleratingLeft = false;

    Rigidbody2D p_rigidbody;
    DashScript dashScript;

    //animator
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        //cache rigidbody at the start of the game
        p_rigidbody = GetComponent<Rigidbody2D>();
        dashScript = GetComponent<DashScript>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.isPaused && !GameOver.gameOver)
        {
            //set bools for movement in update so we're instantly detecting input
            if (Input.GetKey(KeyCode.D) || Input.GetAxis("Horizontal") > 0)
            {
                movingRight = true;
                animator.SetBool("moveRight", movingRight);

                if (!WallClimb.grabbing)
                {
                    //if we're using joystick to getaxis (Keys are also used to determine input.getaxis) limit max speed based on joystick position
                    if ((!Input.GetKey(KeyCode.D) && Input.GetAxis("Horizontal") > 0))
                    {
                        tempMaxSpeed = maxSpeed * Input.GetAxis("Horizontal");
                    }
                    else
                    {
                        tempMaxSpeed = maxSpeed;
                    }

                    animator.SetFloat("Speed", tempMaxSpeed);

                    acceleratingRight = true;
                }
            }
            if (Input.GetKeyUp(KeyCode.D) || Input.GetAxis("Horizontal") <= 0)
            {
                acceleratingRight = false;
            }

            if (Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal") < 0)
            {
                movingRight = false;
                animator.SetBool("moveRight", movingRight);

                if (!WallClimb.grabbing)
                {
                    //if we're using joystick to getaxis (Keys are also used to determine input.getaxis) limit max speed based on joystick position
                    if ((!Input.GetKey(KeyCode.A) && Input.GetAxis("Horizontal") < 0))
                    {
                        tempMaxSpeed = maxSpeed * -Input.GetAxis("Horizontal");
                    }
                    else
                    {
                        tempMaxSpeed = maxSpeed;
                    }

                    animator.SetFloat("Speed", tempMaxSpeed);

                    acceleratingLeft = true;
                }
            }
            if (Input.GetKeyUp(KeyCode.A) || Input.GetAxis("Horizontal") >= 0)
            {
                acceleratingLeft = false;
            }

            if (!acceleratingRight && !acceleratingLeft && !dashScript.dashing && !WallClimb.ignoreDecelerationForWallJump)
            {
                animator.SetFloat("Speed", 0.0f);
            }
        }
    }

    void FixedUpdate()
    {
        //handle movement in fixedupdate to be consistent at all framerates
        if (acceleratingRight && !dashScript.dashing)
        {
            AccelerateRight(p_rigidbody, acceleration, tempMaxSpeed);
        }

        if (acceleratingLeft && !dashScript.dashing)
        {
            AccelerateLeft(p_rigidbody, acceleration, tempMaxSpeed);
        }

        if(!acceleratingRight && !acceleratingLeft && !dashScript.dashing && !WallClimb.ignoreDecelerationForWallJump)
        {
            Decelerate(p_rigidbody, deceleration);
        }

    }

    //function to move right
    void AccelerateRight(Rigidbody2D rigidbody, float accelerationValue, float maximumSpeed)
    {
        //if we're moving left and want to accelerate right reset speed
        if (rigidbody.velocity.x < 0 && !WallClimb.ignoreDecelerationForWallJump)
        {
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
        }
        //check if we're going above top speed and set it to top speed if we are
        if (rigidbody.velocity.x + accelerationValue > maximumSpeed)
        {
            rigidbody.velocity = new Vector2(maximumSpeed, rigidbody.velocity.y);
        }
        else
        {
            //add acceleration in the positive x direction for our rigidbody's velocity
            rigidbody.velocity = new Vector2(rigidbody.velocity.x + accelerationValue, rigidbody.velocity.y);
        }
    }

    //function to move left
    void AccelerateLeft(Rigidbody2D rigidbody, float accelerationValue, float maximumSpeed)
    {
        //if we're moving right and want to accelerate left reset speed
        if(rigidbody.velocity.x > 0 && !WallClimb.ignoreDecelerationForWallJump)
        {
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
        }
        //check if we're going above top speed and set it to top speed if we are
        if (rigidbody.velocity.x - accelerationValue < -maximumSpeed)
        {
            rigidbody.velocity = new Vector2(-maximumSpeed, rigidbody.velocity.y);
        }
        else
        {
            //otherwise add acceleration in the negative x direction for our rigidbody's velocity
            rigidbody.velocity = new Vector2(rigidbody.velocity.x - accelerationValue, rigidbody.velocity.y);
        }

    }

    void Decelerate(Rigidbody2D rigidbody, float decelerationValue)
    {
        //if we're less than our deceleration speed to stopping set our velocity to 0
        if (rigidbody.velocity.x <= decelerationValue && rigidbody.velocity.x > 0 || rigidbody.velocity.x >= -decelerationValue && rigidbody.velocity.x < 0)
        {
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
        }

        //if we have a negative velocity, accelerate in positive direction
        if (rigidbody.velocity.x < 0)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x + decelerationValue, rigidbody.velocity.y);
        }

        //if we have a positive velocity, accelerate in negative direction
        else if (p_rigidbody.velocity.x > 0)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x - decelerationValue, rigidbody.velocity.y);
        }

    }
}
