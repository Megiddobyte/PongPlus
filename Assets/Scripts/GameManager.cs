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
    private GameObject _pauseObject;
    private readonly float _timeBetweenRounds = 2; //fix

    public static GameManager Instance;
    private GameObject _puck;
    [SerializeField] private GameEvent _win;
    
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;

        _pausePrefab = Resources.Load<GameObject>("Prefabs/PauseCanvas");
        
        SceneManager.sceneLoaded += OnGameSceneLoaded; //static event handler, must unsubscribe before OnDestroy for domain reloading
        _pauseObject = Instantiate(_pausePrefab);
        _pauseObject.SetActive(false);
        
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(_pauseObject);
    }
    public void RoundReset()
    {
        StartCoroutine(SimpleSleep(_timeBetweenRounds));
    }
    void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != (int) Level.Game) return;

        StartCoroutine(SimpleSleep(_timeBetweenRounds));

        Cursor.visible = false;
    }

    IEnumerator SimpleSleep(float timeToSleep)
    {
        yield return new WaitForSeconds(timeToSleep);
        RespawnPuck();
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
        if (SceneManager.GetActiveScene().buildIndex != (int)Level.Game) return; //technically redundant because of action maps
        _pauseObject.SetActive(_pauseObject.activeSelf);
        Time.timeScale = _pauseObject.activeSelf ? 0 : 1;
    }


    public void LoadGameAgainstAI()
    {
        SceneManager.LoadSceneAsync((int)Level.Game, LoadSceneMode.Single);
    }

    public void LoadGameCoop() //rename coop
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
