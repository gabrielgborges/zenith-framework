using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class VisualEffectsService : BaseService, IVisualEffectsService
{
    [SerializeField] ObjectPoolSystem _objectPool;
    [SerializeField] private List<VFXByType> _vfxByType;
    
    public override void Setup()
    {
        ServiceLocator.AddService<IVisualEffectsService>(this);
    }

    public override void Dispose()
    {
        ServiceLocator.RemoveService<IVisualEffectsService>(this);
    }

    public VFXObject SpawnVFX(VFXTypes visualEffect)
    {
        CheckForObjectPoolingService();

        foreach (VFXByType vfxByType in _vfxByType)
        {
            if (vfxByType.VFXType == visualEffect)
            {
                return _objectPool.SpawnObject(vfxByType.VFXObject) as VFXObject;

            }
        }

        return null;
    }

    public void SpawnVFX(VFXTypes visualEffect, Vector3 spawnPosition)
    {
        VFXObject vfxObject = SpawnVFX(visualEffect);
        Transform vfxTransform = vfxObject.transform;
        vfxTransform.position = spawnPosition;
    }


    private void CheckForObjectPoolingService()
    {
        //if(_objectPool == null)
        //{
        //    ServiceLocator.GetService<IObjectPoolingService>();
        //}
    }
}

[Serializable]
internal struct VFXByType
{
    public VFXTypes VFXType;
    public VFXObject VFXObject;
}


