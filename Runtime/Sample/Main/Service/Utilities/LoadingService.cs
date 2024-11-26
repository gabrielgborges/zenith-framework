using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LoadingService : MonoBehaviour, ILoadingService
{
    [SerializeField] private Animator _fadeAnimator;
    
    private IEventService _eventService;

    private readonly string FADE_IN_ANIMATION = "FadeIn";
    private readonly string FADE_OUT_ANIMATION = "FadeOut";

    private void Awake()
    {
        ServiceLocator.AddService<ILoadingService>(this);
    }

    private async void Start()
    {
        _eventService = await ServiceLocator.GetService<IEventService>();
    }


    public void LoadForFixedTime(float seconds, GameEvent eventToInvoke = null)
    {
        throw new System.NotImplementedException();
    }

    public async UniTask WaitForLoading(float seconds)
    {
        _fadeAnimator.Play(FADE_IN_ANIMATION);
        await UniTask.Delay(1000);
        
        await UniTask.Delay((int)(seconds * 1000));
        _fadeAnimator.Play(FADE_OUT_ANIMATION);
    }

    public async UniTask StartLoading()
    {
        _fadeAnimator.Play(FADE_IN_ANIMATION);
        await UniTask.Delay(1000);
    }

    public async UniTask EndLoading()
    {
        _fadeAnimator.Play(FADE_OUT_ANIMATION);
        await UniTask.Delay(1000);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
