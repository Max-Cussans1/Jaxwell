﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    [SerializeField] float maxSpeed = 3.0f;
    [SerializeField] float acceleration = 0.3f;
    [SerializeField] float deceleration = 0.3f;

    bool acceleratingRight = false;
    bool acceleratingLeft = false;

    Rigidbody2D p_rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        //cache rigidbody at the start of the game
        p_rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //set bools for movement in update so we're instantly detecting input
        if (Input.GetKey(KeyCode.D))
        {
            acceleratingRight = true;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            acceleratingRight = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            acceleratingLeft = true;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            acceleratingLeft = false;
        }

    }

    void FixedUpdate()
    {
        //handle movement in fixedupdate to be consistent at all framerates
        if(acceleratingRight)
        {
            AccelerateRight(p_rigidbody, acceleration, maxSpeed);
        }

        if(acceleratingLeft)
        {
            AccelerateLeft(p_rigidbody, acceleration, maxSpeed);
        }

        if(!acceleratingRight && !acceleratingLeft)
        {
            Decelerate(p_rigidbody, deceleration);
        }
    }

    //function to move right
    void AccelerateRight(Rigidbody2D rigidbody, float accelerationValue, float maximumSpeed)
    {
        //check if we're going above top speed and set it to top speed if we are
        if (rigidbody.velocity.x + accelerationValue > maximumSpeed)
        {
            rigidbody.velocity = new Vector2(maxSpeed, rigidbody.velocity.y);
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
        //check if we're going above top speed and set it to top speed if we are
        if (rigidbody.velocity.x - accelerationValue < -maximumSpeed)
        {
            rigidbody.velocity = new Vector2(-maxSpeed, rigidbody.velocity.y);
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