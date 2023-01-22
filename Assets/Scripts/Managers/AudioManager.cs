using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;


    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;


    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == ("MainMenu") ||
            SceneManager.GetActiveScene().name == ("Level 1") ||
            SceneManager.GetActiveScene().name == ("Level 2") ||
            SceneManager.GetActiveScene().name == ("Level 3") ||
            SceneManager.GetActiveScene().name == ("Level 4") ||
            SceneManager.GetActiveScene().name == ("Level 5"))
            PlayMusic("Level 1");

        if (SceneManager.GetActiveScene().name == ("Level 6") ||
            SceneManager.GetActiveScene().name == ("Level 7") ||
            SceneManager.GetActiveScene().name == ("Level 8") ||
            SceneManager.GetActiveScene().name == ("Level 9") ||
            SceneManager.GetActiveScene().name == ("Level 10"))
            PlayMusic("Level 2");

        if (SceneManager.GetActiveScene().name == ("Level 11") ||
            SceneManager.GetActiveScene().name == ("Level 12") ||
            SceneManager.GetActiveScene().name == ("Level 13") ||
            SceneManager.GetActiveScene().name == ("Level 14") ||
            SceneManager.GetActiveScene().name == ("Level 15"))
            PlayMusic("Level 3");
    }
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not Fund");
        }

        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not FOund");
        }

        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void ToogleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToogleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

}
