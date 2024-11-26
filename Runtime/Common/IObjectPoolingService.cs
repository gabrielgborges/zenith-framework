using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPoolingService : IService
{
    public ObjectPooledBase SpawnObject(ObjectPooledBase prefab);
}