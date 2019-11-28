using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthDash : MonoBehaviour
{
    Rigidbody2D p_rigidbody;
    PlayerState playerstate;
    float initialGravityScale;

    [SerializeField] float earthDashSpeed = 20.0f;

    public static bool pressedDashToEarth = false;
    bool forceApplied;

    // Start is called before the first frame update
    void Start()
    {
        p_rigidbody = GetComponent<Rigidbody2D>();
        playerstate = GetComponent<PlayerState>();
        initialGravityScale = p_rigidbody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(CollisionManager.isGrounded || WallClimb.grabbing)
        {
            pressedDashToEarth = false;
            forceApplied = false;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3) && playerstate.element != Elements.elements.earth)
        {
            pressedDashToEarth = true;
        }
    }

    void FixedUpdate()
    {
        if(pressedDashToEarth && !forceApplied)
        {
            DashToEarth(p_rigidbody);
            forceApplied = true;
        }
        
        //set gravity back to normal if we're climbing or dashing
        if(!pressedDashToEarth && !WallClimb.grabbing)
        {
            p_rigidbody.gravityScale = initialGravityScale;
            p_rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        }
        
    }

    void DashToEarth(Rigidbody2D rigidbody)
    {
        //disable gravity
        rigidbody.gravityScale = 0;
        //add a fall speed for when we're grabbing
        rigidbody.velocity = new Vector2(0, 0);

        //add a force for our
        rigidbody.AddForce(new Vector2(0, -earthDashSpeed), ForceMode2D.Impulse);

        rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }
}
