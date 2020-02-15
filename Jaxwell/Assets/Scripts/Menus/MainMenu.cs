using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {    
        SceneManager.LoadScene("Level_001");              
    }

    public void QuitGame()
    {
        DebugHelper.Log("Pressed quit from main menu");
        Application.Quit();
    }
}
