using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Range(0f, 1f)]
    public float musicVolume = 1f;

    [Range(0f, 1f)]
    public float sfxVolume = 1f;

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

    bool firstMusicSourceIsPlaying;

    void Awake()
    {
        //make sure we don#t destroy this instance
        DontDestroyOnLoad(this.gameObject);

        //create audiosource components
        musicSource = this.gameObject.AddComponent<AudioSource>();
        musicSource2 = this.gameObject.AddComponent<AudioSource>();
        sfxSource = this.gameObject.AddComponent<AudioSource>();

        //make sure the music tracks are looping
        musicSource.loop = true;
        musicSource2.loop = true;
    }

    public void PlayMusic(AudioClip musicClip)
    {
        AudioSource activeSource;

        //check which music source is playing
        if (firstMusicSourceIsPlaying)
        {
            activeSource = musicSource;
        }
        else
        {
            activeSource = musicSource2;
        }

        activeSource.clip = musicClip;
        activeSource.volume = musicVolume;
        activeSource.Play();
    }

    public void PlayMusicWithFade(AudioClip nextMusic, float transitionTime = 1.0f)
    {
        //store the transition time
        float startingTransitionTime = transitionTime;
        AudioSource activeSource;

        //check which music source is playing
        if (firstMusicSourceIsPlaying)
        {
            activeSource = musicSource;
        }
        else
        {
            activeSource = musicSource2;
        }

        StartCoroutine(UpdateMusicWithFade(activeSource, nextMusic, transitionTime, startingTransitionTime));

    }

    private IEnumerator UpdateMusicWithFade(AudioSource activeSource, AudioClip nextMusic, float transitionTime, float startingTransitionTime)
    {

        if(!activeSource.isPlaying)
        {
            activeSource.Play();
        }

        bool beginFadeIn = false;    

        //if transition time is still remaining and we aren't fading back in yet, reduce volume of music
        if (transitionTime > 0f && !beginFadeIn)
        {
            transitionTime -= Time.deltaTime;
            activeSource.volume = (transitionTime / startingTransitionTime) * musicVolume;
            yield return null;
        }

        //if transition is finished, stop the track and play next music
        if(transitionTime <= 0f && !beginFadeIn)
        {
            activeSource.Stop();
            activeSource.clip = nextMusic;
            activeSource.Play();
            beginFadeIn = true;
        }

        if(transitionTime < startingTransitionTime && beginFadeIn)
        {
            transitionTime += Time.deltaTime;
            activeSource.volume = (transitionTime / startingTransitionTime) * musicVolume;
            yield return null;
        }
    }

    public void PlayMusicWithCrossFade(AudioClip nextMusic, float transitionTime = 1.0f)
    {
        float startingTransitionTime = transitionTime;
        AudioSource activeSource;
        AudioSource newSource;

        //check which music source is playing and assign both audiosources
        if (firstMusicSourceIsPlaying)
        {
            activeSource = musicSource;
            newSource = musicSource2;
        }
        else
        {
            activeSource = musicSource2;
            newSource = musicSource;
        }

        //swap to whichever source is not playing
        firstMusicSourceIsPlaying = !firstMusicSourceIsPlaying;

        newSource.clip = nextMusic;
        newSource.Play();

        StartCoroutine(UpdateMusicWithCrossFade(activeSource, newSource, transitionTime, startingTransitionTime));

    }

    private IEnumerator UpdateMusicWithCrossFade(AudioSource activeSource, AudioSource newSource, float transitionTime, float startingTransitionTime)
    {
        if (transitionTime > 0f)
        {
            transitionTime -= Time.deltaTime;
            //reduce volume of the active source and increase volume of the new source at the same time by the same amount
            activeSource.volume = (transitionTime / startingTransitionTime) * musicVolume;
            newSource.volume = ((startingTransitionTime - transitionTime) / startingTransitionTime) * musicVolume;
            yield return null;
        }
    }


    public void PlaySFX(AudioClip sfxClip)
    {
        sfxSource.volume = sfxVolume;
        sfxSource.PlayOneShot(sfxClip);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = musicVolume;
        musicSource2.volume = musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        sfxSource.volume = volume;
    }
}
