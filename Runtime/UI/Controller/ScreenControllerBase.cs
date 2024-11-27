using System;
using UnityEngine;

public class ScreenControllerBase : MonoBehaviour
{
    public Action OnOpen;
    public Action OnClose;

    public virtual void Open()
    {
        Initialize();
        OnOpen?.Invoke();
    }

    public virtual void Close()
    {
        OnClose?.Invoke();
        Destroy(gameObject);
    }

    protected virtual void Initialize() { }

    protected virtual void SetupView() { }
}