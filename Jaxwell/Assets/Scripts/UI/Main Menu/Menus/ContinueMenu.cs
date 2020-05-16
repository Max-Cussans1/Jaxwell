using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ContinueMenu : MonoBehaviour
{
    public LevelLoader levelLoader;

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

    void Start()
    {
        savePath1 = Application.persistentDataPath + "/save1.jxw";

        savePath2 = Application.persistentDataPath + "/save2.jxw";

        savePath3 = Application.persistentDataPath + "/save3.jxw";


        save1 = SaveSystem.Load(savePath1);
        save2 = SaveSystem.Load(savePath2);
        save3 = SaveSystem.Load(savePath3);

        if (save1 != null)
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
        levelLoader.LoadLevel(("Level_001"));
    }

    public void LoadGame()
    {
         PlayerData save = SaveSystem.Load(SaveManager.currentSavePath);

         //if we have a save file
         if (save != null)
         {
            SaveManager.loadingGame = true;

            SaveManager.position.x = save.position[0];
            SaveManager.position.y = save.position[1];
            SaveManager.position.z = save.position[2];

            //if the scene loaded isn't the current scene in the save file, load the scene in the save file
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName(save.sceneName))
             {
                 DebugHelper.Log("Loaded scene from the save file " + SaveManager.currentSavePath);
                 levelLoader.LoadLevel((save.sceneName));
             }
         
         }         
    }

    public void Save1Chosen()
    {
        SaveManager.currentSavePath = savePath1;
        if (!save1Empty)
        {
            LoadGame();
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
            LoadGame();
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
            LoadGame();
        }
        else
        {
            NewGame();
            DebugHelper.Log("No save slot in save 3, starting new game with the savepath " + savePath3);
        }
    }
}
