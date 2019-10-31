using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //store our components here in void start
    Rigidbody2D p_rigidbody;
    SpriteRenderer p_spriteRenderer;
    BoxCollider2D p_collider;


    //important player variables as public variables so we can change in editor
    [SerializeField] float moveSpeed = 3.0f;
    [SerializeField] float jumpHeight = 5.0f;
    [SerializeField] float dashSpeed = 25.0f;
    [SerializeField] float dashCooldown = 6.0f;
    [SerializeField] float dashDuration = 0.1f;
    [SerializeField] int maxHealth = 100;
    [SerializeField] int Lives = 3;

    //the amount we will modify the y size of the collider by when crouching
    float crouchColliderY = 0.5f;
    //variable to store the original collider y size
    float originalColliderYSize;
    //variable to store original y offset of the collider
    float originalColliderYOffset;

    int currentHealth;
    bool dead = false;

    //Vector2 to store the checkpoint (modified in our checkpoint class)
    public Vector2 currentCheckpoint;

    //number of times we jump
    int jumpNumber = 0;
    //if we can dash or not (if it's on cooldown)
    bool canDash = true;
    //if we have dashed right or left
    bool dashedRight = false;
    bool dashedLeft = false;
    //temp variable to store the cooldown every time dash is used
    float tempDashCooldown = 0.0f;
    float tempDashDuration = 0.0f;


    //flag so we know when we can and can't jump - not needed after working in double jump (for now)
    //bool canJump = false;

    //flag to determine if double jump can be activated
    bool canDoubleJump = false;

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
        p_collider = GetComponent<BoxCollider2D>();
        originalColliderYSize = p_collider.size.y;
        originalColliderYOffset = p_collider.offset.y;

        currentHealth = maxHealth;

        //start as fire if no mode selected
        if (!fire && !water && !earth && !air)
        {
            fire = true;
            p_spriteRenderer.color = Color.red;
        }

        //set our current checkpoint at our start position at the start
        currentCheckpoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //check if we can jump before then check for keydown press instead of getkey,
        //since we don't leave jumpable surface in 1 frame (causes multiple jumps)
        if (Input.GetKeyDown("w"))
        {
            //if we have double jump, check if we've jumped twice, if not allow jumping again
            if (canDoubleJump)
            {
                if (jumpNumber < 2)
                {
                    Jump();
                    jumpNumber++;
                }
            }
            //if we don't have double jump, don't let us jump if we have jumped already
            else
            {
                if (jumpNumber < 1)
                {
                    Jump();
                    jumpNumber++;
                }
            }
        }

        //check if we dashed right
        if(dashedRight)
        {
            //set gravity to 0 so short sharp dash in air
            p_rigidbody.gravityScale = 0;
            //countdown from dash duration
            tempDashDuration -= Time.deltaTime;
            //check if duration has completed
            if (tempDashDuration <= 0)
            {
                //set x velocity back to 0 and gravity back to normal
                p_rigidbody.velocity = new Vector2(0.0f, p_rigidbody.velocity.y);
                p_rigidbody.gravityScale = 1;
                dashedRight = false;
            }
        }

        //check if we dashed left
        if(dashedLeft)
        {
            //set gravity to 0 so short sharp dash in air
            p_rigidbody.gravityScale = 0;
            //countdown from dash duration
            tempDashDuration -= Time.deltaTime;
            //check if duration has completed
            if (tempDashDuration <= 0)
            {
                //set x velocity back to 0 and gravity back to normal
                p_rigidbody.velocity = new Vector2(0.0f, p_rigidbody.velocity.y);
                p_rigidbody.gravityScale = 1;
                dashedLeft = false;
            }
        }

        //check if we can dash
        if(!canDash)
        {
            //if we can't we have just dashed and it's on cooldown
            //since we're calling in update, time.deltatime is time since last frame, so -= will give us a countdown
            tempDashCooldown -= Time.deltaTime;
            //if cooldown is finished we can dash again
            if(tempDashCooldown <= 0)
            {                
                canDash = true;
            }
        }

        if (Input.GetKey("d"))
        {
            MoveRight();
        }


        if (Input.GetKey("a"))
        {
            MoveLeft();
        }

        //crouching with getkeydown and getkeyup to use key as a toggle
        if (Input.GetKeyDown("c"))
        {
            //reduce the collider size
            p_collider.size = new Vector2(p_collider.size.x, p_collider.size.y * crouchColliderY);
            //half of half of the full size as an offset below the normal offset
            p_collider.offset = new Vector2(p_collider.offset.x, -originalColliderYSize * 0.25f);
        }
        if(Input.GetKeyUp("c"))
        {
            //if we're not crouching put the collision offset and size back to normal
            p_collider.size = new Vector2(p_collider.size.x, originalColliderYSize);
            p_collider.offset = new Vector2(p_collider.offset.x, originalColliderYOffset);
        }


        if (Input.GetKeyDown("e"))
        {
            if (canDash)
            {
                DashRight();
            }
        }

        if (Input.GetKeyDown("q"))
        {
            if (canDash)
            {
                DashLeft();
            }
        }



        if(dead && Lives > -1)
        {
            //if we aren't out of lives after we die, respawn at the current checkpoint
            Respawn(currentCheckpoint);
        }

        if (fire)
        {
            //disable double jump if it's active if we're in any other mode apart from air
            if (canDoubleJump == true)
            {
                canDoubleJump = false;
            }
        }

        if(water)
        {
            //disable double jump if it's active if we're in any other mode apart from air
            if (canDoubleJump == true)
            {
                canDoubleJump = false;
            }
        }

        if(earth)
        {
            //disable double jump if it's active if we're in any other mode apart from air
            if (canDoubleJump == true)
            {
                canDoubleJump = false;
            }
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

            //Enable double jump if we're in air mode
            if (canDoubleJump == false)
            {
                canDoubleJump = true;
            }
        }
        #endregion
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        //FOR JUMP LOGIC
        //create 2D raycast fired directly down from player's position
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);
        //if the raycast hits something
        if (hit.collider != null)
        {
           //print what the raycast hit to console and where the object is
           Debug.Log("Raycast hit " + hit.collider.gameObject + " at " + hit.transform.position);
           
           //check if the raycast hits the thing we are colliding with (so we know it is underneath us)
           if (hit.collider == collision.collider)
           {
                //check if we are touching a jumpable surface (add tag in editor to surface objects' parent)
                if (collision.gameObject.transform.parent.CompareTag("JumpableSurface"))
                {
                    //print the gameobject with jumpablesurface tag and where it is
                    Debug.Log("Touched Jumpable Surface at " + collision.transform.position);

                    //canJump = true;
                    //reset the number of times we have jumped
                    jumpNumber = 0;
                }
            }
        }
    }


    //Don't need this after working in double jump (for now)
   // void OnCollisionExit2D(Collision2D collision)
   // {
   //     //check if we have left a jumpable surface (add tag in editor to surface objects' parent)
   //     if (collision.gameObject.transform.parent.CompareTag("JumpableSurface"))
   //     {
   //        //Debug.Log("Left Jumpable Surface");
   //        //canJump = false;
   //        //if(canDoubleJump == true)
   //        //{
   //        //    canJump = true;
   //        //}
   //     }
   // }



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

    void DashRight()
    {
        Debug.Log("Dashed Right");
        //set x velocity to 0 to make our dash more reliable
        p_rigidbody.velocity = new Vector2(0.0f, p_rigidbody.velocity.y);
        //add a force in the positive X direction to dash right
        p_rigidbody.AddForce(new Vector2(dashSpeed, 0), ForceMode2D.Impulse);
        //turn dashedRight on
        dashedRight = true;
        //Set on cooldown
        canDash = false;
        tempDashCooldown = dashCooldown;
        tempDashDuration = dashDuration;
    }

    void DashLeft()
    {
        Debug.Log("Dashed Left");
        //set x velocity to 0 to make our dash more reliable
        p_rigidbody.velocity = new Vector2(0.0f, p_rigidbody.velocity.y);
        //add a force in the positive X direction to dash right
        p_rigidbody.AddForce(new Vector2(-dashSpeed, 0), ForceMode2D.Impulse);
        //turn dashedLeft on
        dashedLeft = true;
        //Set on cooldown
        canDash = false;
        tempDashCooldown = dashCooldown;
        tempDashDuration = dashDuration;
    }

    //NOTE: if we want to change jump speed at runtime these could be changed to pass in speed as a parameter
    //function to jump
    void Jump()
    {
       Debug.Log("Jumped!");      
        //set y velocity to 0 to make our jump more reliable (if we just add the force it will take into account how fast we're falling)
        p_rigidbody.velocity = new Vector2(p_rigidbody.velocity.x, 0.0f);
        //add a force in the Y direction to jump
        p_rigidbody.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
    }

    //simple function to take damage passes in the amount of damage to take as a parameter
    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player took " + damage + " damage");
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        dead = true;        
        Lives--;
        Debug.Log("Died!");
        //DO YOU ARE DEAD STUFF
        if(Lives == -1)
        {
            //lose level logic
        }
        else
        {
            //if we're not out of lives we can respawn
            Respawn(currentCheckpoint);
        }

    }

    void Respawn(Vector2 respawnLocation)
    {
        //essentially just moves the player to this location with full health and no velocity         
        transform.position = respawnLocation;
        p_rigidbody.velocity = new Vector2(0,0);
        currentHealth = maxHealth;
        dead = false;
        Debug.Log("Respawning at " + respawnLocation);
    }
        

}
