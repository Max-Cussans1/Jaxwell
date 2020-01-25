using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthDash : MonoBehaviour
{
    Rigidbody2D p_rigidbody;
    PlayerState playerstate;
    float initialGravityScale;
    float initialDrag;

    [SerializeField] float earthDashSpeed = 20.0f;

    public static bool pressedDashToEarth = false;
    bool forceApplied;
    bool earthDashEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        p_rigidbody = GetComponent<Rigidbody2D>();
        playerstate = GetComponent<PlayerState>();
        initialGravityScale = p_rigidbody.gravityScale;
        initialDrag = p_rigidbody.drag;
    }

    // Update is called once per frame
    void Update()
    {
        //if we change element after dashing or we are grounded set gravity and drag back to normal
        if((playerstate.element != Elements.elements.earth && forceApplied) || CollisionManager.isGrounded)
        {
            earthDashEnded = true;
            forceApplied = false;
        }
    }

    void FixedUpdate()
    {
        if(pressedDashToEarth && !forceApplied && playerstate.element == Elements.elements.earth)
        {
            DashToEarth(p_rigidbody, earthDashSpeed);
            forceApplied = true;
            pressedDashToEarth = false;
        }

        if(earthDashEnded)
        {            
            HandleEarthDashEnd(p_rigidbody);
            earthDashEnded = false;
        }
        
    }

    void DashToEarth(Rigidbody2D rigidbody, float speed)
    {
        //disable gravity & drag
        rigidbody.gravityScale = 0;
        rigidbody.drag = 0;

        //if we are travelling upwards
        if (rigidbody.velocity.y > 0)
        {
            //zero out x and y velocity to make the dash more sudden
            rigidbody.velocity = new Vector2(0, 0);
        }
        //if we are travelling downwards already, we add the force to the current speed so we will never be slowing down
        //add a force for our downward dash
        rigidbody.AddForce(new Vector2(0, -speed), ForceMode2D.Impulse);
    }

    void HandleEarthDashEnd(Rigidbody2D rigidbody)
    {
        //reset gravity & drag
        rigidbody.gravityScale = initialGravityScale;
        rigidbody.drag = initialDrag;
    }
}
