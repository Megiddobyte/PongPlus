using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Puck : MonoBehaviour
{
    private Rigidbody2D _rb;
    private CircleCollider2D _cc;

    [SerializeField, Range(1, 20)] private int _puckSpeed;
    [SerializeField, Range(0, 360)] private float _maxAngle;
    [SerializeField] private GameEvent _bounce;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = new Vector2(1, -10).normalized * _puckSpeed;
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        Vector2 _normal = col.contacts[0].normal;
        print("The velocity vector is " + _rb.velocity);
        print("The normal is " + _normal);
        print("The angle of collision is " + Vector2.Angle(_rb.velocity, col.contacts[0].normal));
        _rb.velocity = Vector2.Reflect(_rb.velocity, col.contacts[0].normal) * _puckSpeed;
        
       //f(Vector2.Angle(_rb.velocity, _normal) > _maxAngle)
       //
       //   _rb.velocity = Vector2.Reflect(_rb.velocity, _normal);
       //   print("Angle was greater than maxAngle");
       //   print("Angle is " + Vector2.Angle(_rb.velocity, _normal));
       //
       //lse
       //
       //   print("Angle was less than maxAngle");
       //   print("Angle is " + Vector2.Angle(_rb.velocity, _normal));

       //
        
        
        //_rb.velocity = Vector2.Reflect(_rb.velocity, col.contacts[0].normal).normalized * _puckSpeed;
        _bounce.Raise(this,  null);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(gameObject);
    }
}
