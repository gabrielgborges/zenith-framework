using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPoolSystem : ServiceBase, IObjectPoolingService
{
    private Dictionary<int, List<ObjectPooledBase>> _objectsSpawnedById = new Dictionary<int, List<ObjectPooledBase>>();
    private Dictionary<int, ObjectToPoolConfiguration> _objectConfigurationById = new Dictionary<int, ObjectToPoolConfiguration>();

    [SerializeField] private ObjectToPoolConfiguration[] _objectsToPool;

    public override void Setup()
    {
        ServiceLocator.AddService<IObjectPoolingService>(this);
    }

    public override void Dispose()
    {
        ServiceLocator.RemoveService<IObjectPoolingService>(this);
    }

    public ObjectPooledBase SpawnObject(ObjectPooledBase prefab)
    {
        int prefabToSpawnId = prefab.GetInstanceID();
        if (!_objectConfigurationById.ContainsKey(prefabToSpawnId))
        {
            Debug.Log("There is no prefab with this Id in the pool: " + prefabToSpawnId);
            return null;
        }

        ObjectPooledBase objectSpawned = SpawnFromPool(prefabToSpawnId);
        if (objectSpawned != null)
        {
            objectSpawned.OnSpawn();
            return objectSpawned;
        }

        if (_objectsSpawnedById[prefabToSpawnId].Count >= _objectConfigurationById[prefabToSpawnId].FirstSpawnAmount
            && _objectConfigurationById[prefabToSpawnId].LimitSpawn)
        {
            Debug.Log("Spawned the max of prefabs");
            return null;
        }

        objectSpawned = Instantiate(prefab);
        _objectsSpawnedById[prefabToSpawnId].Add(objectSpawned);
        objectSpawned.OnSpawn();
        return objectSpawned;
    }

    private void Awake()
    {
        foreach (ObjectToPoolConfiguration objectToPool in _objectsToPool)
        {
            InitializeObject(objectToPool);
        }

        SceneManager.activeSceneChanged += HandleSceneChanged;
    }

    private void HandleSceneChanged(Scene previousScene, Scene currentScene)
    {
        foreach (int objectSpawnedId in _objectConfigurationById.Keys)
        {
            ObjectToPoolConfiguration objectConfiguration = _objectConfigurationById[objectSpawnedId];

            if (!objectConfiguration.IsPersistent && _objectsSpawnedById.ContainsKey(objectSpawnedId))
            {
                Debug.Log("despawned extra copies");
                DespawnObjectCopies(objectSpawnedId);
            }

            if (objectConfiguration.Scene == currentScene.name
                && objectConfiguration.SpanwAtSceneStart)
            {
                Debug.Log("spawned new copies");
                _objectsSpawnedById.Add(objectSpawnedId, new List<ObjectPooledBase>());
                SpawnObjectCopies(objectConfiguration.ObjectPrefab, objectSpawnedId, objectConfiguration.FirstSpawnAmount);
            }
        }    
    }

    private void InitializeObject(ObjectToPoolConfiguration objectToPool)
    {
        int objectId = objectToPool.ObjectPrefab.GetInstanceID();
        _objectConfigurationById.Add(objectId, objectToPool);
    }

    private ObjectPooledBase SpawnFromPool(int prefabId)
    {
        foreach (ObjectPooledBase objectSpawned in _objectsSpawnedById[prefabId])
        {
            if (objectSpawned.IsAvailable)
            {
                return objectSpawned;
            }
        }

        return null;
    }

    private void SpawnObjectCopies(ObjectPooledBase prefab, int prefabId, int copies)
    {
        for (int i = 0; i < copies; i++)
        {
            ObjectPooledBase objectFromPool = Instantiate(prefab);
            objectFromPool.gameObject.SetActive(false);
            _objectsSpawnedById[prefabId].Add(objectFromPool);
        }
    }

    private void DespawnObjectCopies(int objectId)
    {
        List<ObjectPooledBase> objectsSpawned = _objectsSpawnedById[objectId];

        for (int i = 0; i < objectsSpawned.Count; i++)
        {
            Destroy(objectsSpawned[i]);
        }

        _objectsSpawnedById.Remove(objectId);

    }
}
