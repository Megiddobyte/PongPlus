using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField, Range(1, 20)] private int _paddleSpeed;

    private Rigidbody2D _rb;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        ActionMap inputActions = new ActionMap();
        inputActions.Paddle.Move.performed += OnPaddleMove;
        inputActions.Paddle.Move.canceled += OnPaddleStop;
        
        inputActions.Enable();
    }

    private void OnPaddleMove(InputAction.CallbackContext obj)
    {
        _rb.velocity = Vector2.up * _paddleSpeed * obj.ReadValue<float>();
    }
    
    private void OnPaddleStop(InputAction.CallbackContext obj) => _rb.velocity = Vector2.zero;
}
