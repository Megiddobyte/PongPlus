using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSwap : MonoBehaviour
{
    private Canvas _canvas;
    [SerializeField] private Canvas _other;
    void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    public void SwapCanvas()
    {
        _canvas.enabled = !_canvas.enabled;
        _other.enabled = !_other.enabled;
    }

    public void SwapCanvas(Canvas other)
    {
        _canvas.enabled = !_canvas.enabled;
        other.enabled = !other.enabled;
    }
}
