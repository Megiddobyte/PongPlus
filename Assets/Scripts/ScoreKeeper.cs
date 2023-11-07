using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    private TMP_Text _scoreText;
    public int Score { get; private set; }
    [field:SerializeField] public PlayerName Name { get; set; }

    [SerializeField] private GameEvent _win;

    public enum PlayerName
    {
        One,
        Two
    }
    private void Awake()
    {
        _scoreText = GetComponent<TMP_Text>();
    }

    public void UpdateScore()
    {
        Score++;
        _scoreText.text = Score.ToString();
        
        if (CheckIfGameWon(Score))
        {
            _win.Raise(this, Score);
        }
    }

    private bool CheckIfGameWon(int score)
    {
        if (score >= 11) 
        {
            print($"Game won by player {Name}");
            return true;
        }
        return false;
    }
}
