using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : Elements
{
    //change to the element we want to start with in editor
    public elements element = 0;
    elements platformElement;

    [SerializeField] float fireMaxSpeed = 3.0f;
    [SerializeField] float fireAcceleration = 0.3f;
    [SerializeField] float fireDeceleration = 0.3f;

    [SerializeField] float waterMaxSpeed = 3.0f;
    [SerializeField] float waterAcceleration = 0.3f;
    [SerializeField] float waterDeceleration = 0.3f;

    [SerializeField] float earthMaxSpeed = 1.5f;
    [SerializeField] float earthAcceleration = 0.05f;
    [SerializeField] float earthDeceleration = 0.3f;

    [SerializeField] float airMaxSpeed = 3.0f;
    [SerializeField] float airAcceleration = 0.3f;
    [SerializeField] float airDeceleration = 0.3f;

    MoveScript moveScript;
    SpriteRenderer p_spriteRenderer;

    bool changedProperties = false;

    void Start()
    {
        //temp solution changing colour until we get sprites/anims
        p_spriteRenderer = GetComponent<SpriteRenderer>();
        moveScript = GetComponent<MoveScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            element = elements.fire;
            Debug.Log("Enabled Fire");
            changedProperties = false;
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            element = elements.water;
            Debug.Log("Enabled Water");
            changedProperties = false;
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            element = elements.earth;
            Debug.Log("Enabled Earth");
            changedProperties = false;
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            element = elements.air;
            Debug.Log("Enabled Air");
            changedProperties = false;
        }

        if(element == elements.fire && !changedProperties)
        {
            ChangeMovementProperties(fireMaxSpeed, fireAcceleration, fireDeceleration);
            p_spriteRenderer.color = Color.red;
            changedProperties = true;
        }

        if (element == elements.water && !changedProperties)
        {
            ChangeMovementProperties(waterMaxSpeed, waterAcceleration, waterDeceleration);
            p_spriteRenderer.color = Color.blue;
            changedProperties = true;
        }

        if (element == elements.earth && !changedProperties)
        {
            ChangeMovementProperties(earthMaxSpeed, earthAcceleration, earthDeceleration);
            p_spriteRenderer.color = Color.green;
            changedProperties = true;
        }

        if (element == elements.air && !changedProperties)
        {
            ChangeMovementProperties(airMaxSpeed, airAcceleration, airDeceleration);
            p_spriteRenderer.color = Color.gray;
            changedProperties = true;
        }
    }

    void ChangeMovementProperties(float maxSpeed, float acceleration, float deceleration)
    {
        moveScript.maxSpeed = maxSpeed;
        moveScript.acceleration = acceleration;
        moveScript.deceleration = deceleration;
    }
}
