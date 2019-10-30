using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    PlayerScript player;

    Camera maincamera;

    //variables to get mouse position
    Vector3 mousePosition;
    Vector3 worldMousePosition;

    //add projectile prefab in editor
    public Rigidbody2D fireProjectile;
    public Rigidbody2D waterProjectile;
    public Rigidbody2D earthProjectile;
    public Rigidbody2D airProjectile;

    //can edit speed of each element's projectule in editor
    public float fireProjectileSpeed = 100.0f;
    public float waterProjectileSpeed = 100.0f;
    public float earthProjectileSpeed = 100.0f;
    public float airProjectileSpeed = 100.0f;


    void Start()
    {
        //get the playerscript (don't need to reference a gameobject since this script will also be on the player)
        player = GetComponent<PlayerScript>();
        //only grab this once because camera.main is slow
        maincamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }


    void Shoot()
    {
        //declare a rigidbody for our projectile to be instantiated
        Rigidbody2D projectile;

        //grab the mouse position
        mousePosition = Input.mousePosition;
        //grab the mouse position from our camera
        worldMousePosition = maincamera.ScreenToWorldPoint(mousePosition);

        //grab the direction in terms of the player to shoot in based on our mouse position
        Vector2 direction = new Vector2(worldMousePosition.x - transform.position.x, worldMousePosition.y - transform.position.y);

        //shoot different projectiles based on what element we are at the time
        if (player.fire == true)
        {
            //instantiate at the player's position and at player's rotation
            projectile = Instantiate(fireProjectile, transform.position, transform.rotation);
            Debug.Log("Fire shot fired!");

            //add the force in the correct direction
            projectile.AddForce(direction * fireProjectileSpeed);
        }

        if(player.water == true)
        {
            //instantiate at the player's position and at player's rotation
            projectile = Instantiate(waterProjectile, transform.position, transform.rotation);
            Debug.Log("Water shot fired!");

            //add the force in the correct direction
            projectile.AddForce(direction * waterProjectileSpeed);
        }

        if (player.earth == true)
        {
            //instantiate at the player's position and at player's rotation
            projectile = Instantiate(earthProjectile, transform.position, transform.rotation);
            Debug.Log("Earth shot fired!");

            //add the force in the correct direction
            projectile.AddForce(direction * earthProjectileSpeed);
        }

        if (player.air == true)
        {
            //instantiate at the player's position and at player's rotation
            projectile = Instantiate(airProjectile, transform.position, transform.rotation);
            Debug.Log("Air shot fired!");

            //add the force in the correct direction
            projectile.AddForce(direction * airProjectileSpeed);
        }
    }
}
