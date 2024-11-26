using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "VisualEffect", menuName = "Data/VisualEffectData", order = 1)]
public class ObjectToPoolConfiguration : ScriptableObject
{
    [SerializeField] private ObjectPooledBase _prefab;

    [SerializeField] private string _sceneName;
    [SerializeField] private int _firstSpawnAmount;
    [SerializeField] private bool _limitSpawn;
    [SerializeField] private bool _spawnAtSceneStart;
    [SerializeField] private bool _isPersistent;

    public ObjectPooledBase ObjectPrefab => _prefab;
    public string Scene => _sceneName;
    public int FirstSpawnAmount => _firstSpawnAmount;
    public bool LimitSpawn => _limitSpawn;
    public bool SpanwAtSceneStart => _spawnAtSceneStart;
    public bool IsPersistent => _isPersistent;
}
