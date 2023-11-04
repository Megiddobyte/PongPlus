using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuWrapper : MonoBehaviour
{
    public void Resume() => GameManager.Instance.Resume();   

    public void Options() => GameManager.Instance.OnPause();

    public void Quit() => GameManager.Instance.Quit();
}
