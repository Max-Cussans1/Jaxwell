using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//used for scene transitions
public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    public void LoadLevel(string sceneName)
    {
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
