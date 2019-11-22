using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashScript : MonoBehaviour
{
    Rigidbody2D p_rigidbody;
    BoxCollider2D p_collider;

    MoveScript movescript;

    [SerializeField] float dashSpeed = 30.0f;
    [SerializeField] float dashCooldown = 2.0f;
    [SerializeField] float dashDuration = 0.1f;

    float initialGravityScale;
    private float tempDashCooldown;
    private float tempDashDuration;

    bool pressedDashLeft = false;
    bool pressedDashRight = false;

    public bool dashing = false;
    bool canDash = true;


    // Start is called before the first frame update
    void Start()
    {
        p_rigidbody = GetComponent<Rigidbody2D>();
        p_collider = GetComponent<BoxCollider2D>();
        initialGravityScale = p_rigidbody.gravityScale;
        movescript = GetComponent<MoveScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //input
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            if (canDash)
            {
                //check which direction we're going from the movescript
                switch (movescript.direction)
                {
                    case MoveScript.directions.left:
                        pressedDashLeft = true;
                        break;

                    case MoveScript.directions.right:
                        pressedDashRight = true;
                        break;

                    default:
                        pressedDashRight = true;
                        break;
                }
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

        //while we're dashing handle gravity and collision detection modes
        if (dashing)
        {
            //since we might be going fast enable continuous collision
            p_rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            //set gravity to 0 so short sharp dash in air
            p_rigidbody.gravityScale = 0;

            tempDashDuration -= Time.deltaTime;
            //check if duration has completed
            if (tempDashDuration <= 0)
            {
                dashing = false;
                p_rigidbody.velocity = new Vector2(0.0f, p_rigidbody.velocity.y);
                p_rigidbody.gravityScale = initialGravityScale;
                p_rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
            }
        }
    }

    void FixedUpdate()
    {
        if(pressedDashLeft == true)
        {
            DashLeft(dashSpeed);
            pressedDashLeft = false;
        }

        if (pressedDashRight == true)
        {
            DashRight(dashSpeed);
            pressedDashRight = false;
        }
    }

    void DashRight(float speed)
    {
        Debug.Log("Dashed Right");
        //set x velocity to 0 to make our dash more reliable
        p_rigidbody.velocity = new Vector2(0.0f, p_rigidbody.velocity.y);
        dashing = true;
        //add a force in the positive X direction to dash right
        p_rigidbody.AddForce(new Vector2(speed, 0), ForceMode2D.Impulse);

        //Set on cooldown
        canDash = false;

        tempDashCooldown = dashCooldown;
        tempDashDuration = dashDuration;
    }

    void DashLeft(float speed)
    {
        Debug.Log("Dashed Left");
        //set x velocity to 0 to make our dash more reliable
        p_rigidbody.velocity = new Vector2(0.0f, p_rigidbody.velocity.y);
        dashing = true;
        //add a force in the positive X direction to dash right
        p_rigidbody.AddForce(new Vector2(-speed, 0), ForceMode2D.Impulse); 

        //Set on cooldown
        canDash = false;

        tempDashCooldown = dashCooldown;
        tempDashDuration = dashDuration;
    }
}
