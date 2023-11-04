using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _bounceSFX, _scoreSFX;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        _audioSource = GetComponent<AudioSource>();
    }

    public void BounceSFX()
    {
        _audioSource.PlayOneShot(_bounceSFX);
    }

    public void ScoreSFX()
    {
        _audioSource.PlayOneShot(_scoreSFX);
    }

    private void OnDisable()
    {
        Instance = null;
    }
}
