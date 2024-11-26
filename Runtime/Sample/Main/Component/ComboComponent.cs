using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ComboComponent : MonoBehaviour
{
    public Action OnStartComboChance;
    public Action OnEndComboChance;
    
    [SerializeField] private ComboData _combo;

    private CancellationTokenSource _cancellationTokenSource;
    private CancellationToken _cancellationToken;
    private int _initialComboIndex;
    private int _currentComboIndex;
    
    private void Start()
    {
        _initialComboIndex = _combo.ComboList.Index;   
        _currentComboIndex = _initialComboIndex;
    }

    public HitData GetHitFromCurrentCombo(string command)
    {
        ComboCommandNode comboNode = _combo.GetComboByIndex(_currentComboIndex);

        foreach (ComboCommandNode node in comboNode.Children)
        {
            if (node.Command == command)
            {
                SetNewComboHit(node);
                CountForComboChances(node);
                return node.HitData;
            }
        }

        return GetFreshFirstHit(command);
    }

    public void ResetCombo()
    {
        _currentComboIndex = _initialComboIndex;
    }

    private HitData GetFreshFirstHit(string command)
    {
        foreach (ComboCommandNode node in _combo.ComboList.Children)
        {
            if (node.Command == command)
            {
                SetNewComboHit(node);
                CountForComboChances(node);
                return node.HitData;
            }
        }

        Debug.Log("#5 hit command could not be found: " + command);
        return null;
    }

    private void SetNewComboHit(ComboCommandNode node)
    {
        CancelLastHitCombo();

        _currentComboIndex = node.Index;
    }

    private async void CountForComboChances(ComboCommandNode node)
    {
        try
        {
            await UniTask.DelayFrame(node.ActivationStartFrame * _combo.Speed, PlayerLoopTiming.Update, _cancellationToken);
            OnStartComboChance?.Invoke();

            await UniTask.DelayFrame((node.ActivationEndFrame * _combo.Speed) - (node.ActivationStartFrame * _combo.Speed), PlayerLoopTiming.Update, _cancellationToken);
            OnEndComboChance?.Invoke();

            ResetCombo();
        }
        catch (OperationCanceledException e)
        {
            Debug.Log($"{nameof(OperationCanceledException)} thrown with message: {e.Message}");
        }
    } 
    
    private void CancelLastHitCombo()
    {
        if(_cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationToken = _cancellationTokenSource.Token;
    }
}
