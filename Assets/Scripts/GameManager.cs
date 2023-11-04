using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using Application = UnityEngine.Device.Application;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _pausePrefab;
    [SerializeField, Range(0, 5)] private float _timeBetweenRounds;

    public static GameManager Instance;
    private GameObject _puck;
    
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _pausePrefab = Resources.Load<GameObject>("Prefabs/PauseCanvas");
        SceneManager.sceneLoaded += OnGameSceneLoaded; //static event handler, must unsubscribe before OnDestroy for domain reloading

    }
    public void OnScore()
    {
        StartCoroutine(SimpleSleep(_timeBetweenRounds));
        RespawnPuck();
    }

    void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != (int) Level.Game) return;
        
        Instance.StartCoroutine(SimpleSleep(_timeBetweenRounds));
        Instance.RespawnPuck();

        Cursor.visible = false;
    }
    
    IEnumerator SimpleSleep(float timeToSleep)
    {
        yield return new WaitForSeconds(timeToSleep);
    }
    
    private void RespawnPuck()
    {
        Vector2 _respawnPosition = new Vector2(0, Random.Range(-23, 24));
        var _obj = Resources.Load<GameObject>("Prefabs/Puck");
        Instantiate(_obj, _respawnPosition, Quaternion.identity);
        //11 points is a win
    }

    public void Resume()
    {
        _pausePrefab.SetActive(false);
        Time.timeScale = 1;
    }
    public void OnPause()
    {
        _pausePrefab.SetActive(_pausePrefab.activeSelf);
        Time.timeScale = _pausePrefab.activeSelf ? 0 : 1;
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
