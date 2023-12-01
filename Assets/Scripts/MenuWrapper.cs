using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MenuWrapper : MonoBehaviour
{
    public void Resume() => GameManager.Instance.Resume();   

    public void Options() => GameManager.Instance.OnPause();

    public void Restart() => GameManager.Instance.Restart();

    public void Quit() => GameManager.Instance.Quit();
}
