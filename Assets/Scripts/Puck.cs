using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Puck : MonoBehaviour
{
    private Rigidbody2D _rb;

    [SerializeField, Range(10, 30)] private int _puckSpeed;
    [SerializeField, Range(1, 5)] private float _extremeAngleSpeedScalar;
    [SerializeField] private Vector2 _initialVelocity, _maxVelocity;
    [SerializeField] private GameEvent _bounce;
    
    private BoxCollider2D _bc;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _bc = GetComponent<BoxCollider2D>();

        _rb.velocity = _initialVelocity.normalized * _puckSpeed;
    }


    private void OnCollisionEnter2D(Collision2D col) //get distance from center, not angle. 
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
            AngleSolver(_directionFlag, _normal, _movingScalar);
           
        }
        else if (_contactPointY >= 0.1 || _contactPointY <= -0.1) //if contactPoint.y is in inner 3rds
        {
            _rb.velocity = Vector2.Reflect(_rb.velocity, _normal) * _movingScalar;
        }
        else //if contactPoint.y is in the center 5th
        {
            _rb.velocity = new Vector2(-_rb.velocity.x, 0).normalized * _puckSpeed * _movingScalar;
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(gameObject);
    }

    private void AngleSolver(float directionFlag, Vector2 normal, float movingScalar)
    {
        float _directionFlag = directionFlag;
        Vector2 _normal = normal;
        float _movingScalar = movingScalar;
        
        if (_rb.velocity.y > 0 && _directionFlag > 0 || _rb.velocity.y < 0 && _directionFlag < 0) //if both paddle and ball move in same direction
        {
            _rb.velocity = Vector2.Reflect(_rb.velocity, _normal) * _movingScalar * _extremeAngleSpeedScalar;
        }
        else
        {
            _rb.velocity = new Vector2(-_rb.velocity.x, -_rb.velocity.y);
        }
    }
}
