using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //store our components here in void start
    Rigidbody2D p_rigidbody;
    SpriteRenderer p_spriteRenderer;


    //public so we can change in editor
    public float moveSpeed = 3.0f;
    public float jumpHeight = 5.0f;


    //flag so we know when we can and can't jump
    bool canJump = false;

    //public bools so we can access from other scripts
    public bool fire = false;
    public bool water = false;
    public bool earth = false;
    public bool air = false;


    void Start()
    {
        //get components in Start so we only have to do that once
        p_spriteRenderer = GetComponent<SpriteRenderer>();
        p_rigidbody = GetComponent<Rigidbody2D>();

        //start as fire
        fire = true;
        p_spriteRenderer.color = Color.red;

    }

    // Update is called once per frame
    void Update()
    {
        //check if we can jump before then check for keydown press instead of getkey,
        //since we don't leave jumpable surface in 1 frame (causes multiple jumps)
        if (canJump && Input.GetKeyDown("w"))
        {
            Jump();
        }

        if (Input.GetKey("d"))
        {
            MoveRight();
        }


        if (Input.GetKey("a"))
        {
            MoveLeft();
        }

        if(fire)
        {
            //TODO
        }

        if(water)
        {
            //TODO
        }

        if(earth)
        {
            //TODO
        }

        if(air)
        {
            //TODO
        }

        #region Enabling/Disabling elements (using keybinds for now)
        if(Input.GetKey("1"))
        {
            if(water == true)
            {
                Debug.Log("Water Disabled for Fire");
                water = false;
            }

            if(earth == true)
            {
                Debug.Log("Earth Disabled for Fire");
                earth = false;
            }

            if(air == true)
            {
                Debug.Log("Air Disabled for Fire");
                air = false;
            }

            if (fire != true)
            {
                Debug.Log("Fire Enabled");
                fire = true;
                p_spriteRenderer.color = Color.red;
            }
        }

        if (Input.GetKey("2"))
        {
            if (fire == true)
            {
                Debug.Log("Fire Disabled for Water");
                fire = false;
            }

            if (earth == true)
            {
                Debug.Log("Earth Disabled for Water");
                earth = false;
            }

            if (air == true)
            {
                Debug.Log("Air Disabled for Water");
                air = false;
            }

            if (water != true)
            {
                Debug.Log("Water Enabled");
                water = true;
                p_spriteRenderer.color = Color.blue;
            }
        }

        if (Input.GetKey("3"))
        {
            if (water == true)
            {
                Debug.Log("Water Disabled for Earth");
                water = false;
            }

            if (fire == true)
            {
                Debug.Log("Fire Disabled for Earth");
                fire = false;
            }

            if (air == true)
            {
                Debug.Log("Air Disabled for Earth");
                air = false;
            }

            if (earth != true)
            {
                Debug.Log("Earth Enabled");
                earth = true;
                p_spriteRenderer.color = Color.green;
            }
        }

        if (Input.GetKey("4"))
        {
            if (water == true)
            {
                Debug.Log("Water Disabled for Air");
                water = false;
            }

            if (earth == true)
            {
                Debug.Log("Earth Disabled for Air");
                earth = false;
            }

            if (fire == true)
            {
                Debug.Log("Fire Disabled for Air");
                fire = false;
            }

            if (air != true)
            {
                Debug.Log("Air Enabled");
                air = true;
                p_spriteRenderer.color = Color.gray;
            }
        }
        #endregion
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        //check if we are touching a jumpable surface (add tag in editor to surface objects' parent)
        if (collision.gameObject.transform.parent.CompareTag("JumpableSurface"))
        {
            Debug.Log("Touched Jumpable Surface");
            canJump = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        //check if we have left a jumpable surface (add tag in editor to surface objects' parent)
        if (collision.gameObject.transform.parent.CompareTag("JumpableSurface"))
        {
            Debug.Log("Left Jumpable Surface");
            canJump = false;
        }
    }

    //NOTE: if we want to change movement speed at runtime these could be changed to pass in speed as a parameter
    //function to move right
    void MoveRight()
    {
        //get x position
        float temp = transform.position.x;
        //update the temp variable in the right direction, * Time.deltaTime for smoothing
        temp = temp + (0.5f * Time.deltaTime * moveSpeed);
        //change position by passing temp in for our x position
        transform.position = new Vector2(temp, transform.position.y);
    }

    //NOTE: if we want to change movement speed at runtime these could be changed to pass in speed as a parameter
    //function to move left
    void MoveLeft()
    {
        //get x position
        float temp = transform.position.x;
        //update the temp variable in the left direction, * Time.deltaTime for smoothing
        temp = temp - (0.5f * Time.deltaTime * moveSpeed);
        //change position by passing temp in for our x position
        transform.position = new Vector2(temp, transform.position.y);
    }

    //NOTE: if we want to change jump speed at runtime these could be changed to pass in speed as a parameter
    //function to jump
    void Jump()
    {
       Debug.Log("Jumped!");
        //add a force in the Y direction to jump
       p_rigidbody.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
    }


}
