using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthDash : MonoBehaviour
{
    Rigidbody2D p_rigidbody;
    PlayerState playerstate;
    float initialGravityScale;
    float initialDrag;
    DashScript dashScript;

    public static float heightDashedAt;

    [SerializeField] float earthDashSpeed = 20.0f;

    public static bool pressedDashToEarth = false;
    bool forceApplied = false;
    public static bool earthDashEnded = true;

    // Start is called before the first frame update
    void Start()
    {
        p_rigidbody = GetComponent<Rigidbody2D>();
        playerstate = GetComponent<PlayerState>();
        dashScript = GetComponent<DashScript>();
        initialGravityScale = p_rigidbody.gravityScale;
        initialDrag = p_rigidbody.drag;
    }

    // Update is called once per frame
    void Update()
    {
        //if we change element after dashing or we are grounded set gravity and drag back to normal
        if((playerstate.element != Elements.elements.earth && playerstate.element != Elements.elements.fire) && forceApplied || (playerstate.element == Elements.elements.earth && CollisionManager.isGrounded))
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

        if(earthDashEnded && !dashScript.dashing)
        {            
            HandleEarthDashEnd(p_rigidbody);
            earthDashEnded = false;
        }
        
    }

    void DashToEarth(Rigidbody2D rigidbody, float speed)
    {
        Platform platform = null;
        if (CollisionManager.groundedObject != null)
        {
            platform = CollisionManager.groundedObject.GetComponent<Platform>();
        }
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
        //if we're on a platform
        if (platform != null)
        {
            //check if the platform is not earth
            if(!Elements.ElementCheck(platform.element, Elements.elements.earth) && platform.element != Elements.elements.neutral)
            {
                //if it's not earth or neutral, disable the platform's collider before we dash
                platform.GetComponent<BoxCollider2D>().enabled = false;
                DebugHelper.Log("Disabled collision for " + platform.gameObject + " earth dash because it wasn't an earth platform");
            }
        }

        //add a force for our downward dash
        rigidbody.AddForce(new Vector2(0, -speed), ForceMode2D.Impulse);
        DebugHelper.Log("Earth dash used");
    }

    void HandleEarthDashEnd(Rigidbody2D rigidbody)
    {
        DebugHelper.Log("Resetting gravity because earth dash ended");
        //reset gravity & drag
        rigidbody.gravityScale = initialGravityScale;
        rigidbody.drag = initialDrag;
    }
}
