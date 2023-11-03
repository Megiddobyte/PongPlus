using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    private TMP_Text _scoreText;
    public int Score { get; private set; }
    private void Awake()
    {
        _scoreText = GetComponent<TMP_Text>();
    }

    public void UpdateScore()
    {
        Score++;
        _scoreText.text = Score.ToString();
    }
}
