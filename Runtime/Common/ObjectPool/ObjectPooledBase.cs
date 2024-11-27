using UnityEngine;

public class ObjectPooledBase : MonoBehaviour
{
    private bool _isAvailable = true;

    public bool IsAvailable => _isAvailable;

   public virtual void OnSpawn()
    {
        gameObject.SetActive(true);
        _isAvailable = false;
    }

    public virtual void OnDespawn()
    {
        _isAvailable = true;
        gameObject.SetActive(false);
    }
}
