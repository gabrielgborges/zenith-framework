using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class AnimationEventsObserver : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private CombatentPhysicsComponent _physics;
    [SerializeField] private int _zDepthForVFX = 1;
    
    private IVisualEffectsService _visualEffectService;
    private int _currentCombo = 0;

    private async void Start()
    {
        _visualEffectService = await ServiceLocator.GetService<IVisualEffectsService>();
    }

    public void IncreaseCombo()
    {
        _currentCombo++;
        _animator.SetInteger("ComboCount", _currentCombo);
    }

    public void ResetCombo()
    {
        _currentCombo = 0;
        _animator.SetInteger("ComboCount", _currentCombo);
    }

    public void EnterDamageFrame()
    {
        StartCoroutine(EnableOneFrameOfDamage());

        IEnumerator EnableOneFrameOfDamage()
        {
            _physics.SetDamageEnable(true);

            yield return 0;
            _physics.SetDamageEnable(false);
        }
    }
    
    public void TakeDamageTimeScale()
    {
        StartCoroutine(FreezeGame(0.08f));
    }

    public void SpawnDamageVFX(CollisionData collisionData)
    {
        _visualEffectService.SpawnVFX(collisionData.VisualEffect, _physics.GetCollisionTransform(collisionData.BodyPosition).position + new Vector3(0,0,_zDepthForVFX));
    }

    private IEnumerator FreezeGame(float pauseTime)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        Time.timeScale = 0f;
        float pauseEndTime = Time.realtimeSinceStartup + pauseTime;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        Time.timeScale = 1f;
        Debug.Log("Done with my pause");
    }
}
