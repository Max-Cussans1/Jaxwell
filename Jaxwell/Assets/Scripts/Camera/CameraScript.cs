using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    //add the player to the script in editor
    public Transform player;
    Camera mainCamera;
    public float orthographicCameraSizeOnToggle = 25.0f;
    private float originalOrthographicCameraSize;

    bool cameraSizeToggled = false;

    //change as needed
    public float height = -10.0f;

    void Start()
    {
        mainCamera = this.GetComponent<Camera>();
        originalOrthographicCameraSize = mainCamera.orthographicSize;
    }

    void Update()
    {
        //changes camera position to be above the player poition (added in editor) in the z axis
        transform.position = new Vector3(player.position.x, player.position.y, height);

        if(Input.GetKeyDown(KeyCode.C))
        {
            if (!cameraSizeToggled)
            {
                mainCamera.orthographicSize = orthographicCameraSizeOnToggle;
                DebugHelper.Log("Camera size toggled to " + orthographicCameraSizeOnToggle);
                cameraSizeToggled = true;
            }
            else
            {
                mainCamera.orthographicSize = originalOrthographicCameraSize;
                DebugHelper.Log("Camera size toggled to " + originalOrthographicCameraSize);
                cameraSizeToggled = false;
            }
        }
    }
}
