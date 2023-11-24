using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Audio[] musicAudio, sfxAudio;
    public AudioSource musicSource, sfxSource;


    //Singleton Initialization in Awake
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("Theme1");
    }

    public void PlayMusic(string name)
    {
        Audio music = Array.Find(musicAudio, x => x.name == name);
        
        if(music == null)
        {
            Debug.Log("Music Not Found");
        }
        else
        {
            musicSource.clip = music.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Audio sfx = Array.Find(sfxAudio, x => x.name == name);

        if(sfx == null)
        {
            Debug.Log("SFX Not Found");
        }
        else
        {
            sfxSource.PlayOneShot(sfx.clip);
        }
    }

    
}
