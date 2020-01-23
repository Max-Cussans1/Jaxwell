using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData
{
    //store the scene as a string to convert to binary in the save file
    Scene scene;
    public string sceneName;

    public int element;
    //array to store 2 floats for position that we can convert to binary
    public float[] position;

    public PlayerData(PlayerState player)
    {
        scene = SceneManager.GetActiveScene();

        sceneName = scene.name;

        element = (int)player.element;
        //populate the position array with our player's position
        position = new float[3];
        position[0] = player.currentCheckpoint.x;
        position[1] = player.currentCheckpoint.y;
        position[2] = player.currentCheckpoint.z;
    }
}
