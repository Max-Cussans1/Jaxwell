using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public Sprite fireSprite;
    public Sprite waterSprite;
    public Sprite earthSprite;
    public Sprite airSprite;

    public bool pressedFire = false;
    public bool pressedWater = false;
    public bool pressedEarth = false;
    public bool pressedAir = false;

    //animator
    Animator animator;

    void Start()
    {
        //temp solution changing colour until we get sprites/anims
        animator = GetComponent<Animator>();
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
            p_spriteRenderer.sprite = waterSprite;
        }

        if (element == elements.earth)
        {
            ChangeMovementProperties(earthMaxSpeed, earthAcceleration, earthDeceleration);
            p_spriteRenderer.color = Color.green;
            p_spriteRenderer.sprite = earthSprite;
        }

        if (element == elements.air)
        {
            ChangeMovementProperties(airMaxSpeed, airAcceleration, airDeceleration);
            p_spriteRenderer.color = Color.gray;
        }
        if(SaveManager.loadingGame)
        {
            transform.position = SaveManager.position;
            SaveManager.loadingGame = false;
        }
        if (SaveManager.currentSavePath != null)
        {
            //save at the start of the level
            Save();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pressedFire)
        {
            element = elements.fire;
            DebugHelper.Log("Enabled Fire");
            ChangeMovementProperties(fireMaxSpeed, fireAcceleration, fireDeceleration);
            p_spriteRenderer.color = Color.red;
            animator.ResetTrigger("pressedWater");
            animator.ResetTrigger("pressedEarth");
            animator.ResetTrigger("pressedAir");
            animator.SetTrigger("pressedFire");
            pressedFire = false;
        }

        if (pressedWater)
        {
            element = elements.water;
            DebugHelper.Log("Enabled Water");
            ChangeMovementProperties(waterMaxSpeed, waterAcceleration, waterDeceleration);
            p_spriteRenderer.color = Color.blue;
            p_spriteRenderer.sprite = waterSprite;
            animator.ResetTrigger("pressedFire");
            animator.ResetTrigger("pressedEarth");
            animator.ResetTrigger("pressedAir");
            animator.SetTrigger("pressedWater");
            pressedWater = false;
        }

        if (pressedEarth)
        {
            element = elements.earth;
            DebugHelper.Log("Enabled Earth");
            ChangeMovementProperties(earthMaxSpeed, earthAcceleration, earthDeceleration);
            p_spriteRenderer.color = Color.green;
            p_spriteRenderer.sprite = earthSprite;
            animator.ResetTrigger("pressedFire");
            animator.ResetTrigger("pressedWater");
            animator.ResetTrigger("pressedAir");
            animator.SetTrigger("pressedEarth");
            pressedEarth = false;
        }

        if (pressedAir)
        {
            element = elements.air;
            DebugHelper.Log("Enabled Air");
            ChangeMovementProperties(airMaxSpeed, airAcceleration, airDeceleration);
            p_spriteRenderer.color = Color.gray;
            animator.ResetTrigger("pressedFire");
            animator.ResetTrigger("pressedWater");
            animator.ResetTrigger("pressedEarth");
            animator.SetTrigger("pressedAir");
            pressedAir = false;
        }
    }

    //function to change properties
    void ChangeMovementProperties(float maxSpeed, float acceleration, float deceleration)
    {
        moveScript.maxSpeed = maxSpeed;
        moveScript.acceleration = acceleration;
        moveScript.deceleration = deceleration;
    }

    public void Save()
    {
        SaveSystem.Save(this, SaveManager.currentSavePath);
    }
}
