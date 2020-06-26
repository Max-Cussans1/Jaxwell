using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

public class GameOver : MonoBehaviour
{
    public static bool gameOver = false;
    public GameObject gameOverUI;
    public PlayerState player;
    Rigidbody2D playerrb;

    void Start()
    {
        Assert.IsNotNull(player, "Player was null, ensure the player GameObject for Player is assigned in the UI GameOver script in the UI Canvas");
        playerrb = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameOver)
        {
            showGameOverScreen();
        }
    }

    void showGameOverScreen()
    {
        DebugHelper.Log("Game over, showing game over screen");
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
    }

    public void restartLevel()
    {
        DebugHelper.Log("Pressed restart level from game over screen");
        Health.lives = 4;
        HealthUI.updateHealthUI = true;

        if (SaveManager.currentSavePath != null)
        {
            PlayerData temp = SaveSystem.Load(SaveManager.currentSavePath);
            playerrb.velocity = new Vector3(0, 0, 0);
            player.transform.position = new Vector2(temp.position[0], temp.position[1]);
            Health.currentCheckpoint = player.transform.position;
        }

        gameOver = false;
        gameOverUI.SetActive(false);
        Time.timeScale = 1;
    }

    public void mainMenu()
    {
        Health.lives = 4;
        gameOver = false;
        gameOverUI.SetActive(false);
        Time.timeScale = 1;
        //destroy audiomanager when going back to main menu so we dont get more than 1 instance
        Destroy(AudioManager.instance.gameObject);
        SceneManager.LoadScene("Main_Menu");
    }
}

