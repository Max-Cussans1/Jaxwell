using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortal : MonoBehaviour
{
    public string destinationScene;

    PlayerState player;

    // Start is called before the first frame update
    void Start()
    {
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
                    Debug.Log("Entered scene transition from " + SceneManager.GetActiveScene().name + " to " + destinationScene);
                    SceneManager.LoadScene(destinationScene);
                    player.Save();
                }
                else
                {
                    Debug.Log("Attempted scene transition through scene portal but destination scene was null");
                }
            }
        }
    }
}
