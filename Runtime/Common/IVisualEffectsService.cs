using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVisualEffectsService : IService
{
    public VFXObject SpawnVFX(VFXTypes visualEffect);
    public void SpawnVFX(VFXTypes visualEffect, Vector3 spawnPosition);
}
