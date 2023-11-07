using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class WinCondition : MonoBehaviour
{
    public void Calculate()
    {
        ScoreKeeper[] _scoreKeepers = FindObjectsOfType<ScoreKeeper>();
        ScoreKeeper _winner = _scoreKeepers[0];
        int _winningScore = 11;
        
        for (int i = 0; i < _scoreKeepers.Length; i++)
        {
            int _greatestValue = int.MinValue;
            if (_scoreKeepers[i].Score > _greatestValue && _scoreKeepers[i].Score >= _winningScore)
            {
                _greatestValue = _scoreKeepers[i].Score;
                _winner = _scoreKeepers[i];
            }
            else if (_scoreKeepers[i].Score > _greatestValue)
            {
                _greatestValue = _scoreKeepers[i].Score;
            }
            print($"Player {_winner.Name.ToString()} Wins!"); //called twice because game manager calls calculate twice
        }

    }
}
    

