using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioManager Instance;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _bounceSFX, _scoreSFX;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
        DontDestroyOnLoad(this);
        
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
}
