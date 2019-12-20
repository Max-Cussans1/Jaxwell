using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int element;
    //array to store 2 floats for position that we can convert to binary
    public float[] position;

    public PlayerData(PlayerState player)
    {
        element = (int)player.element;
        //populate the position array with our player's position
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }
}
