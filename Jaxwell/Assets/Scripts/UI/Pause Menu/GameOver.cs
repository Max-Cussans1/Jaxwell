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

    void Start()
    {
        Assert.IsNotNull(player, "Player was null, ensure the player GameObject for Player is assigned in the UI GameOver script");
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
            player.transform.position = new Vector2(temp.position[0], temp.position[1]);
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
        SceneManager.LoadScene("Main_Menu");
    }
}

