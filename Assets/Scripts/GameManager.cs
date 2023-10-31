using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public GameManager Instance;
    private GameObject _puck;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }


    private void SpawnPuck()
    {
        
    }

    public void LoadGameAgainstAI()
    {
        SceneManager.LoadSceneAsync((int)Level.Game, LoadSceneMode.Single);
    }

    public void LoadGameSplitscreen()
    {
        
    }

    public void LoadGameOnline()
    {
        
    }
}
