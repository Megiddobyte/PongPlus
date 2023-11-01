using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreFieldComponent : MonoBehaviour
{
    [SerializeField] private GameEvent _score;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.GetComponent<Puck>()) return;
        _score.Raise(this, null);
    }
}
