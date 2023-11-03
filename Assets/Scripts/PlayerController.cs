using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField, Range(15, 30)] private int _paddleSpeed;
    [SerializeField] private float _minYPos, _maxYPos;

    private float _cachedReadValue;

    private Rigidbody2D _rb;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        ActionMap inputActions = new ActionMap();
        inputActions.Paddle.Move.performed += OnPaddleMove;
        inputActions.Paddle.Move.canceled += OnPaddleStop;

        inputActions.Paddle.Pause.performed += OnPause;
        
        inputActions.Enable();
    }


    private void OnPaddleMove(InputAction.CallbackContext obj)
    {
        if (transform.position.y >= _maxYPos || transform.position.y <= _minYPos)
        {
            
        }
        _rb.velocity = Vector2.up * _paddleSpeed * obj.ReadValue<float>();
        _cachedReadValue = obj.ReadValue<float>();
    }
    private void OnPaddleStop(InputAction.CallbackContext obj) => _rb.velocity = Vector2.zero;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Puck>()) return;

        _rb.velocity = Vector2.zero;
        _rb.AddForce(new Vector2(0, 5000) * -_cachedReadValue, ForceMode2D.Impulse);
        print("cached: " + _cachedReadValue);
    }

    private void OnPause(InputAction.CallbackContext obj) => GameManager.Instance.OnPause();
}
