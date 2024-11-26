using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CombatentViewComponent : MonoBehaviour, ISignatureAnimator
{
    public Action OnFinishAwaitableAnimations;

    [SerializeField] private Animator _animator;

    private CancellationTokenSource _cancellationTokenSource;
    private CancellationToken _cancellationToken;

    private Coroutine _currentAnimationCoroutine = null;
    private List<int> _animationsQueue = new List<int>();
    private string _currentAnimationTrigger;
    private int _currentAnimationIndex = 0;

    public void Initialize()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationToken = _cancellationTokenSource.Token;
    }
    
    public void PlayAnimationSignature(AnimationSignature animation)
    {
        _animator.Play(animation.ToString());
    }

    public void PlayAnimation(string animation)
    {
        _animator.ResetTrigger(_currentAnimationTrigger);
        _currentAnimationTrigger = animation;
        _animator.SetTrigger(animation);
    }

    public void SetAnimationValue(string animation, float value)
    {
        if(_currentAnimationTrigger != animation)
        {
            _currentAnimationTrigger = animation;
            _animator.SetTrigger(animation);
        }

        _animator.SetFloat("Walk_Value", value);
    }

    public void StopContiguousAnimation()
    {
        _animator.SetBool("Play_loop", false);
    }

    public void ResetAnimations()
    {
        _animator.ResetTrigger(_currentAnimationTrigger);
        _animationsQueue.Clear();
        CancelNextQueueAnimation();
        if (_currentAnimationCoroutine != null)
        {
            StopCoroutine(_currentAnimationCoroutine);
            _currentAnimationCoroutine = null;
        }
    }

    public async UniTask AwaitQueueToPlayAnimation(string animation)
    { 
        int myPlaceInQueue = 0;

        if (_animationsQueue.Count > 0)
        {
            myPlaceInQueue = _animationsQueue[_animationsQueue.Count - 1] + 1;
        }
        _animationsQueue.Add(myPlaceInQueue);

        if (_currentAnimationCoroutine != null)
        {
            try
            {
                await UniTask.WaitUntil(() => _currentAnimationIndex == myPlaceInQueue && _currentAnimationCoroutine == null, PlayerLoopTiming.Update, _cancellationToken);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Attack canceled");
                OnFinishAwaitableAnimations?.Invoke();
                return;
            }
        }

        _currentAnimationTrigger = animation;

        _currentAnimationCoroutine = StartCoroutine(PlayQueueAnimations());
    }

    private IEnumerator PlayQueueAnimations()
    {
        _animationsQueue.RemoveAt(0);
        PlayAnimation(_currentAnimationTrigger);
        Debug.Log("Choffer4: " + _currentAnimationTrigger);

        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName(_currentAnimationTrigger)) ;
        Debug.Log("Choffer5");

        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length/ _animator.speed);

        if(_animationsQueue.Count > 0)
        {
            _currentAnimationIndex = _animationsQueue[0]; Debug.Log("Choffer7");
        }
        else
        {
            OnFinishAwaitableAnimations?.Invoke();
        }
        Debug.Log("Choffer8");
        _currentAnimationCoroutine = null;
    }

    private void CancelNextQueueAnimation()
    {
        if (_cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationToken = _cancellationTokenSource.Token;

    }
}
