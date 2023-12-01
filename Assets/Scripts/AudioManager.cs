using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _bounceSFX, _scoreSFX, _coinSFX, _backgroundTrack;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        _audioSource = GetComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnSceneLoad(Scene arg0, LoadSceneMode arg1)
    {
        if (SceneManager.GetActiveScene().buildIndex != (int)Level.Game) return;
        StartBackgroundTrack(_backgroundTrack, true);
    }

    private void StartBackgroundTrack(AudioClip source, bool loop)
    {
        _audioSource.clip = source;
        _audioSource.loop = loop;
        _audioSource.Play();
    }

    private void StopBackgroundTrack() => _audioSource.Stop();
    
    public void OnWin()
    {
        StopBackgroundTrack();
        //add winning track to audio source
    }

    public void BounceSFX() => _audioSource.PlayOneShot(_bounceSFX, 0.05f);
    public void ScoreSFX() => _audioSource.PlayOneShot(_scoreSFX);
    public void CoinSFX() => _audioSource.PlayOneShot(_coinSFX, 1.3f);
    
    
    private void OnDestroy()
    {
        if (Instance == this) //prevents destroyed managers in other scenes from destroying the static instance
        {
            SceneManager.sceneLoaded -= OnSceneLoad;
            Instance = null;
        }
    }
}
