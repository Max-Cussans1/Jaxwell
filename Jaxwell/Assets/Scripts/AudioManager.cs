using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager privateInstance;

    public static AudioManager instance
    {
        get
        {
            if(privateInstance == null)
            {
                //if there is no instance of AudioManager, create one
                privateInstance = FindObjectOfType<AudioManager>();
                if(privateInstance == null)
                {
                    privateInstance = new GameObject("Created AudioManager", typeof(AudioManager)).GetComponent<AudioManager>();
                }
            }

            return privateInstance;
        }
        private set
        {
            privateInstance = value;
        }
    }

    //need 2 music sources to transition between scenes
    private AudioSource musicSource;
    private AudioSource musicSource2;
    private AudioSource sfxSource;


}
