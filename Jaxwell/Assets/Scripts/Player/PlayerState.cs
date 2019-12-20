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

    public bool pressedFire = false;
    public bool pressedWater = false;
    public bool pressedEarth = false;
    public bool pressedAir = false;

    [SerializeField] float timeToSave = 10.0f;
    float tempTimeToSave;

    void Start()
    {
        Load();
        //temp solution changing colour until we get sprites/anims
        p_spriteRenderer = GetComponent<SpriteRenderer>();
        moveScript = GetComponent<MoveScript>();

        if (element == elements.fire)
        {
            ChangeMovementProperties(fireMaxSpeed, fireAcceleration, fireDeceleration);
            p_spriteRenderer.color = Color.red;
        }

        if (element == elements.water)
        {
            ChangeMovementProperties(waterMaxSpeed, waterAcceleration, waterDeceleration);
            p_spriteRenderer.color = Color.blue;
        }

        if (element == elements.earth)
        {
            ChangeMovementProperties(earthMaxSpeed, earthAcceleration, earthDeceleration);
            p_spriteRenderer.color = Color.green;
        }

        if (element == elements.air)
        {
            ChangeMovementProperties(airMaxSpeed, airAcceleration, airDeceleration);
            p_spriteRenderer.color = Color.gray;
        }

        tempTimeToSave = timeToSave;
    }

    // Update is called once per frame
    void Update()
    {
        if (pressedFire)
        {
            element = elements.fire;
            Debug.Log("Enabled Fire");
            ChangeMovementProperties(fireMaxSpeed, fireAcceleration, fireDeceleration);
            p_spriteRenderer.color = Color.red;
            pressedFire = false;
        }

        if (pressedWater)
        {
            element = elements.water;
            Debug.Log("Enabled Water");
            ChangeMovementProperties(waterMaxSpeed, waterAcceleration, waterDeceleration);
            p_spriteRenderer.color = Color.blue;
            pressedWater = false;
        }

        if (pressedEarth)
        {
            element = elements.earth;
            Debug.Log("Enabled Earth");
            ChangeMovementProperties(earthMaxSpeed, earthAcceleration, earthDeceleration);
            p_spriteRenderer.color = Color.green;
            pressedEarth = false;
        }

        if (pressedAir)
        {
            element = elements.air;
            Debug.Log("Enabled Air");
            ChangeMovementProperties(airMaxSpeed, airAcceleration, airDeceleration);
            p_spriteRenderer.color = Color.gray;
            pressedAir = false;
        }

        //save every timeToSave seconds
        if(tempTimeToSave > 0)
        {
            tempTimeToSave -= Time.deltaTime;
            if(tempTimeToSave < 0)
            {
                Save();
                Debug.Log("Game Saved");
                tempTimeToSave = timeToSave;
            }
        }
    }

    //function to change properties
    void ChangeMovementProperties(float maxSpeed, float acceleration, float deceleration)
    {
        moveScript.maxSpeed = maxSpeed;
        moveScript.acceleration = acceleration;
        moveScript.deceleration = deceleration;
    }

    void Save()
    {
        SaveSystem.Save(this);
    }

    void Load()
    {
        PlayerData data = SaveSystem.Load();

        //if we have a save file
        if (SaveSystem.Load() != null)
        {
            element = (elements)data.element;
            Vector3 position;
            position.x = data.position[0];
            position.y = data.position[1];
            position.z = data.position[2];

            transform.position = position;
        }
    }
}
