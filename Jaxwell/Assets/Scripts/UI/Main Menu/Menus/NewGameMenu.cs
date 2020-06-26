using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewGameMenu : MonoBehaviour
{
    public LevelLoader levelLoader;
    public GameObject confirmationScreen;
    public Text confirmationScreenText;

    public static string savePath1;
    public static string savePath2;
    public static string savePath3;

    public static PlayerData save1;
    public static PlayerData save2;
    public static PlayerData save3;

    public Text saveText1;
    public Text saveText2;
    public Text saveText3;

    bool save1Empty = true;
    bool save2Empty = true;
    bool save3Empty = true;

    public AudioClip music;

    void Start()
    {
        savePath1 = Application.persistentDataPath + "/save1.jxw";

        savePath2 = Application.persistentDataPath + "/save2.jxw";

        savePath3 = Application.persistentDataPath + "/save3.jxw";


        save1 = SaveSystem.Load(savePath1);
        save2 = SaveSystem.Load(savePath2);
        save3 = SaveSystem.Load(savePath3);

        if(save1 != null)
        {
            saveText1.text = save1.sceneName;
            save1Empty = false;
        }

        if (save2 != null)
        {
            saveText2.text = save2.sceneName;
            save2Empty = false;
        }

        if (save3 != null)
        {
            saveText3.text = save3.sceneName;
            save3Empty = false;
        }
    }

    public void NewGame()
    {
        levelLoader.LoadLevel("Level_001");
        AudioManager.instance.PlayMusic(music);
    }

    public void Save1Chosen()
    {
        SaveManager.currentSavePath = savePath1;
        if (!save1Empty)
        {
            confirmationScreen.SetActive(true);
            confirmationScreenText.text = "Are you sure you want to overwrite the save slot in save slot 1?";
        }
        else
        {            
            NewGame();
            DebugHelper.Log("No save slot in save 1, starting new game with the savepath " + savePath1);
        }
    }

    public void Save2Chosen()
    {
        SaveManager.currentSavePath = savePath2;
        if (!save2Empty)
        {
            confirmationScreen.SetActive(true);
            confirmationScreenText.text = "Are you sure you want to overwrite the save slot in save slot 2?";
        }
        else
        {            
            NewGame();
            DebugHelper.Log("No save slot in save 2, starting new game with the savepath " + savePath2);
        }
    }

    public void Save3Chosen()
    {
        SaveManager.currentSavePath = savePath3;
        if (!save3Empty)
        {
            confirmationScreen.SetActive(true);
            confirmationScreenText.text = "Are you sure you want to overwrite the save slot in save slot 3?";
        }
        else
        {            
            NewGame();
            DebugHelper.Log("No save slot in save 3, starting new game with the savepath " + savePath3);
        }
    }
}
