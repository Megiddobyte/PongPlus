using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Puck : MonoBehaviour
{
    private Rigidbody2D _rb;

    [SerializeField, Range(1, 20)] private int _puckSpeed;
    [SerializeField, Range(1, 5)] private float _extremeAngleSpeedScalar;
    [SerializeField] private Vector2 _initialVelocity, _minVelocity, _maxVelocity;
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
        float _directionScalar = col.rigidbody.velocity.y > 0 ? 1 : -1; //
        
        if (!col.gameObject.GetComponent<PlayerController>())
        {
            _rb.velocity = Vector2.Reflect(_rb.velocity, _normal);
            _bounce.Raise(this,  null);
        }
        else if (_contactPointY >= 0.4 || _contactPointY <= -0.4) //if contactPointY is in outermost 5ths
        {
            print("Extreme");
            print("Contact point: " + _contactPointY);
            _rb.velocity = Vector2.Reflect(_rb.velocity, _normal) * _movingScalar * _extremeAngleSpeedScalar;
        }
        else if (_contactPointY >= 0.05 || _contactPointY <= -0.05) //if contactPoint.y is in inner 3rds
        {
            print("Mild");
            print("Contact point: " + _contactPointY);
            _rb.velocity = Vector2.Reflect(_rb.velocity, _normal) * _movingScalar;
        }
        else //if contactPoint.y is in the center 5th
        {
            print("Center");
            print("Contact point: " + _contactPointY);
            _rb.velocity = new Vector2(-_rb.velocity.x, 0).normalized * _puckSpeed * _movingScalar;
        }
        print("Movingscalar is " + _movingScalar);
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
}
