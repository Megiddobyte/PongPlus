using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIBrain : MonoBehaviour
{
    [SerializeField, Range(10, 20)] private int _moveSpeed = 15;
    private Rigidbody2D _rb;
    private BoxCollider2D _bc;
    private int _direction;
    private bool _isMoving;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _bc = GetComponent<BoxCollider2D>();
    }
    private void Move()
    {
        _isMoving = true; //prevents more than one method frame being pushed on the stack at once
        _rb.velocity = new Vector2(0, -_moveSpeed);
        UnityTimer.Timer.Register(Random.Range(3, 4), () =>
        {
            print("setting velocity soon...");
            _rb.velocity = new Vector2(0, _moveSpeed);
            _isMoving = false;
        });

    }
    
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Puck>())
        {
            
        }
        else //collided with wall
        {
            _rb.velocity = -_rb.velocity;
        }
    }

    private void FixedUpdate()
    {
        if(!_isMoving) { Move(); }
    }
}
