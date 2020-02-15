using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewGameMenu : MonoBehaviour
{
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

        if(save1 != null)
        {
            saveText1.text = save1.sceneName;
            save1Empty = false;
        }

        if (save2 != null)
        {
            saveText1.text = save2.sceneName;
            save2Empty = false;
        }

        if (save3 != null)
        {
            saveText1.text = save3.sceneName;
            save3Empty = false;
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Level_001");
    }

    public void Save1Chosen()
    {
        if (!save1Empty)
        {
            //enable confirmation screen for save 1
        }
        else
        {
            SaveManager.currentSavePath = savePath1;
            NewGame();
            DebugHelper.Log("No save slot in save 1, starting new game with the savepath " + savePath1);
        }
    }

    public void Save2Chosen()
    {
        if (!save2Empty)
        {
            //enable confirmation screen for save 2
        }
        else
        {
            SaveManager.currentSavePath = savePath2;
            NewGame();
            DebugHelper.Log("No save slot in save 2, starting new game with the savepath " + savePath2);
        }
    }

    public void Save3Chosen()
    {        
        if(!save3Empty)
        {
            //enable confirmation screen for save 3
        }
        else
        {
            SaveManager.currentSavePath = savePath3;
            NewGame();
            DebugHelper.Log("No save slot in save 3, starting new game with the savepath" + savePath3);
        }
    }
}
