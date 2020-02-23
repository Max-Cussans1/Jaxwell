using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 7"))
        {
            if(isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        DebugHelper.Log("Pressed resume from pause menu");
        Time.timeScale = 1;
        pauseMenuUI.SetActive(false);
        isPaused = false;
    }

    public void Pause()
    {
        DebugHelper.Log("Game paused");
        Time.timeScale = 0;
        pauseMenuUI.SetActive(true);
        isPaused = true;
    }

    public void Options()
    {
        DebugHelper.Log("Pressed options from pause menu");
    }

    public void QuitToMainMenu()
    {
        DebugHelper.Log("Pressed quit to main menu from pause menu");
        pauseMenuUI.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("Main_Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
        DebugHelper.Log("Pressed quit game from pause menu");
    }
}
