using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class AIBrainWithTracking : MonoBehaviour
{
    private Rigidbody2D _rb;
    private BoxCollider2D _bc;
    private Vector3 _puckPosition;
    private bool _findingPosition;
    [SerializeField, Range(10, 20)] private int _moveSpeed = 10;
    [SerializeField, Range(10, 20)] private int _fastMoveSpeed = 20;
    [SerializeField] private float _minYPos, _maxYPos;
    private UnityTimer.Timer _timer;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _bc = GetComponent<BoxCollider2D>();
        _timer = UnityTimer.Timer.Register(0.5f, TryGetPosition, isLooped: true);
    }

    private void Move()
    {
        var _yDistance = Mathf.Abs(_puckPosition.y - _rb.position.y);
        var _xDistance = Mathf.Abs(_puckPosition.x - _rb.position.x);
        var _direction = Mathf.Sign(_puckPosition.y - _rb.position.y);
        if (_yDistance > 4)
        {
            UnityTimer.Timer.Register(0.5f, () => //small delay before moving directions for smoothing
            {
                _rb.velocity = new Vector2(0, _fastMoveSpeed * _direction);
            });
        }
        else
        {
            UnityTimer.Timer.Register(0.5f, () =>
            {
                if (UnityEngine.Random.Range(0, 2) == 1)
                {
                    _rb.velocity = Vector2.zero;
                }
                else
                {
                    _rb.velocity = new Vector2(0, _moveSpeed * _direction);
                }
            });
        }
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
        if (_findingPosition)
        {
            //print("Moving...");
            Move();
            _findingPosition = false;
        }
        if (transform.position.y >= _maxYPos)
        {
            transform.position = new Vector2(transform.position.x, _maxYPos);
        }

        if (transform.position.y <= _minYPos)
        {
            transform.position = new Vector2(transform.position.x, _minYPos);
        }
    }

    private void TryGetPosition()
    {
        //print("Trying to find position...");
        _findingPosition = true;
        var _cache = FindObjectOfType<Puck>();
        if (_cache != null) {_puckPosition = _cache.gameObject.transform.position;}
        else
        {
            _puckPosition = Vector2.zero;
        }
    }

    private void OnDisable()
    {
        UnityTimer.Timer.Cancel(_timer);
    }
}
