using UnityEngine;

public class WeaponPhysicsComponent : MonoBehaviour
{
    public DamageData Damage;

    [SerializeField] private Collider _collider;

    public void SetCollisionEnable(bool enabled)
    {
        _collider.enabled = enabled;
        if (enabled)
        {
            CheckCollisionsOnFrame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageableTarget))
        {
            damageableTarget.TakeDamage(Damage);
            SetCollisionEnable(false);
        }
    }

    private void CheckCollisionsOnFrame()
    {
        Collider[] colliders = Physics.OverlapBox(_collider.bounds.center, _collider.bounds.extents,
            _collider.transform.rotation);

        foreach (Collider overlappedCollider in colliders)
        {
            if (overlappedCollider != _collider)
            {
                OnTriggerEnter(overlappedCollider);
            }
        }
    }
}
