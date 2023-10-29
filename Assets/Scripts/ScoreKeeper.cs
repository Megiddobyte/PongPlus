using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    private TMP_Text _scoreText;
    private int _score;
    [SerializeField] private ScoreFieldComponent.ScoreSide _side;
    
    private void Awake()
    {
        _scoreText = GetComponent<TMP_Text>();
    }

    public void UpdateScore()
    {
        _scoreText.text = _score.ToString();
    }
}
