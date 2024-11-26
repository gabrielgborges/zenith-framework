using System.Threading.Tasks;
using UnityEngine;

public class VFXObject : ObjectPooledBase
{
    [SerializeField] private ParticleSystem _visualEffect;

    public override void OnSpawn()
    {
        base.OnSpawn();
        DespawnOnAnimationEnd();
    }

    private async void DespawnOnAnimationEnd()
    {
        float durationInMiliseconds = _visualEffect.main.duration * 1000;
        await Task.Delay((int)durationInMiliseconds);
        base.OnDespawn();
    }
}
