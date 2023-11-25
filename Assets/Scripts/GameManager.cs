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
    [SerializeField] private GameObject _pausePrefab;
    [SerializeField] private GameObject _pauseObject;
    private readonly float _timeBetweenRounds = 2;
    private bool _timerFlag;

    public static GameManager Instance;
    private GameObject _puck;
    [SerializeField] private GameEvent _win;
    private bool _canPause = true;
    private bool _isDone;

    public float ElapsedTime { get; set; }
    public bool StartTimer { get; set; }
    
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        
            #region UI INIT
            _pausePrefab = Resources.Load<GameObject>("Prefabs/PauseCanvas");
            _pauseObject = Instantiate(_pausePrefab, new Vector2(0, 0), Quaternion.identity, null);
            _pauseObject.SetActive(false);
            #endregion
            
            SceneManager.sceneLoaded += OnGameSceneLoaded;
            DontDestroyOnLoad(gameObject);
        }
        
    }
    
    void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != (int) Level.Game) return;
        RoundReset();
        print("respawning puck");
        
        #if UNITY_STANDALONE
       // Cursor.visible = false;
        #endif
    }
    
    public void RoundReset() => UnityTimer.Timer.Register(_timeBetweenRounds, RespawnPuck);
    
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
        if (!_canPause || SceneManager.GetActiveScene().buildIndex != (int)Level.Game) return;
        _canPause = false;

        UI_Initialization();
        print("Initializing UI...");
        _pauseObject.SetActive(!_pauseObject.activeSelf);
        Cursor.visible = _pauseObject.activeSelf;
        Time.timeScale = _pauseObject.activeSelf ? 0 : 1;
        
    }

    private void UI_Initialization() //why do I instantiate it here instead of on Awake?
    {
        _pauseObject.SetActive(false);
        DontDestroyOnLoad(_pauseObject);
    }
    
    public void LoadGameAgainstAI()
    {
        SceneManager.LoadSceneAsync((int)Level.Game, LoadSceneMode.Single);
    }

    public void LoadGameCoop()
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
        Application.Quit();
    }

    private void OnDisable()
    {
        Instance = null;
        SceneManager.sceneLoaded -= OnGameSceneLoaded;
    }
}
