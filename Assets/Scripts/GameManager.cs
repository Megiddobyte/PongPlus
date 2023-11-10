using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using Application = UnityEngine.Device.Application;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _pausePrefab;
    [SerializeField] private GameObject _pauseObject;
    private readonly float _timeBetweenRounds = 2;

    public static GameManager Instance;
    private GameObject _puck;
    [SerializeField] private GameEvent _win;
    private bool _canPause = true;
    
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;

        _pausePrefab = Resources.Load<GameObject>("Prefabs/PauseCanvas");
        
        SceneManager.sceneLoaded += OnGameSceneLoaded; //static event handler, must unsubscribe before OnDestroy for domain reloading

        
        DontDestroyOnLoad(gameObject);
    }
    
    void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != (int) Level.Game) return;

        StartCoroutine(RoundReset(_timeBetweenRounds, _canPause));

        #if UNITY_STANDALONE
        Cursor.visible = false;
        #endif
    }
    
    public IEnumerator RoundReset(float time, bool flagToFlip)
    {
        yield return StartCoroutine(SimpleSleep(_timeBetweenRounds, flagToFlip));
        RespawnPuck();
    }
    
    /// <summary>
    /// Sleeps for <c>timeToSleep</c> seconds. Optional flag <c>flagToFlipAfterCooldown</c> used for a simple cooldown timer.
    /// </summary>
    /// <param name="timeToSleep"></param>
    /// <param name="flagToFlipAfterCooldown"></param>
    /// <returns></returns>
    IEnumerator SimpleSleep(float timeToSleep, bool flagToFlipAfterCooldown = false)
    {
        yield return new WaitForSeconds(timeToSleep);
        flagToFlipAfterCooldown = true;
    }
    
    
    private void RespawnPuck()
    {
        Vector2 _respawnPosition = new Vector2(0, Random.Range(-23, 24));
        var _obj = Resources.Load<GameObject>("Prefabs/Puck");
        Instantiate(_obj, _respawnPosition, Quaternion.identity);
    }

    public void Resume()
    {
        _pausePrefab.SetActive(false);
        Time.timeScale = 1;
    }
    
    public void OnPause()
    {
        if (!_canPause || SceneManager.GetActiveScene().buildIndex != (int)Level.Game) return;
        UI_Initialization();
        _pauseObject.SetActive(!_pauseObject.activeSelf);
        Cursor.visible = _pauseObject.activeSelf;
        Time.timeScale = _pauseObject.activeSelf ? 0 : 1;
        float _pauseCooldown = 2f;
        SimpleSleep(_pauseCooldown, _canPause); //can I not do this because _canPause is passed by value, not reference?
        
    }

    private void UI_Initialization()
    {
        if (_pauseObject != null) return;
        _pauseObject = Instantiate(_pausePrefab, new Vector2(0, 0), Quaternion.identity, null);
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
