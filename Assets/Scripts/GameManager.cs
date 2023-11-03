using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _pausePrefab;
    [SerializeField, Range(0, 5)] private float _timeBetweenRounds;
    
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

    public void OnScore()
    {
        StartCoroutine(SimpleSleep(_timeBetweenRounds));
        RespawnPuck();
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

    public void LoadGameAgainstAI()
    {
        SceneManager.LoadSceneAsync((int)Level.Game, LoadSceneMode.Single);
    }

    public void LoadGameSplitscreen() //rename coop
    {
        
    }

    public void LoadGameOnline()
    {
        
    }
}
