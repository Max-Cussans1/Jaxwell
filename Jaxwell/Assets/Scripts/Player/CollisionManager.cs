using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public static bool isGrounded = false;
    public static bool isAgainstWallLeft = false;
    public static bool isAgainstWallRight = false;
    JumpScript jumpScript;

    [SerializeField] float coyoteTime = 0.1f;

    float raycastStartOffset;
    float raycastDistance;

    float tempCoyoteTime;
    bool coyoteTimeActive = false;
    public static bool jumped = false;

    BoxCollider2D p_collider;

    void Start()
    {
        p_collider = GetComponent<BoxCollider2D>();
        //the distance from centre of the collider we will check left/right and top/bottom
        raycastStartOffset = p_collider.size.y * 0.4f;
        //the distance the raycast will travel (just further than the collider)
        raycastDistance = (p_collider.size.y * 0.5f) +0.05f;

        tempCoyoteTime = coyoteTime;
    }

    void Update()
    {
        if (coyoteTimeActive)
        {
            Debug.Log("Coyote time is active");
            isGrounded = true;

            //start counting down our coyote time
            tempCoyoteTime -= Time.deltaTime;
            if(tempCoyoteTime <= 0)
            {
                Debug.Log("Coyote time ended");
                coyoteTimeActive = false;
                //set isGrounded to false when coyote time is over
                isGrounded = false;
                //reset coyote time
                tempCoyoteTime = coyoteTime;
            }

            //if we jump disable coyote time straight away
            if (jumped)
            {
                Debug.Log("Coyote time no longer active because we jumped during coyote time");
                coyoteTimeActive = false;
                //set isGrounded to false when coyote time is over
                isGrounded = false;
                //reset coyote time
                tempCoyoteTime = coyoteTime;
                jumped = false;
            }
        }

    }

    //run checks when we collide with something
    void OnCollisionEnter2D(Collision2D collision)
    {        
        isAgainstWallRight = WallCheckRight();

        //if we didn't hit anything with our raycasts to the right, try left   
        if (!isAgainstWallRight)
        {
            isAgainstWallLeft = WallCheckLeft();
        }
        isGrounded = GroundedCheck();
    }

    //run wallcheck when we stay colliding with something
    void OnCollisionStay2D(Collision2D collision)
    {
        isAgainstWallRight = WallCheckRight();

        //if we didn't hit anything with our raycasts to the right, try left   
        if (!isAgainstWallRight)
        {
            isAgainstWallLeft = WallCheckLeft();
        }

        isGrounded = GroundedCheck();
    }

    //run checks when we exit collision with something
    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
        isAgainstWallRight = false;
        isAgainstWallLeft = false;
        
    }

    private bool GroundedCheck()
    {
        bool isGroundedCheck = false;

        //create 2D raycast fired directly down from player's centre just a tiny bit further than our player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, raycastDistance);
        //if the raycast hits something
        if (hit.collider != null)
        {
            //print what the raycast hit to console and where the object is
            Debug.Log("Raycast to check if we're grounded from " + p_collider.gameObject + " hit " + hit.collider.gameObject + " at " + hit.point);
            isGroundedCheck = true;
        }
        else
        {
            //fire a raycast from the left side of the player if the centre might not hit
            RaycastHit2D hitLeft = Physics2D.Raycast(new Vector2(transform.position.x - raycastStartOffset, transform.position.y), -Vector2.up, raycastDistance);

            if (hitLeft.collider != null)
            {
                //print what the raycast hit to console and where the object is
                Debug.Log("Left raycast to check if we're grounded from " + p_collider.gameObject + " hit " + hitLeft.collider.gameObject + " at " + hitLeft.point);
                isGroundedCheck = true;
            }
            else
            {
                //fire a raycast from the right side of the player if the centre or left might not hit
                RaycastHit2D hitRight = Physics2D.Raycast(new Vector2(transform.position.x + raycastStartOffset, transform.position.y), -Vector2.up, raycastDistance);

                if (hitRight.collider != null)
                {
                    //print what the raycast hit to console and where the object is
                    Debug.Log("Right raycast to check if we're grounded from " + p_collider.gameObject + " hit " + hitRight.collider.gameObject + " at " + hitRight.point);
                    isGroundedCheck = true;
                }
            }
        }
        //if we're not grounded or against any walls after doing a ground check enable coyote time
        if(!isGroundedCheck && !isAgainstWallLeft && !isAgainstWallRight)
        {
            Debug.Log("Raycasts to check if we're grounded from " + p_collider.gameObject + " didn't hit anything, enabling coyote time");
            coyoteTimeActive = true;
        }
        return isGroundedCheck;
    }

    private bool WallCheckRight()
    {
        bool isWallCheck = false;

        //create 2D raycast fired directly right from player's centre just a tiny bit further than our player
        RaycastHit2D grabRightHit = Physics2D.Raycast(transform.position, Vector2.right, raycastDistance);
        //if the raycast hits something
        if (grabRightHit.collider != null)
        {
            //print what the raycast hit to console and where the object is
            Debug.Log("Grab raycast right from centre hit " + grabRightHit.collider.gameObject + " at " + grabRightHit.point);
            isWallCheck = true;

        }
        else
        {
            //fire a raycast from the bottom side of the player if the centre might not hit
            RaycastHit2D grabRightHitBottom = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - raycastStartOffset), Vector2.right, raycastDistance);

            if (grabRightHitBottom.collider != null)
            {
                //print what the raycast hit to console and where the object is
                Debug.Log("Grab raycast right from bottom hit " + grabRightHitBottom.collider.gameObject + " at " + grabRightHitBottom.point);
                isWallCheck = true;

            }
            else
            {
                //fire a raycast from the top side of the player if the centre or bottom might not hit
                RaycastHit2D grabRightHitTop = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + raycastStartOffset), Vector2.right, raycastDistance);

                if (grabRightHitTop.collider != null)
                {
                    //print what the raycast hit to console and where the object is
                    Debug.Log("Grab raycast right from top hit " + grabRightHitTop.collider.gameObject + " at " + grabRightHitTop.point);
                    isWallCheck = true;

                }             
            }
        }

        return isWallCheck;
    }

    private bool WallCheckLeft()
    {
        bool isWallCheck = false;

            
        //create 2D raycast fired directly left from player's centre just a tiny bit further than our player
        RaycastHit2D grabLeftHit = Physics2D.Raycast(transform.position, -Vector2.right, raycastDistance);
        //if the raycast hits something
        if (grabLeftHit.collider != null)
        {
            //print what the raycast hit to console and where the object is
            Debug.Log("Grab raycast left from centre hit " + grabLeftHit.collider.gameObject + " at " + grabLeftHit.point);
            isWallCheck = true;

        }
        else
        {
            //fire a raycast from the bottom side of the player if the centre might not hit; 0.4f to avoid raycasting from the very edge, may lead to false positives when we hit something with the bottom side
            RaycastHit2D grabLeftHitBottom = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - raycastStartOffset), -Vector2.right, raycastDistance);

            if (grabLeftHitBottom.collider != null)
            {
                //print what the raycast hit to console and where the object is
                Debug.Log("Grab raycast left from bottom hit " + grabLeftHitBottom.collider.gameObject + " at " + grabLeftHitBottom.point);
                isWallCheck = true;

            }
            else
            {
                //fire a raycast from the top side of the player if the centre or bottom might not hit; 0.4f to avoid raycasting from the very edge, may lead to false positives when we hit something with the right side
                RaycastHit2D grabLeftHitTop = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + raycastStartOffset), -Vector2.right, raycastDistance);

                if (grabLeftHitTop.collider != null)
                {
                    //print what the raycast hit to console and where the object is
                    Debug.Log("Grab raycast left top hit " + grabLeftHitTop.collider.gameObject + " at " + grabLeftHitTop.point);
                    isWallCheck = true;

                }
            }
        }

        return isWallCheck;
    }
}


