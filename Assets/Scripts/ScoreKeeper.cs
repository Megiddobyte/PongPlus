using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    private TMP_Text _scoreText;
    private int _score;
    
    private void Awake()
    {
        _scoreText = GetComponent<TMP_Text>();
    }

    public void UpdateScore()
    {
        _score++;
        _scoreText.text = _score.ToString();
    }
}
