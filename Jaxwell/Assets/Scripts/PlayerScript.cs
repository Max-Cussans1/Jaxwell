using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //store our components here in void start
    Rigidbody2D p_rigidbody;
    SpriteRenderer p_spriteRenderer;
    BoxCollider2D p_collider;
    float initialGravityScale;
    Destructible destructible;
    float startMeleeAttackSpeed = 0f;


    //important player variables as serlialized variables so we can change in editor
    [SerializeField] float maxSpeed = 3.0f;
    [SerializeField] float acceleration = 0.3f;
    [SerializeField] float deceleration = 0.3f;
    [SerializeField] float jumpHeight = 5.0f;
    [SerializeField] float dashSpeed = 25.0f;
    [SerializeField] float dashCooldown = 6.0f;
    [SerializeField] float dashDuration = 0.1f;
    [SerializeField] int maxHealth = 100;
    [SerializeField] int Lives = 3;
    [SerializeField] float meleeAttackSpeed = 1.0f; //time in between attacks
    [SerializeField] int earthDamage = 50;  //damage we do to destructibles
    [SerializeField] float earthFallForce = 35.0f;  //force to fall at

    bool canAttack = true;
    bool accelerating = false;


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

    bool canDestroy = false;


    //flag so we know when we can and can't jump - not needed after working in double jump (for now)
    //bool canJump = false;

    //flag to determine if double jump can be activated
    bool canDoubleJump = false;

    bool earthFalling = false;

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
        startMeleeAttackSpeed = meleeAttackSpeed;
        initialGravityScale = p_rigidbody.gravityScale;

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
        //check if we've attacked, wait for the attack speed if not
        if(!canAttack)
        {
            meleeAttackSpeed -= Time.deltaTime;
            if(meleeAttackSpeed < 0)
            {
                //set the attack speed back to its original value
                meleeAttackSpeed = startMeleeAttackSpeed;
                canAttack = true;
            }
        }

        //check if we can jump before then check for keydown press instead of getkey,
        //since we don't leave jumpable surface in 1 frame (causes multiple jumps)
        if (Input.GetKeyDown(KeyCode.Space))
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
                p_rigidbody.gravityScale = initialGravityScale;
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
                p_rigidbody.gravityScale = initialGravityScale;
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

        //check if we're already going maxSpeed
        if (p_rigidbody.velocity.x < maxSpeed)
        {
            if (Input.GetKey(KeyCode.D))
            {
                AccelerateRight();                

                //if this pushes us past our maxspeed stay at maxspeed
                if (p_rigidbody.velocity.x > maxSpeed)
                {
                    p_rigidbody.velocity = new Vector2(maxSpeed, p_rigidbody.velocity.y);
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            accelerating = false;
        }

        //check if we're already going maxSpeed
        if (p_rigidbody.velocity.x > -maxSpeed)        
        {
            if (Input.GetKey(KeyCode.A))
            {
                AccelerateLeft();                

                //if this pushes us past our maxspeed stay at maxspeed
                if (p_rigidbody.velocity.x < -maxSpeed)
                {
                    p_rigidbody.velocity = new Vector2(-maxSpeed, p_rigidbody.velocity.y);
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            accelerating = false;
        }

        //decelerate if we aren't putting input in for left or right
        if(accelerating == false)
        {
            Decelerate();
        }



        //crouching with getkeydown and getkeyup to use key as a toggle
        if (Input.GetKeyDown(KeyCode.C))
        {
            Crouch();
        }
        if(Input.GetKeyUp(KeyCode.C))
        {
            Uncrouch();
        }


        if (fire)
        {
            //disable double jump if it's active if we're in any other mode apart from air
            if (canDoubleJump == true)
            {
                canDoubleJump = false;
            }

            //dash right
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (canDash)
                {
                    DashRight();
                }
            }

            //dash left
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (canDash)
                {
                    DashLeft();
                }
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

            if (canAttack)
            {
                //check if our bool has been made true by colliding with destructibles
                if (canDestroy)
                {
                    //check the destructible isn't dead already
                    if (destructible.health > 0)
                    {
                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            //deal damage on the key press
                            destructible.health -= earthDamage;
                            Debug.Log("Damaged Destructible at " + destructible.transform.position + " for " + earthDamage + " - it has " + destructible.health + " remaining");
                            //can't attack because we have just attacked
                            canAttack = false;
                        }
                    }
                }
            }
            
            if(Input.GetKeyDown(KeyCode.E))
            {
                //call our earthfall function
                EarthFall(earthFallForce);                                
            }            
        }


        if (air)
        {
            //TODO
        }

        #region Enabling/Disabling elements (using keybinds for now)
        if(Input.GetKey(KeyCode.Keypad1))
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

        if (Input.GetKey(KeyCode.Keypad2))
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

        if (Input.GetKey(KeyCode.Keypad3))
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

        if (Input.GetKey(KeyCode.KeypadEnter))
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
        Debug.Log("Collided with " + collision.collider.gameObject + " at " + collision.transform.position);

        //FOR JUMP LOGIC
        //create 2D raycast fired directly down from player's centre just a tiny bit further than our player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, (p_collider.size.y * 0.5f) + 0.05f);
        //if the raycast hits something
        if (hit.collider != null)
        {
           //print what the raycast hit to console and where the object is
           Debug.Log("Raycast from centre hit " + hit.collider.gameObject + " at " + hit.point);

           //reset our jumpNumber
           jumpNumber = 0;

            if (earthFalling)
            {
                //set collision detection mode back to discrete from continuous to avoid overhead
                p_rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
                Debug.Log("Enabling discrete collision because earthfall stopped");
                //end falling fast
                earthFalling = false;
            }
        }
        else
        {
            //fire a raycast from the left side of the player if the centre might not hit; 0.4f to avoid raycasting from the very edge, may lead to false positives when we hit something with the left side
            RaycastHit2D hitLeft = Physics2D.Raycast(new Vector2(transform.position.x - (p_collider.size.x * 0.4f), transform.position.y), -Vector2.up, (p_collider.size.y * 0.5f) + 0.05f);

            if(hitLeft.collider != null)
            {
                //print what the raycast hit to console and where the object is
                Debug.Log("Raycast from left hit " + hitLeft.collider.gameObject + " at " + hitLeft.point);

                //reset our jumpNumber
                jumpNumber = 0;

                if (earthFalling)
                {
                    //set collision detection mode back to discrete from continuous to avoid overhead
                    p_rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
                    Debug.Log("Enabling discrete collision because earthfall stopped");
                    //end falling fast
                    earthFalling = false;
                }
            }
            else
            {
                //fire a raycast from the right side of the player if the centre or left might not hit; 0.4f to avoid raycasting from the very edge, may lead to false positives when we hit something with the right side
                RaycastHit2D hitRight = Physics2D.Raycast(new Vector2(transform.position.x + (p_collider.size.x * 0.4f), transform.position.y), -Vector2.up, (p_collider.size.y * 0.5f) + 0.05f);
                
                if(hitRight.collider != null)
                {
                    //print what the raycast hit to console and where the object is
                    Debug.Log("Raycast from right hit " + hitRight.collider.gameObject + " at " + hitRight.point);

                    //reset our jumpNumber
                    jumpNumber = 0;

                    if (earthFalling)
                    {
                        //set collision detection mode back to discrete from continuous to avoid overhead
                        p_rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
                        Debug.Log("Enabling discrete collision because earthfall stopped");
                        //end falling fast
                        earthFalling = false;
                    }
                }
            }
        }

        //check if we are colliding with a destructible
        if (collision.gameObject.GetComponent<Destructible>() != null)
        {
            Debug.Log("Collided with Destructible at " + collision.gameObject.transform.position);
            //store the destructible we are colliding with in that local variable
            destructible = collision.gameObject.GetComponent<Destructible>();
            canDestroy = true;
        }
    }



    void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("Stopped colliding with " + collision.collider.gameObject + " at " + collision.transform.position);

        //check if we've stopped colliding with a destructible
        if (collision.gameObject.GetComponent<Destructible>() != null)
        {
            Debug.Log("Exited collision with Destructible at " + collision.gameObject.transform.position);
            canDestroy = false;
        }
    }



    //NOTE: if we want to change movement speed at runtime these could be changed to pass in speed as a parameter
    //function to move right
    void AccelerateRight()
    {
        //add acceleration in the positive x direction for our rigidbody's velocity
        p_rigidbody.velocity = new Vector2(p_rigidbody.velocity.x + acceleration, p_rigidbody.velocity.y);
        accelerating = true;
    }

    //NOTE: if we want to change movement speed at runtime these could be changed to pass in speed as a parameter
    //function to move left
    void AccelerateLeft()
    {
        //add acceleration in the negative x direction for our rigidbody's velocity
        p_rigidbody.velocity = new Vector2(p_rigidbody.velocity.x - acceleration, p_rigidbody.velocity.y);
        accelerating = true;
    }

    void Decelerate()
    {
        //if we're less than our deceleration speed to stopping set our velocity to 0
        if(p_rigidbody.velocity.x <= deceleration || p_rigidbody.velocity.x >= -deceleration)
        {
            p_rigidbody.velocity = new Vector2(0, p_rigidbody.velocity.y);
        }

        //if we have a negative velocity, accelerate in positive direction
        if (p_rigidbody.velocity.x < 0)
        {
            p_rigidbody.velocity = new Vector2(p_rigidbody.velocity.x + deceleration, p_rigidbody.velocity.y);
        }
        //if we have a positive velocity, accelerate in negative direction
        else if (p_rigidbody.velocity.x > 0)
        {
            p_rigidbody.velocity = new Vector2(p_rigidbody.velocity.x - deceleration, p_rigidbody.velocity.y);
        }
        
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

    void EarthFall(float force)
    {
        Debug.Log("Earthfall!");
        //set velocity to 0 to make our fall straight down
        p_rigidbody.velocity = new Vector2(0.0f, 0.0f);
        //add a force in the negative Y direction to fall down fast
        p_rigidbody.AddForce(new Vector2(0, -force), ForceMode2D.Impulse);
        earthFalling = true;
        //change to continuous collision because we might be falling really fast
        p_rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        Debug.Log("Enabled continuous collision for earthfall");
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

    void Crouch()
    {
        Debug.Log("Crouched!");
        //reduce the collider size
        p_collider.size = new Vector2(p_collider.size.x, p_collider.size.y * crouchColliderY);
        //half of half of the full size as an offset below the normal offset
        p_collider.offset = new Vector2(p_collider.offset.x, -originalColliderYSize * 0.25f);
    }

    void Uncrouch()
    {
        Debug.Log("Uncrouched!");
        //if we're not crouching put the collision offset and size back to normal
        p_collider.size = new Vector2(p_collider.size.x, originalColliderYSize);
        p_collider.offset = new Vector2(p_collider.offset.x, originalColliderYOffset);
    }
        

}
