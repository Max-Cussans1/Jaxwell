using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript : MonoBehaviour
{
    [SerializeField] float jumpHeight = 15.0f;

    PlayerState playerstate;
    Rigidbody2D p_rigidbody;

    public bool pressedAirJump = false;
    public bool usedAirJump = false;
    public bool pressedJump = false;
    public bool pressedPreJump = false;

    bool preJump = false;

    // Start is called before the first frame update
    void Start()
    {
        p_rigidbody = GetComponent<Rigidbody2D>();
        playerstate = GetComponent<PlayerState>();
    }

    // Update is called once per frame
    void Update()
    {
        if(CollisionManager.isGrounded && usedAirJump)
        {
            usedAirJump = false;
        }

        if(CollisionManager.isGrounded && pressedPreJump)
        {
            preJump = true;
        }
    }

    void FixedUpdate()
    {
        //call our jump function in fixedupdate so it's consistent across machines
        if (preJump)
        {
            DebugHelper.Log("Jumping from pre-jump");
            Jump(jumpHeight);
            pressedPreJump = false;
            preJump = false;
            pressedJump = false;
            CollisionManager.jumped = true;
        }

        //call our jump function in fixedupdate so it's consistent across machines
        if (pressedJump)
        {
            DebugHelper.Log("Jumping from a normal jump");
            Jump(jumpHeight);
            pressedJump = false;
            CollisionManager.jumped = true;
        }

        //call our jump function in fixedupdate so it's consistent across machines
        if (pressedAirJump)
        {
            Jump(jumpHeight);
            usedAirJump = true;
            pressedAirJump = false;
        }
    }

    void Jump(float height)
    {
        //set y velocity to 0 to make our jump more reliable (if we just add the force it will take into account how fast we're falling)
        p_rigidbody.velocity = new Vector2(p_rigidbody.velocity.x, 0.0f);

        //add a force in the Y direction to jump
        p_rigidbody.AddForce(new Vector2(0, height), ForceMode2D.Impulse);
    }
}