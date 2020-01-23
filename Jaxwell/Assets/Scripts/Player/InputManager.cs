using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    DashScript dashScript;
    EarthDash earthDash;
    JumpScript jumpScript;
    PlayerState playerState;
    WallClimb wallClimb;

    // Start is called before the first frame update
    void Start()
    {
        dashScript = GetComponent<DashScript>();
        earthDash = GetComponent<EarthDash>();
        jumpScript = GetComponent<JumpScript>();
        playerState = GetComponent<PlayerState>();    
        wallClimb = GetComponent<WallClimb>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            //manage input when we press fire
            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetAxis("LeftTrigger") > 0)
            {
                //if we aren't in fire already, dash
                if (playerState.element != Elements.elements.fire && dashScript.canDash)
                {
                    dashScript.pressedDash = true;
                }
                playerState.pressedFire = true;
            }

            //manage imput when we press water
            if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown("joystick button 4"))
            {
                playerState.pressedWater = true;
            }

            //manage input when we press earth
            if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetAxis("RightTrigger") > 0)
            {
                //if we aren't in earth already, earth dash
                if (playerState.element != Elements.elements.earth)
                {
                    EarthDash.pressedDashToEarth = true;
                }

                playerState.pressedEarth = true;
            }

            //manage input when we press air
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown("joystick button 5"))
            {
                if (playerState.element != Elements.elements.air)
                {
                    if (!jumpScript.usedAirJump)
                    {
                        jumpScript.pressedAirJump = true;
                    }
                }

                playerState.pressedAir = true;
            }

            //manage input when we press jump
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0"))
            {
                //if we're grounded and not earth form, jump
                if (CollisionManager.isGrounded == true && playerState.element != Elements.elements.earth)
                {
                    jumpScript.pressedJump = true;                    
                }

                //if grabbing use walljump
                if (WallClimb.grabbing)
                {
                    wallClimb.pressedWallJump = true;
                }

                //if we're in pre-jump range (about to land)
                if(CollisionManager.preJump)
                {
                    jumpScript.pressedPreJump = true;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {            
            if(Time.timeScale == 1)
            {
                Debug.Log("Paused");
                Time.timeScale = 0;
            }
            else if (Time.timeScale == 0)
            {
                Debug.Log("Unpaused");
                Time.timeScale = 1;
            }
        }
    }
}
