using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    [SerializeField] float maxSpeed = 3.0f;
    [SerializeField] float acceleration = 0.3f;
    [SerializeField] float deceleration = 0.3f;

    bool accelerating = false;
    bool lastInputRight = false;

    Rigidbody2D p_rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        p_rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //function to move right
    void AccelerateRight(Rigidbody2D rigidbody, float accelerationValue)
    {
        //add acceleration in the positive x direction for our rigidbody's velocity
        rigidbody.velocity = new Vector2(rigidbody.velocity.x + accelerationValue, rigidbody.velocity.y);
        accelerating = true;
        lastInputRight = true;
    }

    //function to move left
    void AccelerateLeft(Rigidbody2D rigidbody, float accelerationValue)
    {
        //add acceleration in the negative x direction for our rigidbody's velocity
        p_rigidbody.velocity = new Vector2(p_rigidbody.velocity.x - acceleration, p_rigidbody.velocity.y);
        accelerating = true;
        lastInputRight = false;
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
            rigidbody.velocity = new Vector2(rigidbody.velocity.x - deceleration, rigidbody.velocity.y);
        }

    }
}
