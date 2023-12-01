using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
public class Puck : MonoBehaviour
{
    private Rigidbody2D _rb;

    [SerializeField, Range(10, 30)] private int _puckSpeed;
    [SerializeField, Range(1, 5)] private float _extremeAngleSpeedScalar;
    [SerializeField] private Vector2 _maxVelocity;
    [SerializeField] private GameEvent _bounce;
    [SerializeField] private float _xAngleOffset;
    
    private BoxCollider2D _bc;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _bc = GetComponent<BoxCollider2D>();

        float _offset = 8;
        _rb.velocity = new Vector2(UnityEngine.Random.Range(-3, 3) + _offset, UnityEngine.Random.Range(-2, 2) + _offset).normalized * _puckSpeed;
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        float _contactPointY = col.transform.InverseTransformPoint(col.contacts[0].point).y;
        Vector2 _normal = col.contacts[0].normal;
        float _movingScalar = col.rigidbody.velocity.y > 0 ? 1.5f : 1; //velocity boost if paddle is moving
        float _directionFlag = col.rigidbody.velocity.y > 0 ? 1 : -1; //velocity direction of ball, used for reflection determination
        
        if (!col.gameObject.GetComponent<PlayerController>())
        {
            _rb.velocity = Vector2.Reflect(_rb.velocity, _normal);
            _bounce.Raise(this,  null);
        }
        else if (_contactPointY >= 0.4 || _contactPointY <= -0.4) //if contactPointY is in outermost 5ths
        {
            _rb.velocity = AngleSolver(_directionFlag, _normal, _movingScalar) * _extremeAngleSpeedScalar;
            print("hit outer part of paddle");
           
        }
        else if (_contactPointY >= 0.1 || _contactPointY <= -0.1) //if contactPoint.y is in inner 3rds
        {
            _rb.velocity = AngleSolver(_directionFlag, _normal, _movingScalar);
            print("hit middle of paddle");
        }
        else //if contactPoint.y is in the center 5th
        {
            if (col.rigidbody.velocity.y != 0) //only dead center reflection if paddle isn't moving
            {
                _rb.velocity = Vector2.Reflect(_rb.velocity, _normal);
                print("hit center while moving");
            }
            else
            {
                _rb.velocity = new Vector2(-_rb.velocity.x, 0).normalized * _puckSpeed * _movingScalar;
                print("hit center while not moving");
            }
        }
        _bounce.Raise(this,  null);
    }

    private void FixedUpdate()
    {
        if (_rb.velocity.x > _maxVelocity.x)
        {
            _rb.velocity = new Vector2(_maxVelocity.x, _rb.velocity.y);
        }
        if (_rb.velocity.y > _maxVelocity.y)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _maxVelocity.y);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.HasWon) //RoundReset() gets called before HasWon is set to true, causing an extra puck to spawn after winning
        //so this destroys it
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(gameObject);
    }

    private Vector2 AngleSolver(float directionFlag, Vector2 normal, float movingScalar) //if puck hits outer part of paddle, it should move in the direction the paddle is moving
    {
        float _directionFlag = directionFlag;
        Vector2 _normal = normal;
        float _movingScalar = movingScalar;
        
        if (_rb.velocity.y > 0 && _directionFlag > 0 || _rb.velocity.y < 0 && _directionFlag < 0) //if both paddle and ball move in same direction
        {
            Vector2 _result = Vector2.Reflect(_rb.velocity + new Vector2(_xAngleOffset, 0), _normal) * _movingScalar;
            return _result;
        }
        else
        {
            Vector2 _result = new Vector2(-_rb.velocity.x, -_rb.velocity.y);
            return _result;
        }
    }
    
}

