using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    //add the player to the script in editor
    public Transform player;

    //change as needed
    public float height = -10.0f;

    void Update()
    {
        //changes camera position to be above the player poition (added in editor) in the z axis
        transform.position = new Vector3(player.position.x, player.position.y, height);
    }
}
