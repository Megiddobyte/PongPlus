using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class AIBrainWithTracking : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Vector3 _puckPosition;
    private bool _findingPosition;
    [SerializeField, Range(10, 20)] private int _moveSpeed = 10;
    [SerializeField, Range(10, 20)] private int _fastMoveSpeed = 15;
    [SerializeField] private float _minYPos, _maxYPos;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        UnityTimer.Timer.Register(1f, TryGetPosition, isLooped: true);
    }

    private void Move()
    {
        var _yDistance = Mathf.Abs(_puckPosition.y - _rb.position.y);
        var _xDistance = Mathf.Abs(_puckPosition.x - _rb.position.x);
        var _direction = Mathf.Sign(_puckPosition.y - _rb.position.y);
        
        if (_xDistance > 30) { _rb.velocity = Vector2.zero; }
        if (_yDistance > 10 && _xDistance < 25) //for reference, paddles are exactly 30 units away from the center in the x direction
        {
            UnityTimer.Timer.Register(0.2f, () => //small delay before moving directions for smoothing
            {
                _rb.velocity = new Vector2(0, _fastMoveSpeed * _direction);
            });
        }
        else
        {
            UnityTimer.Timer.Register(0.2f, () =>
            {
                if (UnityEngine.Random.Range(0, 2) == 1) //can tweak this to increase odds of AI missing the bounce
                {
                    _rb.velocity = new Vector2(0, _moveSpeed * _direction);
                }
                else
                {
                    _rb.velocity = Vector2.zero;
                }
            });
        }
    }
    private void FixedUpdate()
    {
        if (_findingPosition)
        {
            Move();
            UnityTimer.Timer.Register(0.2f, () => _findingPosition = false);
            print($"y velocity is {_rb.velocity.y}");
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
        _findingPosition = true;
        var _cache = FindObjectOfType<Puck>();
        if (_cache != null) { _puckPosition = _cache.gameObject.transform.position; }
        else { _puckPosition = Vector2.zero; }
    }
}
