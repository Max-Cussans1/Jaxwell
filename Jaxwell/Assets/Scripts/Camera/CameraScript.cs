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

    PlayerState playerState;

    bool cameraSizeToggled = false;

    //change as needed
    public float height = -10.0f;

    float shakeDistanceX = 0.2f;
    float shakeDistanceY = 0.2f;
    [SerializeField] float shakeDuration = 0.1f;
    float tempShakeDuration;
    bool cameraShake = false;

    void Start()
    {
        mainCamera = this.GetComponent<Camera>();
        originalOrthographicCameraSize = mainCamera.orthographicSize;
        tempShakeDuration = shakeDuration;

        playerState = player.GetComponent<PlayerState>();
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

        //if we dashed a distance of more than 1.0f and are grounded
        if(EarthDash.earthDashEnded && playerState.element == Elements.elements.earth && CollisionManager.isGrounded && EarthDash.heightDashedAt - player.position.y > 1.0f)
        {
            cameraShake = true;
        }

        if(cameraShake)
        {
            if (tempShakeDuration > 0)
            {
                DebugHelper.Log("Camera started shaking");
                CameraShake(shakeDistanceX, shakeDistanceY);

                tempShakeDuration -= Time.deltaTime;
            }
        }
        if(tempShakeDuration < 0)
        {
            cameraShake = false;
            DebugHelper.Log("Camera stopped shaking");
        }
        //reset the duration after we change out of earth
        if(playerState.element != Elements.elements.earth && !cameraShake)
        {
            tempShakeDuration = shakeDuration;
        }
    }

    void CameraShake(float distanceX, float distanceY)
    {
        float rdistanceX = Random.Range(-distanceX, distanceX);
        float rdistanceY = Random.Range(-distanceY, distanceY);
        
        float floatX = transform.position.x + rdistanceX;
        float floatY = transform.position.y + rdistanceY;

        transform.position = new Vector3(floatX, floatY, height);
        DebugHelper.Log("Camera shook with " + rdistanceX + " as the X value and " + rdistanceY + " as the Y value");
    }
}
