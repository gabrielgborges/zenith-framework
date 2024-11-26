using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatentPhysicsComponent : MonoBehaviour, IDamageable, ICombatent
{
    public Action<DamageData> OnTakeDamage;

    [SerializeField] private WeaponPhysicsComponent _weapon;
    [SerializeField] private bool _takeDamageCollider;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private List<TransformByBodyCollision> _collisionBodies;
    
    private Coroutine _knocbackCoroutine;
    private float _knockbackTime = 1.5f;

    public Transform ColliderTransform => _characterController.transform;

    public void TakeDamage(DamageData damage)
    {
        if (_takeDamageCollider)
        {
            if (damage.DamageType == DamageType.KNOCKBACK)
            {
                ApplyKnockBack(damage.ImpulseForce);
            }

            OnTakeDamage?.Invoke(damage);
        }
    }

    public void SetTakeDamageEnable(bool enabled)
    {
        _takeDamageCollider = enabled;
    }

    public void SetDamageEnable(bool enabled, DamageData damage)
    {
        _weapon.SetCollisionEnable(enabled);
        _weapon.Damage = damage;
    }

    public void SetDamageEnable(bool enabled)
    {
        _weapon.SetCollisionEnable(enabled);
    }

    public Transform GetCollisionTransform(BodyCollisionType bodyCollision)
    {
        foreach (TransformByBodyCollision collisionBody in _collisionBodies)
        {
            if (collisionBody.BodyCollision == bodyCollision)
            {
                return collisionBody.MyTransform;
            }
        }
        
        return null;
    }

    private void ApplyKnockBack(Vector3 impulseForce)
    {
        if (_knocbackCoroutine != null)
        {
            StopCoroutine(_knocbackCoroutine);
        }

        _knocbackCoroutine = StartCoroutine(KnockBack(impulseForce));
    }

    private IEnumerator KnockBack(Vector3 impulseForce)
    {
        Vector3 lerpedValue = impulseForce;
        float elapsedTime = 0;
        while (elapsedTime < _knockbackTime)
        {
            lerpedValue = Vector3.Lerp(impulseForce, Vector3.zero, _animationCurve.Evaluate(elapsedTime));

            _characterController.Move(lerpedValue);

            elapsedTime += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        _knocbackCoroutine = null;
    }
}

[Serializable]
internal struct TransformByBodyCollision
{
    public Transform MyTransform;
    public BodyCollisionType BodyCollision;
}