using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//used for scene transitions
public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public AudioClip music1;
    public AudioClip music2;
    public float transitionTime = 1f;

    public void LoadLevel(string sceneName)
    {
        AudioManager.instance.PlayMusic(music1);
        StartCoroutine(LoadLevelWithTransition(sceneName));
    }

    IEnumerator LoadLevelWithTransition(string sceneName)
    {
        DebugHelper.Log("Loading level with transition " + sceneName);
        transition.SetTrigger("Start_Fade");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
    }
}
