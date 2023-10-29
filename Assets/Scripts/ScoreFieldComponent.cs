using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreFieldComponent : MonoBehaviour
{
    [SerializeField] private GameEvent _score;
    public ScoreSide Side  { get; private set; }
    private BoxCollider2D _bc;
    void Awake()
    {
        _bc = GetComponent<BoxCollider2D>();
    }

    public enum ScoreSide
    {
        Left,
        Right
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.GetComponent<Puck>()) return;
        _score.Raise(this, Side);
    }
}
