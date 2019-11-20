using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public static bool isGrounded = false;
    public static bool isAgainstWall = false;

    BoxCollider2D p_collider;

    void Start()
    {
        p_collider = GetComponent<BoxCollider2D>();
    }

    //run checks when we collide with something
    void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = GroundedCheck();
        isAgainstWall = WallCheck();
    }

    //run wallcheck when we stay colliding with something
    void OnCollisionStay2D(Collision2D collision)
    {
        isAgainstWall = WallCheck();
    }

    //run checks when we exit collision with something
    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = GroundedCheck();
        isAgainstWall = WallCheck();
    }

    private bool GroundedCheck()
    {
        bool isGroundedCheck = false;

        //create 2D raycast fired directly down from player's centre just a tiny bit further than our player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, (p_collider.size.y * 0.5f) + 0.05f);
        //if the raycast hits something
        if (hit.collider != null)
        {
            //print what the raycast hit to console and where the object is
            Debug.Log("Raycast to check if we're grounded hit " + hit.collider.gameObject + " at " + hit.point);
            isGroundedCheck = true;
        }
        else
        {
            //fire a raycast from the left side of the player if the centre might not hit; 0.4f to avoid raycasting from the very edge, may lead to false positives when we hit something with the left side
            RaycastHit2D hitLeft = Physics2D.Raycast(new Vector2(transform.position.x - (p_collider.size.x * 0.4f), transform.position.y), -Vector2.up, (p_collider.size.y * 0.5f) + 0.05f);

            if (hitLeft.collider != null)
            {
                //print what the raycast hit to console and where the object is
                Debug.Log("Jump raycast from left hit " + hitLeft.collider.gameObject + " at " + hitLeft.point);
                isGroundedCheck = true;
            }
            else
            {
                //fire a raycast from the right side of the player if the centre or left might not hit; 0.4f to avoid raycasting from the very edge, may lead to false positives when we hit something with the right side
                RaycastHit2D hitRight = Physics2D.Raycast(new Vector2(transform.position.x + (p_collider.size.x * 0.4f), transform.position.y), -Vector2.up, (p_collider.size.y * 0.5f) + 0.05f);

                if (hitRight.collider != null)
                {
                    //print what the raycast hit to console and where the object is
                    Debug.Log("Jump raycast from right hit " + hitRight.collider.gameObject + " at " + hitRight.point);
                    isGroundedCheck = true;
                }
            }
        }
        return isGroundedCheck;
    }

    private bool WallCheck()
    {
        bool isWallCheck = false;

        return isWallCheck;
    }
}


