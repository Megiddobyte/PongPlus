using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Puck : MonoBehaviour
{
    private Rigidbody2D _rb;
    private BoxCollider2D _bc;

    [SerializeField, Range(1, 20)] private int _puckSpeed;
    [SerializeField] private GameEvent _bounce;
    [SerializeField] private GameEvent _score;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = new Vector2(-1, -1) * _puckSpeed;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        _rb.velocity = Vector2.Reflect(_rb.velocity, col.contacts[0].normal).normalized * _puckSpeed;
        _bounce.Raise(this,  null);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(gameObject);
    }
}
