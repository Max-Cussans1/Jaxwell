using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //public so we can change in editor
    public float moveSpeed = 3.0f;
    public float jumpHeight = 5.0f;

    //flag so we know when we can and can't jump
    bool canJump = false;

    // Update is called once per frame
    void Update()
    {     
        //check if we can jump before then check for keydown press instead of getkey, since we don't leave jumpable surface in 1 frame (causes multiple jumps)
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
    }

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        //check if we are touching a jumpable surface
        if (collision.gameObject.CompareTag("JumpableSurface"))
        {
            Debug.Log("Touched Jumpable Surface");
            canJump = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {        
        //check if we have left a jumpable surface
        if (collision.gameObject.CompareTag("JumpableSurface"))
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
        transform.position = new Vector3(temp, transform.position.y, transform.position.z);
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
        transform.position = new Vector3(temp, transform.position.y, transform.position.z);
    }

    //NOTE: if we want to change jump speed at runtime these could be changed to pass in speed as a parameter
    //function to jump
    void Jump()
    {
       Debug.Log("Jumped!");
        //add a force in the Y direction to jump
       GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
    }

}
