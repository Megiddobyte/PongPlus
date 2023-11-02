using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _pausePrefab;
    
    public static GameManager Instance;
    private GameObject _puck;
    
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
        DontDestroyOnLoad(this);
        
        //resolve dependencies via instantiation

        //_pausePrefab = Instantiate();

    }

    public void OnPause()
    {
        _pausePrefab.SetActive(_pausePrefab.activeSelf);
        Time.timeScale = _pausePrefab.activeSelf ? 0 : 1;
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
