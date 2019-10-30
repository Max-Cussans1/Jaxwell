using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    //declate a playerscript variable so we can access members from the script
    PlayerScript player;

    //array of gameobjects for each element
    public static GameObject[] fireObjects;
    public static GameObject[] waterObjects;
    public static GameObject[] earthObjects;
    public static GameObject[] airObjects;

    //List of colliders for each element, we can then use this to turn collision on/off for all of a certain type
    public static List<Collider2D> fireObjectsCollider = new List<Collider2D>();
    public static List<Collider2D> waterObjectsCollider = new List<Collider2D>();
    public static List<Collider2D> earthObjectsCollider = new List<Collider2D>();
    public static List<Collider2D> airObjectsCollider = new List<Collider2D>();

    // Start is called before the first frame update
    void Start()
    {
        //grab the gameobjects by tag (add tags in editor)
        fireObjects = GameObject.FindGameObjectsWithTag("Fire");
        waterObjects = GameObject.FindGameObjectsWithTag("Water");
        earthObjects = GameObject.FindGameObjectsWithTag("Earth");
        airObjects = GameObject.FindGameObjectsWithTag("Air");

        //Make our playerscript variable our player's script so we can get the bools from that script
        player = GameObject.Find("Player").GetComponent<PlayerScript>();

        #region Debug prints for locations of all element objects
        //print where each element is to console
        for (int i = 0; i < fireObjects.Length; i++)
        {
            Debug.Log(fireObjects[i] + " (fire object) at " + fireObjects[i].transform.position);
        }

        for (int i = 0; i < waterObjects.Length; i++)
        {
            Debug.Log(waterObjects[i] + " (water object) at " + waterObjects[i].transform.position);
        }

        for (int i = 0; i < earthObjects.Length; i++)
        {
            Debug.Log(earthObjects[i] + " (earth object) at " + earthObjects[i].transform.position);
        }

        for (int i = 0; i < airObjects.Length; i++)
        {
            Debug.Log(airObjects[i] + " (air object) at " + airObjects[i].transform.position);
        }
        #endregion

        #region Add colliders to each collider list
        //add the colliders for each element to the collider list
        for (int i = 0; i < fireObjects.Length; i++)
        {
            fireObjectsCollider.Add(fireObjects[i].GetComponent<Collider2D>());
        }

        for (int i = 0; i < waterObjects.Length; i++)
        {
            waterObjectsCollider.Add(waterObjects[i].GetComponent<Collider2D>());
        }

        for (int i = 0; i < earthObjects.Length; i++)
        {
            earthObjectsCollider.Add(earthObjects[i].GetComponent<Collider2D>());
        }

        for (int i = 0; i < airObjects.Length; i++)
        {
            airObjectsCollider.Add(airObjects[i].GetComponent<Collider2D>());
        }
        #endregion
    }

    
    void Update()
    {
        #region If player is fire...
        //check if the player is fire, turn the collision on for fire platforms and off for others
        if (player.fire == true)
        {
            for (int i = 0; i < fireObjects.Length; i++)
            {
                if (fireObjectsCollider[i].enabled == false)
                {
                    fireObjectsCollider[i].enabled = true;
                }
            }

            for (int i = 0; i < waterObjects.Length; i++)
            {
                if (waterObjectsCollider[i].enabled == true)
                {
                    waterObjectsCollider[i].enabled = false;
                }
            }

            for (int i = 0; i < earthObjects.Length; i++)
            {
                if (earthObjectsCollider[i].enabled == true)
                {
                    earthObjectsCollider[i].enabled = false;
                }
            }

            for (int i = 0; i < airObjects.Length; i++)
            {
                if (airObjectsCollider[i].enabled == true)
                {
                    airObjectsCollider[i].enabled = false;
                }
            }
        }
        #endregion

        #region If player is water...
        //check if the player is water, turn the collision on for water platforms and off for others
        if (player.water == true)
        {
            for (int i = 0; i < fireObjects.Length; i++)
            {
                if (fireObjectsCollider[i].enabled == true)
                {
                    fireObjectsCollider[i].enabled = false;
                }
            }

            for (int i = 0; i < waterObjects.Length; i++)
            {
                if (waterObjectsCollider[i].enabled == false)
                {
                    waterObjectsCollider[i].enabled = true;
                }
            }

            for (int i = 0; i < earthObjects.Length; i++)
            {
                if (earthObjectsCollider[i].enabled == true)
                {
                    earthObjectsCollider[i].enabled = false;
                }
            }

            for (int i = 0; i < airObjects.Length; i++)
            {
                if (airObjectsCollider[i].enabled == true)
                {
                    airObjectsCollider[i].enabled = false;
                }
            }
        }
        #endregion

        #region If player is earth...
        //check if the player is earth, turn the collision on for earth platforms and off for others
        if (player.earth == true)
        {
            for (int i = 0; i < fireObjects.Length; i++)
            {
                if (fireObjectsCollider[i].enabled == true)
                {
                    fireObjectsCollider[i].enabled = false;
                }
            }

            for (int i = 0; i < waterObjects.Length; i++)
            {
                if (waterObjectsCollider[i].enabled == true)
                {
                    waterObjectsCollider[i].enabled = false;
                }
            }

            for (int i = 0; i < earthObjects.Length; i++)
            {
                if (earthObjectsCollider[i].enabled == false)
                {
                    earthObjectsCollider[i].enabled = true;
                }
            }

            for (int i = 0; i < airObjects.Length; i++)
            {
                if (airObjectsCollider[i].enabled == true)
                {
                    airObjectsCollider[i].enabled = false;
                }
            }
        }
        #endregion

        #region If player is air...
        //check if the player is air, turn the collision on for air platforms and off for others
        if (player.air == true)
        {
            for (int i = 0; i < fireObjects.Length; i++)
            {
                if (fireObjectsCollider[i].enabled == true)
                {
                    fireObjectsCollider[i].enabled = false;
                }
            }

            for (int i = 0; i < waterObjects.Length; i++)
            {
                if (waterObjectsCollider[i].enabled == true)
                {
                    waterObjectsCollider[i].enabled = false;
                }
            }

            for (int i = 0; i < earthObjects.Length; i++)
            {
                if (earthObjectsCollider[i].enabled == true)
                {
                    earthObjectsCollider[i].enabled = false;
                }
            }

            for (int i = 0; i < airObjects.Length; i++)
            {
                if (airObjectsCollider[i].enabled == false)
                {
                    airObjectsCollider[i].enabled = true;
                }
            }
        }
        #endregion
    }
}
