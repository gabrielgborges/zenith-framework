using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface ILoadingService : IService
{
    public void LoadForFixedTime(float seconds, GameEventBase eventToInvoke = null);
    public UniTask WaitForLoading(float seconds);
    public UniTask StartLoading();
    public UniTask EndLoading();
}
