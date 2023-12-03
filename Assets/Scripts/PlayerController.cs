using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;
using Vector2 = UnityEngine.Vector2;

[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField, Range(25, 40)] private int _paddleSpeed;
    [SerializeField] private float _minYPos, _maxYPos;

    private ActionMap _inputActions;
    private Rigidbody2D _rb;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        _inputActions = new ActionMap();
        _inputActions.Paddle.Move.performed += OnPaddleMove;
        _inputActions.Paddle.Move.canceled += OnPaddleStop;
        _inputActions.Paddle.Pause.started += OnPause;
        
        _inputActions.Enable();
    }


    private void OnPaddleMove(InputAction.CallbackContext obj)
    {
        _rb.velocity = Vector2.up * _paddleSpeed * obj.ReadValue<float>();
    }
    private void OnPaddleStop(InputAction.CallbackContext obj) => _rb.velocity = Vector2.zero;
    
    private void OnPause(InputAction.CallbackContext obj) => GameManager.Instance.OnPause();

    private void FixedUpdate()
    {
        if (transform.position.y >= _maxYPos)
        {
            transform.position = new Vector2(transform.position.x, _maxYPos);
        }

        if (transform.position.y <= _minYPos)
        {
            transform.position = new Vector2(transform.position.x, _minYPos);
        }
    }
    private void OnDestroy() => DisableController();

    private void DisableController()
    {
        _inputActions.Paddle.Move.performed -= OnPaddleMove;
        _inputActions.Paddle.Move.canceled -= OnPaddleStop;
        _inputActions.Paddle.Pause.started -= OnPause;
        
        _inputActions.Disable();
    }
}
