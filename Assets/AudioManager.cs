using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;

    public Sound[] musicSounds;
    public Sound[] sfxSounds;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(string name)
    {
        Sound sound = Array.Find(musicSounds, s => s.name == name);
        musicSource.clip = sound.audioClip;
        musicSource.Play();
    }

    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(sfxSounds, s => s.name == name);
        musicSource.PlayOneShot(sound.audioClip);
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Stop the audio when a new scene is loaded
        musicSource.Stop();
        sfxSource.Stop();

        if (scene.buildIndex == 0)
        {
            musicSource.clip = musicSounds[0].audioClip;
            musicSource.Play();
        }
        else if (scene.buildIndex == 1)
        {
            musicSource.clip = musicSounds[1].audioClip;
            musicSource.Play();
        }
    }
}
