using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

public class ScenePortal : MonoBehaviour
{
    [SerializeField] AudioClip levelEndSFX;

    public LevelLoader levelLoader;
    public string destinationScene;

    PlayerState player;

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(levelLoader, "Level Loader was null, ensure the Level Loader GameObject is assigned in the scene portal editor options");
        Assert.IsNotNull(levelEndSFX, "Level end SFX was null, ensure a sound is assigned to the scene portal script");
        player = FindObjectOfType<PlayerState>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        //check if whatever we are hitting isn't null
        if (other.gameObject != null)
        {
            if (other.gameObject == player.gameObject)
            {
                if (destinationScene != null)
                {
                    AudioManager.instance.PlaySFX(levelEndSFX);
                    DebugHelper.Log("Entered scene transition from " + SceneManager.GetActiveScene().name + " to " + destinationScene);
                    levelLoader.LoadLevel(destinationScene);
                }
                else
                {
                    DebugHelper.Log("Attempted scene transition through scene portal but destination scene was null");
                }
            }
        }
    }

}
