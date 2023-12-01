using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using System.Threading;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    private GameObject _pausePrefab, _pauseObject;
    private GameObject _winPrefab, _winObject;
    private readonly float _timeBetweenRounds = 2;
    private bool _timerFlag;
    
    public static GameManager Instance;
    
    private GameObject _puck;
    private bool _canPause = true;
    private bool _isDone;

    public bool HasWon { get; private set; }
    void Awake()
    {
        if(Instance != null && Instance != this) //works for destroying other managers throughout scene loading, but then NREs when calling Instance?
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            InitializeDependencies();
            
            SceneManager.sceneLoaded += OnGameSceneLoaded;
            DontDestroyOnLoad(gameObject);
        }
    }

    void InitializeDependencies()
    {
        #region UI INIT
        _pausePrefab = Resources.Load<GameObject>("Prefabs/PauseCanvas");
        _pauseObject = Instantiate(_pausePrefab, new Vector2(0, 0), Quaternion.identity, null);
        _pauseObject.SetActive(false);
        DontDestroyOnLoad(_pauseObject);

        _winPrefab = Resources.Load<GameObject>("Prefabs/WinCanvas");
        _winObject = Instantiate(_winPrefab, new Vector2(0, 0), Quaternion.identity, null);
        _winObject.SetActive(false);
        DontDestroyOnLoad(_winObject);
        #endregion
    }
    
    void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != (int) Level.Game) return;
        RoundReset();
    }

    public void RoundReset()
    {
        if (HasWon) return;
        UnityTimer.Timer.Register(_timeBetweenRounds, RespawnPuck);
    }
    
    private void RespawnPuck()
    {
        Vector2 _respawnPosition = new Vector2(0, UnityEngine.Random.Range(-23, 24));
        var _obj = Resources.Load<GameObject>("Prefabs/Puck");
        Instantiate(_obj, _respawnPosition, Quaternion.identity);
    }

    public void Resume()
    {
        _pauseObject.SetActive(false);
        Time.timeScale = 1;
    }
    public void OnPause()
    {
        if (!_canPause || SceneManager.GetActiveScene().buildIndex == (int)Level.MainMenu) return; //can pause anywhere but mainmenu
        _canPause = false;

        _pauseObject.SetActive(!_pauseObject.activeSelf);
        Time.timeScale = _pauseObject.activeSelf ? 0 : 1;
        int _pauseDuration = 2;
        UnityTimer.Timer.Register(_pauseDuration, () => _canPause = true); //after a cooldown, allow player to pause again
    }

    public void OnWin(ScoreKeeper.PlayerName winningPlayer)
    {
        HasWon = true;
        _winObject.SetActive(true);
        _winObject.GetComponentInChildren<TMPro.TMP_Text>().text = $"Player {winningPlayer} Wins!";
    }

    public void Restart()
    {
        SceneManager.LoadSceneAsync((int) Level.Game, LoadSceneMode.Single);
        _winObject.SetActive(false);
    }

    public void LoadGameAgainstAI()
    {
        SceneManager.LoadSceneAsync((int)Level.Game, LoadSceneMode.Single);
    }

    public void LoadGameLocal()
    {
        
    }

    public void LoadGameOnline()
    {
        
    }

    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        
        #if UNITY_STANDALONE
        Application.Quit();
        #endif
    }

    private void OnDestroy()
    {
        if (Instance == this) //prevents destroyed managers in other scenes from destroying the static instance
        {
            Instance = null;
            SceneManager.sceneLoaded -= OnGameSceneLoaded;
        }
    }
}
